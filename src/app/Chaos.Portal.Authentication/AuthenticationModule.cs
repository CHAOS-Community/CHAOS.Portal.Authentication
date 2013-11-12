namespace Chaos.Portal.Authentication
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    using Chaos.Portal.Authentication.Data;
    using Chaos.Portal.Authentication.Extension;
    using Chaos.Portal.Core;
    using Chaos.Portal.Core.Exceptions;
    using Chaos.Portal.Core.Extension;
    using Chaos.Portal.Core.Module;

    public class AuthenticationModule : IModule
    {
        public AuthenticationRepository AuthenticationRepository { get; private set; }

        public IPortalApplication PortalApplication { get; private set; }

        #region Fields

        private const string CONFIGURATION_NAME = "Authentication";

        #endregion
        #region Properties

        public IExtension[] Extensions { get; private set; }

        #endregion
        #region Implementation of IModule

        public IEnumerable<string> GetExtensionNames(Protocol version)
        {
            yield return "EmailPassword";
            yield return "SecureCookie";
            yield return "SiteAccess";
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
                    case "SiteAccess":
                        return new SiteAccess(PortalApplication, AuthenticationRepository);
                }
            }

            throw new ProtocolVersionException();
        }

        public IExtension GetExtension<TExtension>(Protocol version) where TExtension : IExtension
        {
            return GetExtension(version, typeof(TExtension).Name);
        }

        public void Load(IPortalApplication portalApplication)
        {
            var configuration    = portalApplication.PortalRepository.ModuleGet(CONFIGURATION_NAME);
            var connectionString = XDocument.Parse(configuration.Configuration).Descendants("ConnectionString").First().Value;

            AuthenticationRepository = new AuthenticationRepository(connectionString);
            PortalApplication        = portalApplication;
        }

        #endregion
    }
}
