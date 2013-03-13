namespace Chaos.Portal.Authentication.Tests
{
    using Chaos.Portal.Authentication.Extension;
    using Chaos.Portal.Data.Dto;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class AuthenticationModuleTest : TestBase
    {
        [Test]
        public void Load_EmailPasswordExtension_AddToPortalApplication()
        {
            var module = new AuthenticationModule();
            var config = Make_ModuleConfig();
            PortalRepository.Setup(m => m.ModuleGet("Authentication")).Returns(config);

            module.Load(PortalApplication.Object);

            PortalApplication.Verify(m => m.AddExtension("EmailPassword", It.IsAny<EmailPassword>()));
        }

        [Test]
        public void Load_SecureCookieExtension_AddToPortalApplication()
        {
            var module = new AuthenticationModule();
            var config = Make_ModuleConfig();
            PortalRepository.Setup(m => m.ModuleGet("Authentication")).Returns(config);

            module.Load(PortalApplication.Object);

            PortalApplication.Verify(m => m.AddExtension("SecureCookie", It.IsAny<SecureCookie>()));
        }

        private static Module Make_ModuleConfig()
        {
            return new Module
                {
                    Configuration = "<Settings><ConnectionString>connectionstring</ConnectionString></Settings>"
                };
        }
    }
}