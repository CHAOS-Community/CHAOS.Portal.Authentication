using System;
using System.Linq;
using Chaos.Portal.Authentication.Data.Model;
using Chaos.Portal.Authentication.Exception;
using Chaos.Portal.Authentication.OAuth;
using Chaos.Portal.Core.Data.Model;
using Chaos.Portal.Core.Extension;
using DotNetAuth.Profiles;

namespace Chaos.Portal.Authentication.Extension.v6
{
	public class OAuth : AExtension
	{
		public IAuthenticationModule AuthenticationModule { get; set; }

		public OAuth(IAuthenticationModule authenticationModule) : base(authenticationModule.PortalApplication)
        {
            AuthenticationModule = authenticationModule;
        }

		public LoginEndPoint GetLoginEndPoint(string callbackUrl)
		{
			return AuthenticationModule.OAuthClient.GetLoginEndPoint(callbackUrl);
		}

		public Session ProcessLogin(string callbackUrl, string responseUrl, string stateCode)
		{
			var profile = AuthenticationModule.OAuthClient.ProcessLogin(callbackUrl, responseUrl, stateCode);
			var user = GetUser(profile);
			var session = AuthenticateSession(user);

			//AuthenticationModule.OnOnUserLoggedIn(new RequestDelegate.PortalRequestArgs(Request));

			return session;
		}

		private OAuthUser GetUser(Profile profile)
		{
			return AuthenticationModule.AuthenticationRepository.OAuth.OAuthUserGet(profile.UniqueID) 
				?? CreateUser(profile);
		}

		private OAuthUser CreateUser(Profile profile)
		{
			var user = new OAuthUser();

			var existingUser = PortalRepository.UserInfoGet(null, null, profile.Email, null).FirstOrDefault();

			if (existingUser == null)
			{
				user.UserGuid = Guid.NewGuid();

				if (PortalRepository.UserCreate(user.UserGuid, profile.Email) != 1)
					throw new LoginException("Failed to create new user");
			}
			else
				user.UserGuid = existingUser.Guid;

			AuthenticationModule.AuthenticationRepository.OAuth.OAuthUserUpdate(user.UserGuid, profile.UniqueID);

			return user;
		}

		private Session AuthenticateSession(OAuthUser user)
		{
			return Request.Session != null
				   ? PortalRepository.SessionUpdate(Request.Session.Guid, user.UserGuid)
				   : PortalRepository.SessionCreate(user.UserGuid);
		}
	}
}