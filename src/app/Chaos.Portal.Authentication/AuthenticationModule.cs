using System;
using CHAOS.Extensions;
using Chaos.Portal.Authentication.OAuth;

namespace Chaos.Portal.Authentication
{
    using System.Collections.Generic;
    using System.Xml.Linq;
    using CHAOS.Serialization.Standard;
    using Configuration;
    using Core.Exceptions;
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
        public event RequestDelegate.PortalRequestHandler OnUserCreated;

        #endregion
        #region Properties

        public IExtension[] Extensions { get; private set; }
        public IAuthenticationRepository AuthenticationRepository { get; private set; }
        public IPortalApplication PortalApplication { get; private set; }
        public IFacebookClient FacebookClient { get; set; }
	    public IOAuthClient OAuthClient { get; set; }
	    public AuthenticationSettings AuthenticationSettings { get; private set; }

        #endregion

        #region Implementation of IModule

        public void Load(IPortalApplication portalApplication)
        {
            AuthenticationSettings = GetSettings(portalApplication);

			_userInfoUpdateListeners = new Dictionary<Type, object>();
            
            AuthenticationRepository = new AuthenticationRepository(AuthenticationSettings.ConnectionString);
            PortalApplication = portalApplication;

            if (AuthenticationSettings.Facebook != null)
                FacebookClient = new FacebookClient(AuthenticationSettings.Facebook);

            if (AuthenticationSettings.OAuth != null)
			    OAuthClient = new OAuthClient(AuthenticationSettings.OAuth);

            portalApplication.MapRoute("/v5/EmailPassword", () => new EmailPassword(PortalApplication, AuthenticationRepository));
            portalApplication.MapRoute("/v5/SecureCookie", () => new SecureCookie(PortalApplication, AuthenticationRepository));
            
            portalApplication.MapRoute("/v6/EmailPassword", () => new EmailPassword(PortalApplication, AuthenticationRepository));
            portalApplication.MapRoute("/v6/SecureCookie", () => new SecureCookie(PortalApplication, AuthenticationRepository));
            portalApplication.MapRoute("/v6/AuthKey", () => new AuthKey(PortalApplication, AuthenticationRepository));
            portalApplication.MapRoute("/v6/OAuth", () => new Extension.v6.OAuth(this));
            portalApplication.MapRoute("/v6/Wayf", () => new Wayf(PortalApplication, AuthenticationRepository));
            portalApplication.MapRoute("/v6/Facebook", () => new Extension.v6.Facebook(this));
        }

        private static AuthenticationSettings GetSettings(IPortalApplication portalApplication)
        {
            try
            {
                var configuration = portalApplication.PortalRepository.ModuleGet(CONFIGURATION_NAME);
                var xdoc = XDocument.Parse(configuration.Configuration);
                var settings = SerializerFactory.XMLSerializer.Deserialize<AuthenticationSettings>(xdoc);

                if (string.IsNullOrEmpty(settings.ConnectionString))
                    throw new ModuleConfigurationMissingException("MCM configuration is invalid.");

                return settings;
            }
            catch (ArgumentException e)
            {
                var config = new AuthenticationSettings
                    {
                        ConnectionString = "",
                        Facebook = new FacebookSettings
                            {
                                AppId = "",
                                AppSecret = ""
                            },
                        OAuth = new OAuthSettings
                            {
                                AuthorizationEndpoint = "",
                                ClientId = "",
                                ClientSecret = "",
                                TokenEndpoint = "",
                                UserInfoEndpoint = ""
                            }
                    };
                var module = new Core.Data.Model.Module
                    {
                        Name = CONFIGURATION_NAME,
                        Configuration = SerializerFactory.XMLSerializer.Serialize(config).ToString()
                    };

                portalApplication.PortalRepository.Module.Set(module);

                throw new ModuleConfigurationMissingException("Authentication configuration was missing, a template was created in the database");
            }
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

			var list = (IList<Action<UserInfoUpdate<T>>>)_userInfoUpdateListeners[type];
		    var update = new UserInfoUpdate<T>(userGuid, userInfo);

		    foreach (var listener in list)
			    listener(update);
	    }

	    #endregion
    }
}
