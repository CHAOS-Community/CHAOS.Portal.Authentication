using System;
using CHAOS.Extensions;
using Chaos.Portal.Authentication.OAuth;
using Chaos.Portal.Authentication.Wayf;

namespace Chaos.Portal.Authentication
{
	using System.Collections.Generic;
	using Configuration;
	using Core.Request;
	using Data;
	using Extension;
	using Core;
	using Core.Extension;
	using Facebook;

	public class AuthenticationModule : IAuthenticationModule
	{
		#region Fields

		private const string CONFIGURATION_NAME = "Authentication";

		private IDictionary<Type, object> _userInfoUpdateListeners;

		public event RequestDelegate.PortalRequestHandler OnUserLoggedIn;
		public event WayfHandler OnWayfUserLoggedIn;
		public event RequestDelegate.PortalRequestHandler OnUserCreated;

		#endregion

		#region Properties

		public IExtension[] Extensions { get; private set; }
		public IAuthenticationRepository AuthenticationRepository { get; private set; }
		public IPortalApplication PortalApplication { get; private set; }
		public IFacebookClient FacebookClient { get; set; }
		public IOAuthClient OAuthClient { get; set; }
		public AuthenticationSettings Settings { get; private set; }

		#endregion

		#region Implementation of IModule

		public void Load(IPortalApplication portalApplication)
		{
			Settings = portalApplication.GetSettings<AuthenticationSettings>(CONFIGURATION_NAME);

			_userInfoUpdateListeners = new Dictionary<Type, object>();

			AuthenticationRepository = new AuthenticationRepository(Settings.ConnectionString);
			PortalApplication = portalApplication;

			if (Settings.Facebook != null)
				FacebookClient = new FacebookClient(Settings.Facebook);

			if (Settings.OAuth != null)
				OAuthClient = new OAuthClient(Settings.OAuth);

			portalApplication.MapRoute("/v5/EmailPassword", () => new EmailPassword(PortalApplication, AuthenticationRepository, Settings.Password, this));
			portalApplication.MapRoute("/v5/SecureCookie", () => new SecureCookie(PortalApplication, AuthenticationRepository));
			portalApplication.MapRoute("/v6/EmailPassword", () => new EmailPassword(PortalApplication, AuthenticationRepository, Settings.Password, this));
			portalApplication.MapRoute("/v6/SecureCookie", () => new SecureCookie(PortalApplication, AuthenticationRepository));
			portalApplication.MapRoute("/v6/AuthKey", () => new AuthKey(PortalApplication, AuthenticationRepository));
			portalApplication.MapRoute("/v6/OAuth", () => new Extension.v6.OAuth(this));
			portalApplication.MapRoute("/v6/Wayf", () => new Extension.Wayf(PortalApplication, AuthenticationRepository, new WayfFilter("WayfFilter.json"), this));
			portalApplication.MapRoute("/v6/Facebook", () => new Extension.v6.Facebook(this));
		}

		public virtual void OnOnUserLoggedIn(RequestDelegate.PortalRequestArgs args)
		{
			var handler = OnUserLoggedIn;
			if (handler != null) handler(this, args);
		}

		public virtual void OnOnUserCreated(RequestDelegate.PortalRequestArgs args)
		{
			var handler = OnUserCreated;
			if (handler != null) handler(this, args);
		}

		public virtual void OnOnWayfUserLoggedIn(WayfProfileArgs args)
		{
			var handler = OnWayfUserLoggedIn;
			if (handler != null) handler(this, args);
		}

		public void AddUserInfoUpdateListener<T>(Action<UserInfoUpdate<T>> listener)
		{
			listener.ValidateIsNotNull("listner");
			var type = typeof (T);
			IList<Action<UserInfoUpdate<T>>> list;

			if (_userInfoUpdateListeners.ContainsKey(type))
				list = (IList<Action<UserInfoUpdate<T>>>) _userInfoUpdateListeners[type];
			else
			{
				list = new List<Action<UserInfoUpdate<T>>>();
				_userInfoUpdateListeners[type] = list;
			}

			list.Add(listener);
		}

		public void OnUserInfoUpdate<T>(Guid userGuid, T userInfo)
		{
			var type = typeof (T);
			if (!_userInfoUpdateListeners.ContainsKey(type)) return;

			var list = (IList<Action<UserInfoUpdate<T>>>) _userInfoUpdateListeners[type];
			var update = new UserInfoUpdate<T>(userGuid, userInfo);

			foreach (var listener in list)
				listener(update);
		}

		#endregion
	}

	public delegate void WayfHandler(object sender, WayfProfileArgs args);

	public class WayfProfileArgs
	{
		public Guid UserId { get; set; }
		public IDictionary<string, IList<string>> AttributesObject { get; set; }

		public WayfProfileArgs(Guid userId, IDictionary<string, IList<string>> attributesObject)
		{
			UserId = userId;
			AttributesObject = attributesObject;
		}
	}
}