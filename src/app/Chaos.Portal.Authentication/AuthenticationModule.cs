namespace Chaos.Portal.Authentication
{
    using System.Collections.Generic;
    using System.Xml.Linq;
    using CHAOS.Serialization.Standard;
    using Configuration;
    using Data;
    using Extension;
    using Core;
    using Core.Exceptions;
    using Core.Extension;
    using Core.Module;
    using Facebook;

    public class AuthenticationModule : IModule
    {
        #region Fields

        private const string CONFIGURATION_NAME = "Authentication";

        #endregion
        #region Properties

        public IExtension[] Extensions { get; private set; }
        public AuthenticationRepository AuthenticationRepository { get; private set; }

        public IPortalApplication PortalApplication { get; private set; }

        private IFacebookClient FacebookClient { get; set; }

        #endregion

        #region Implementation of IModule

        public void Load(IPortalApplication portalApplication)
        {
            var settings = GetSettings(portalApplication);

            AuthenticationRepository = new AuthenticationRepository(settings.ConnectionString);
            PortalApplication = portalApplication;
            FacebookClient = new FacebookClient(settings.Facebook);
        }

        private static Settings GetSettings(IPortalApplication portalApplication)
        {
            var configuration = portalApplication.PortalRepository.ModuleGet(CONFIGURATION_NAME);
            var xdoc = XDocument.Parse(configuration.Configuration);
            var settings = SerializerFactory.XMLSerializer.Deserialize<Settings>(xdoc);
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
						return new Extension.v6.Facebook(PortalApplication, AuthenticationRepository, FacebookClient);
                }
            }

            throw new ProtocolVersionException();
        }

        public IExtension GetExtension<TExtension>(Protocol version) where TExtension : IExtension
        {
            return GetExtension(version, typeof(TExtension).Name);
        }

        #endregion
    }
}
