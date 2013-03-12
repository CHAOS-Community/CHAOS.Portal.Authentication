namespace Chaos.Portal.Authentication
{
    using Chaos.Portal;
    using Chaos.Portal.Authentication.Extension;
    using Chaos.Portal.Module;
    using Chaos.Portal.Extension;

    public class AuthenticationModule : IModule
    {
        #region Fields

        private const string CONFIGURATION_NAME = "EmailPassword";

        #endregion
        #region Properties

        public IExtension[] Extensions { get; private set; }

        #endregion
        #region Implementation of IModule

        public void Load(IPortalApplication portalApplication)
        {
            var configuration = portalApplication.PortalRepository.ModuleGet(CONFIGURATION_NAME);

            Extensions = new IExtension[1];
            Extensions[0] = new EmailPassword();

            foreach (var extension in Extensions)
            {
                extension.WithPortalApplication(portalApplication);
                extension.WithConfiguration(configuration.Configuration);
            }

            portalApplication.AddExtension("EmailPassword", Extensions[0]);
        }

        #endregion
    }
}
