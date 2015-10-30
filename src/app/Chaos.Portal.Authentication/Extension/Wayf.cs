using System;
using System.Collections.Generic;
using System.Linq;
using Chaos.Portal.Authentication.Data;
using Chaos.Portal.Authentication.Exception;
using Chaos.Portal.Authentication.Wayf;
using Chaos.Portal.Core;
using Chaos.Portal.Core.Data.Model;
using Chaos.Portal.Core.Exceptions;
using Chaos.Portal.Core.Extension;
using Chaos.Portal.Core.Request;
using Newtonsoft.Json;

namespace Chaos.Portal.Authentication.Extension
{
	using Data.Model;

	public class Wayf : AExtension
	{
		private readonly IWayfFilter _wayfFilter;
		private IAuthenticationRepository AuthenticationRepository { get; set; }
		public IAuthenticationModule AuthenticationModule { get; set; }

		public Wayf(IPortalApplication portalApplication, IAuthenticationRepository authenticationRepository,
		            IWayfFilter wayfFilter, IAuthenticationModule authenticationModule) : base(portalApplication)
		{
			_wayfFilter = wayfFilter;
			AuthenticationRepository = authenticationRepository;
			AuthenticationModule = authenticationModule;
		}

		public UserInfo Login(string attributes, Guid sessionGuidToAuthenticate)
		{
			if (!Request.User.HasPermission(SystemPermissons.Manage))
				throw new InsufficientPermissionsException("Only managers can authenticate sessions");

			var attributesObject = JsonConvert.DeserializeObject<IDictionary<string, IList<string>>>(attributes);

			if (!attributesObject.ContainsKey("eduPersonTargetedID") || attributesObject["eduPersonTargetedID"].Count == 0)
				throw new LoginException("Missing eduPersonTargetedID from Wayf attributes");
			if (attributesObject["eduPersonTargetedID"][0] == null)
				throw new LoginException(string.Format("First value in eduPersonTargetedID is null (contained {0} value(s))",
				                                       attributesObject["eduPersonTargetedID"].Count));
			if (!_wayfFilter.Validate(attributesObject)) throw new WayfUserNotAllowedException(attributes);

			var wayfId = attributesObject["eduPersonTargetedID"][0];
			var wayfUser = AuthenticationRepository.WayfProfileGet(wayfId);

			if (wayfUser == null)
			{
				wayfUser = new WayfUser();

				var email = attributesObject.ContainsKey("mail") && attributesObject["mail"].Count != 0 &&
				            !string.IsNullOrWhiteSpace(attributesObject["mail"][0])
					            ? attributesObject["mail"][0]
					            : wayfId;
				var existingUser = PortalRepository.UserInfoGet(null, null, email, null).FirstOrDefault();

				if (existingUser == null)
				{
					wayfUser.UserGuid = Guid.NewGuid();

					if (PortalRepository.UserCreate(wayfUser.UserGuid, email) != 1)
						throw new LoginException("Failed to create new user");
				}
				else
					wayfUser.UserGuid = existingUser.Guid;

				AuthenticationRepository.WayfProfileUpdate(wayfUser.UserGuid, wayfId);
			}

			var result = PortalRepository.SessionUpdate(sessionGuidToAuthenticate, wayfUser.UserGuid);

			if (result == null) throw new LoginException("Session could not be updated");

			AuthenticationModule.OnOnUserLoggedIn(new RequestDelegate.PortalRequestArgs(Request));
			AuthenticationModule.OnOnWayfUserLoggedIn(new WayfProfileArgs(wayfUser.UserGuid, attributesObject));

			return PortalRepository.UserInfoGet(null, sessionGuidToAuthenticate, null, null).First();
		}
	}
}