namespace Chaos.Portal.Authentication
{
    using System.Collections.Generic;
    using System.Xml.Linq;
    using CHAOS.Serialization.Standard;
    using Configuration;
    using Core.Request;
    using Data;
    using Extension;
    using Core;
    using Core.Exceptions;
    using Core.Extension;
    using Facebook;

    public class AuthenticationModule : IAuthenticationModule
    {
        #region Fields

        private const string CONFIGURATION_NAME = "Authentication";

        public event RequestDelegate.PortalRequestHandler OnUserLoggedIn;
        public event RequestDelegate.PortalRequestHandler OnUserCreated;

        #endregion
        #region Properties

        public IExtension[] Extensions { get; private set; }
        public IAuthenticationRepository AuthenticationRepository { get; private set; }
        public IPortalApplication PortalApplication { get; private set; }
        public IFacebookClient FacebookClient { get; set; }
        public AuthenticationSettings AuthenticationSettings { get; private set; }

        #endregion

        #region Implementation of IModule

        public void Load(IPortalApplication portalApplication)
        {
            AuthenticationSettings = GetSettings(portalApplication);
            
            AuthenticationRepository = new AuthenticationRepository(AuthenticationSettings.ConnectionString);
            PortalApplication = portalApplication;
            FacebookClient = new FacebookClient(AuthenticationSettings.Facebook);
        }

        private static AuthenticationSettings GetSettings(IPortalApplication portalApplication)
        {
            var configuration = portalApplication.PortalRepository.ModuleGet(CONFIGURATION_NAME);
            var xdoc = XDocument.Parse(configuration.Configuration);
            var settings = SerializerFactory.XMLSerializer.Deserialize<AuthenticationSettings>(xdoc);
            return settings;
        }

        public IEnumerable<string> GetExtensionNames(Protocol version)
        {
            yield return "EmailPassword";
            yield return "SecureCookie";
            yield return "AuthKey";
            yield return "Wayf";
            yield return "Facebook";
        }

        public IExtension GetExtension(Protocol version, string name)
        {
            if(version == Protocol.V5)
            {
                switch(name)
                {
                    case "EmailPassword":
                        return new EmailPassword(PortalApplication, AuthenticationRepository);
                    case "SecureCookie":
                        return new SecureCookie(PortalApplication, AuthenticationRepository);
                }
            }

            if (version == Protocol.V6)
            {
                switch (name)
                {
                    case "EmailPassword":
                        return new EmailPassword(PortalApplication, AuthenticationRepository);
                    case "SecureCookie":
                        return new SecureCookie(PortalApplication, AuthenticationRepository);
                    case "AuthKey":
                        return new AuthKey(PortalApplication, AuthenticationRepository);
					case "Wayf":
						return new Wayf(PortalApplication, AuthenticationRepository);
                    case "Facebook":
						return new Extension.v6.Facebook(this);
                }
            }

            throw new ProtocolVersionException();
        }

        public IExtension GetExtension<TExtension>(Protocol version) where TExtension : IExtension
        {
            return GetExtension(version, typeof(TExtension).Name);
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

        #endregion
    }
}
