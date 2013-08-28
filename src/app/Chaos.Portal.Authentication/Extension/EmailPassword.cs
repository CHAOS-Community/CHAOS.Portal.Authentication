using Chaos.Portal.Core.Exceptions;

namespace Chaos.Portal.Authentication.Extension
{
    using System;
    using System.Linq;
    using System.Net.Mail;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Xsl;

    using Chaos.Portal.Authentication.Data;
    using Chaos.Portal.Core;
    using Chaos.Portal.Core.Data.Model;
    using Chaos.Portal.Core.Extension;
    using Chaos.Portal.Authentication.Exception;

    public class EmailPassword : AExtension
    {
        #region Properties

        public MailAddress          FromEmailAddress { get; set; }
        public string               ChangePasswordRequestSubject { get; set; }
        public string               SmtpPassword { get; set; }
        public XslCompiledTransform ChangePasswordRequestEmailXslt { get; set; }

        private IAuthenticationRepository AuthenticationRepository { get; set; }

        #endregion
        #region Initialization

        public EmailPassword(IPortalApplication portalApplication, IAuthenticationRepository authenticationRepository): base(portalApplication)
        {
            AuthenticationRepository = authenticationRepository;
        }

        
        #endregion
        #region Login (Email/password)

        public UserInfo Login(string email, string password )
        {
            var user = PortalRepository.UserInfoGet(email);

            if(user == null) throw new LoginException("Login failed, either email or password is incorrect");

            var res = AuthenticationRepository.EmailPasswordGet(user.Guid, GeneratePasswordHash(password));

            if(res == null) throw new LoginException( "Login failed, either email or password is incorrect" );

            var result = PortalRepository.SessionUpdate(Request.Session.Guid, user.Guid);

            if(result == null) throw new LoginException("Session could not be updated");

            return PortalRepository.UserInfoGet(null, Request.Session.Guid, null, null).First();
        }

        #endregion
        #region Change Password

	    public ScalarResult SetPassword(Guid userGuid, string newPassword)
	    {
		   if (!Request.User.HasPermission(SystemPermissons.UserManager))
			   throw new InsufficientPermissionsException("Current user must be user manager to set password");
		    
			var result = AuthenticationRepository.EmailPasswordUpdate(userGuid, GeneratePasswordHash(newPassword));

			if (result != 1 && result != 2)
				throw new Exception("Failed to set password for user. Error code: " + result);

			return new ScalarResult(1);
	    }

//        public ScalarResult ChangePasswordRequest(ICallContext callContext, string email, string password, string url)
//        {
//            var user = PortalRepository.GetUserInfo(email);
//
//            var guid = new guid();
//            var xml  = string.Format("<ChangePassword UserGUID=\"{0}\" PasswordHash=\"{1}\" />", user.guid, GeneratePasswordHash(password));
//            PortalRepository.CreateTicket(guid, (uint) Chaos.Portal.Data.TicketType.ChangePassword, xml, null);
//
//            // TODO: Make Send mail part of the portal SDK
//            var message = new MailMessage();
//
//            message.To.Add(new MailAddress(user.Email));
//            message.From       = FromEmailAddress;
//            message.Subject    = ChangePasswordRequestSubject;
//            message.Body       = GenerateChangePasswordRequestEmail(guid, url).ToString(SaveOptions.DisableFormatting);
//            message.IsBodyHtml = true;
//                
//            var smtp  = new SmtpClient("smtp.gmail.com", 587);
//            smtp.EnableSsl   = true;
//            smtp.Credentials = new NetworkCredential(FromEmailAddress.Address, SmtpPassword);
//
//            smtp.Send(message);
//
//            return new ScalarResult(1);
//        }

        //[Datatype("EmailPassword","ChangePassword")]
        //public Session ChangePassword( ICallContext callContext, string ticketGUID )
        //{
        //    using( var db = new PortalEntities() )
        //    {
        //        var ticket   = db.Ticket_Get( new UUID( ticketGUID ).ToByteArray() ).First();
        //        var xml      = XDocument.Parse( ticket.XML ).Root;
        //        var userGUID = new UUID( xml.Attribute("UserGUID").Value );
        //        var password = xml.Attribute("PasswordHash").Value;

        //        db.Ticket_Use( new UUID( ticketGUID ).ToByteArray() );
        //        db.Session_Update( userGUID.ToByteArray(), callContext.User.SessionGUID.ToByteArray(), null );

        //        using( EmailPasswordEntities edb = NewEmailPasswordDataContext )
        //        {
        //            if( edb.EmailPassword_Get( userGUID.ToByteArray(), null ).FirstOrDefault() == null )
        //                edb.EmailPassword_Create( userGUID.ToByteArray(), password );
        //            else
        //                edb.EmailPassword_Update( userGUID.ToByteArray(), password );
        //        }

        //        int? totalCount = 1;

        //        return db.Session_Get( callContext.Session.GUID.ToByteArray(), null ).First();
        //    }
        //}

        private XDocument GenerateChangePasswordRequestEmail(Guid ticketGUID, string url)
        {
            var source = XDocument.Parse(string.Format("<Xml><URL>{0}</URL></Xml>", HttpUtility.HtmlDecode(url).Replace("$Token$", ticketGUID.ToString()).Replace("&", "&amp;")));
            var result = new XDocument();

            using (XmlWriter writer = result.CreateWriter())
            {
                ChangePasswordRequestEmailXslt.Transform(source.CreateReader(), writer);
            }

            return result;
        }

        #endregion
        #region Helper methods

        private string GeneratePasswordHash( string password )
        {
            var byteHash = SHA1.Create().ComputeHash(Encoding.Unicode.GetBytes(password));

             return BitConverter.ToString( byteHash ).Replace("-", "").ToLower();
        }

        #endregion
    }
}
