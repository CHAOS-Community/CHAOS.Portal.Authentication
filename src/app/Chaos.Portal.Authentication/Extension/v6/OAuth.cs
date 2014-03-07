using System;
using System.Linq;
using Chaos.Portal.Authentication.Data.Model;
using Chaos.Portal.Authentication.Exception;
using Chaos.Portal.Core.Data.Model;
using Chaos.Portal.Core.Exceptions;
using Chaos.Portal.Core.Extension;

namespace Chaos.Portal.Authentication.Extension.v6
{
	public class OAuth : AExtension
	{
		public IAuthenticationModule AuthenticationModule { get; set; }

		public OAuth(IAuthenticationModule authenticationModule) : base(authenticationModule.PortalApplication)
        {
            AuthenticationModule = authenticationModule;
        }

		public UserInfo Login(string oAuthId, string email, Guid sessionGuidToAuthenticate)
		{
			if (!Request.User.HasPermission(SystemPermissons.Manage)) throw new InsufficientPermissionsException("Only managers can authenticate sessions");

			var oAuthUser = AuthenticationModule.AuthenticationRepository.OAuth.OAuthUserGet(oAuthId);

			if (oAuthUser == null)
			{
				oAuthUser = new OAuthUser();

				var existingUser = PortalRepository.UserInfoGet(null, null, email, null).FirstOrDefault();

				if (existingUser == null)
				{
					oAuthUser.UserGuid = Guid.NewGuid();

					if (PortalRepository.UserCreate(oAuthUser.UserGuid, email) != 1) throw new LoginException("Failed to create new user");
				}
				else
					oAuthUser.UserGuid = existingUser.Guid;

				AuthenticationModule.AuthenticationRepository.OAuth.OAuthUserUpdate(oAuthUser.UserGuid, oAuthId);
			}

			var result = PortalRepository.SessionUpdate(sessionGuidToAuthenticate, oAuthUser.UserGuid);

			if (result == null) throw new LoginException("Session could not be updated");

			return PortalRepository.UserInfoGet(null, sessionGuidToAuthenticate, null, null).First();
		}
	}
}