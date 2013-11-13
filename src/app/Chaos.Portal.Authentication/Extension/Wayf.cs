using System;
using System.Linq;
using Chaos.Portal.Authentication.Data;
using Chaos.Portal.Authentication.Data.Dto;
using Chaos.Portal.Authentication.Exception;
using Chaos.Portal.Core;
using Chaos.Portal.Core.Data.Model;
using Chaos.Portal.Core.Extension;

namespace Chaos.Portal.Authentication.Extension
{
	public class Wayf : AExtension
	{
		private IAuthenticationRepository AuthenticationRepository { get; set; }

		public Wayf(IPortalApplication portalApplication, IAuthenticationRepository authenticationRepository) : base(portalApplication)
		{
			AuthenticationRepository = authenticationRepository;
		}

		public UserInfo Login(string wayfId, string email)
		{
			var wayfProfile = AuthenticationRepository.WayfProfileGet(wayfId);

			//TODO: Verify valid wayf request

			if (wayfProfile == null)
			{
				wayfProfile = new WayfUser();

				var existingUser = PortalRepository.UserInfoGet(null, null,email, null).FirstOrDefault();

				if (existingUser == null)
				{
					wayfProfile.UserGuid = Guid.NewGuid();

					if (PortalRepository.UserCreate(wayfProfile.UserGuid, email) != 1) throw new LoginException("Failed to create new user");
				}
				else
					wayfProfile.UserGuid = existingUser.Guid;

				AuthenticationRepository.WayfProfileUpdate(wayfProfile.UserGuid, wayfId);
			}

			var result = PortalRepository.SessionUpdate(Request.Session.Guid, wayfProfile.UserGuid);

			if (result == null) throw new LoginException("Session could not be updated");

			return PortalRepository.UserInfoGet(null, Request.Session.Guid, null, null).First();
		}
	}
}