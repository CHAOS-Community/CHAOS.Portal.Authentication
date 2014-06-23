using Chaos.Portal.Core.Exceptions;

namespace Chaos.Portal.Authentication.Extension
{
    using System;
    using System.Linq;
    using System.Net.Mail;
    using System.Xml.Xsl;
    using Configuration;
    using Data;
    using Core;
    using Core.Data.Model;
    using Core.Extension;
    using Domain;
    using Exception;

    public class EmailPassword : AExtension
    {
        #region Properties

        public MailAddress          FromEmailAddress { get; set; }
        public string               ChangePasswordRequestSubject { get; set; }
        public string               SmtpPassword { get; set; }
        public XslCompiledTransform ChangePasswordRequestEmailXslt { get; set; }

        private IAuthenticationRepository AuthenticationRepository { get; set; }
        public PasswordSettings Settings { get; set; }

        #endregion
        #region Initialization

        public EmailPassword(IPortalApplication portalApplication, IAuthenticationRepository authenticationRepository, PasswordSettings settings): base(portalApplication)
        {
            AuthenticationRepository = authenticationRepository;
            Settings = settings;
        }

        #endregion
        #region Login (Email/password)

        public UserInfo Login(string email, string password )
        {
            var user = GetUserByEmail(email);

            VerifyPasswordMatches(password, user);
            AuthenticateSessionWithUser(user);

            return PortalRepository.UserInfoGet(null, Request.Session.Guid, null, null).First();
        }

        private UserInfo GetUserByEmail(string email)
        {
            try
            {
                return PortalRepository.UserInfoGet(email);
            }
            catch (Exception e)
            {
                throw new LoginException("Login failed, either email or password is incorrect", e);
            }
        }

        private void VerifyPasswordMatches(string password, UserInfo user)
        {
            var hashed = GeneratePasswordHash(password, user.Guid.ToString());
            var res = AuthenticationRepository.EmailPasswordGet(user.Guid, hashed);

            if (res == null) throw new LoginException("Login failed, either email or password is incorrect");
        }

        private void AuthenticateSessionWithUser(UserInfo user)
        {
            var result = PortalRepository.SessionUpdate(Request.Session.Guid, user.Guid);

            if (result == null) throw new LoginException("Session could not be updated");
        }

        #endregion
        #region Change Password

	    public ScalarResult SetPassword(Guid userGuid, string newPassword)
	    {
		   if (!Request.User.HasPermission(SystemPermissons.UserManager))
			   throw new InsufficientPermissionsException("Current user must be user manager to set password");
		    
			var result = AuthenticationRepository.EmailPasswordUpdate(userGuid, GeneratePasswordHash(newPassword, userGuid.ToString()));

			if (result != 1 && result != 2)
				throw new Exception("Failed to set password for user. Error code: " + result);

			return new ScalarResult(1);
	    }

        #endregion
        #region Helper methods

        private string GeneratePasswordHash( string password, string salt )
        {
            return new PasswordHelper(Settings).GenerateHash(password, salt);
        }

        #endregion
    }
}
