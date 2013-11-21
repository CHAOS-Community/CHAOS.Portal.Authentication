using System;
using System.Linq;
using Chaos.Portal.Authentication.Data;
using Chaos.Portal.Authentication.Exception;
using Chaos.Portal.Core;
using Chaos.Portal.Core.Data.Model;
using Chaos.Portal.Core.Exceptions;
using Chaos.Portal.Core.Extension;

namespace Chaos.Portal.Authentication.Extension
{
    using Data.Model;

    public class Wayf : AExtension
	{
		private IAuthenticationRepository AuthenticationRepository { get; set; }

		public Wayf(IPortalApplication portalApplication, IAuthenticationRepository authenticationRepository) : base(portalApplication)
		{
			AuthenticationRepository = authenticationRepository;
		}

		public UserInfo Login(string wayfId, string email, Guid sessionGuidToAuthenticate)
		{
			if(!Request.User.HasPermission(SystemPermissons.Manage)) throw new InsufficientPermissionsException("Only managers can authenticate sessions");

			var wayfProfile = AuthenticationRepository.WayfProfileGet(wayfId);

			if (wayfProfile == null)
			{
				wayfProfile = new WayfUser();

				var existingUser = PortalRepository.UserInfoGet(null, null, email, null).FirstOrDefault();

				if (existingUser == null)
				{
					wayfProfile.UserGuid = Guid.NewGuid();

					if (PortalRepository.UserCreate(wayfProfile.UserGuid, email) != 1) throw new LoginException("Failed to create new user");
				}
				else
					wayfProfile.UserGuid = existingUser.Guid;

				AuthenticationRepository.WayfProfileUpdate(wayfProfile.UserGuid, wayfId);
			}

			var result = PortalRepository.SessionUpdate(sessionGuidToAuthenticate, wayfProfile.UserGuid);

			if (result == null) throw new LoginException("Session could not be updated");

			return PortalRepository.UserInfoGet(null, sessionGuidToAuthenticate, null, null).First();
		}
	}
}