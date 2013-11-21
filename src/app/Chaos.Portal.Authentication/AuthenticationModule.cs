namespace Chaos.Portal.Authentication
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    using Data;
    using Extension;
    using Core;
    using Core.Exceptions;
    using Core.Extension;
    using Core.Module;
    using Extension.v6;
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
            var configuration    = portalApplication.PortalRepository.ModuleGet(CONFIGURATION_NAME);
            var connectionString = XDocument.Parse(configuration.Configuration).Descendants("ConnectionString").First().Value;
            
            AuthenticationRepository = new AuthenticationRepository(connectionString);
            PortalApplication = portalApplication;
            FacebookClient = new FacebookClient();
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
