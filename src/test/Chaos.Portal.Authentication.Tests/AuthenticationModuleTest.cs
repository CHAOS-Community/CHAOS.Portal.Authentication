namespace Chaos.Portal.Authentication.Tests
{
    using System.Linq;

    using Chaos.Portal.Authentication.Extension;
    using Chaos.Portal.Core;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class AuthenticationModuleTest : TestBase
    {
        [Test]
        public void GetExtensionNames_V5_ReturnListOfExtensionNames()
        {
            var module = new AuthenticationModule();

            var results = module.GetExtensionNames(Protocol.V5).ToList();

            Assert.That(results[0], Is.EqualTo("EmailPassword"));
            Assert.That(results[1], Is.EqualTo("SecureCookie"));
        }

        [Test]
        public void GetExtensionNames_V6_ReturnListOfExtensionNames()
        {
            var module = new AuthenticationModule();

            var results = module.GetExtensionNames(Protocol.V6).ToList();

            Assert.That(results[0], Is.EqualTo("EmailPassword"));
            Assert.That(results[1], Is.EqualTo("SecureCookie"));
            Assert.That(results[2], Is.EqualTo("SiteAccess"));
        }

        [Test]
        public void GetExtension_GivenEmailPasswordInV5_Return()
        {
            var module = new AuthenticationModule();

            var result = module.GetExtension(Protocol.V5, "EmailPassword");

            Assert.That(result, Is.InstanceOf<EmailPassword>());
        }

        private static Core.Data.Model.Module Make_ModuleConfig()
        {
            return new Core.Data.Model.Module
                {
                    Configuration = "<Settings><ConnectionString>connectionstring</ConnectionString></Settings>"
                };
        }
    }
}