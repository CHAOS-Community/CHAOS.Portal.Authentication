namespace Chaos.Portal.Authentication
{
    using System.Linq;
    using System.Xml.Linq;

    using Chaos.Portal;
    using Chaos.Portal.Authentication.Data;
    using Chaos.Portal.Authentication.Extension;
    using Chaos.Portal.Module;
    using Chaos.Portal.Extension;

    public class AuthenticationModule : IModule
    {
        #region Fields

        private const string CONFIGURATION_NAME = "Authentication";

        #endregion
        #region Properties

        public IExtension[] Extensions { get; private set; }

        #endregion
        #region Implementation of IModule

        public void Load(IPortalApplication portalApplication)
        {
            var configuration    = portalApplication.PortalRepository.ModuleGet(CONFIGURATION_NAME);
            var connectionString = XDocument.Parse(configuration.Configuration).Descendants("ConnectionString").First().Value;

            var authenticationRepository = new AuthenticationRepository(connectionString);

            portalApplication.AddExtension("EmailPassword", new EmailPassword(authenticationRepository).WithPortalApplication(portalApplication));
            portalApplication.AddExtension("SecureCookie", new SecureCookie(authenticationRepository).WithPortalApplication(portalApplication));
        }

        #endregion
    }
}
