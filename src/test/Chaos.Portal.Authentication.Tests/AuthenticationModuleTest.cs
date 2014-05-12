using System;
using Chaos.Portal.Authentication.Data;
using Moq;

namespace Chaos.Portal.Authentication.Tests
{
    using System.Linq;

    using Authentication.Extension;
    using Authentication.Extension.v6;
    using Core;
    using Core.Data.Model;
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
            Assert.That(results[2], Is.EqualTo("AuthKey"));
            Assert.That(results[3], Is.EqualTo("OAuth"));
            Assert.That(results[4], Is.EqualTo("Wayf"));
            Assert.That(results[5], Is.EqualTo("Facebook"));
        }

        [Test]
        public void GetExtension_GivenEmailPasswordInV5_Return()
        {
            var module = new AuthenticationModule();

            var result = module.GetExtension(Protocol.V5, "EmailPassword");

            Assert.That(result, Is.InstanceOf<EmailPassword>());
        }

        [Test]
        public void GetExtension_Facebookv6_ReturnFacebookExtension()
        {
            var module = new AuthenticationModule();

            var result = module.GetExtension(Protocol.V6, "Facebook");

            Assert.That(result, Is.InstanceOf<Facebook>());
        }

        [Test]
        public void Load_ValidConfiguration()
        {
            var module = new AuthenticationModule();
            var config = Make_ModuleConfig();
            PortalRepository.Setup(m => m.ModuleGet("Authentication")).Returns(config);

            module.Load(PortalApplication.Object);
        }

		[Test]
		public void OnUserInfoUpdate_GivenInfo_ShouldInvokeListener()
		{
			var module = new AuthenticationModule();
			var config = Make_ModuleConfig();
			var userGuid = new Guid("10000000-0000-0000-0000-000000000001");
			var userInfo = 5;
			UserInfoUpdate<int> result = null;
			Action<UserInfoUpdate<int>> callback = i => result = i;

			PortalRepository.Setup(m => m.ModuleGet("Authentication")).Returns(config);
			module.Load(PortalApplication.Object);

			module.AddUserInfoUpdateListener(callback);
			module.OnUserInfoUpdate(userGuid, userInfo);

			Assert.That(result, Is.Not.Null);
			Assert.That(result.UserGuid, Is.EqualTo(userGuid));
			Assert.That(result.UserInfo, Is.EqualTo(userInfo));
		}

		[Test]
		public void OnUserInfoUpdate_GivenInfo_ShouldNotInvokeListenerWithDifferentType()
		{
			var module = new AuthenticationModule();
			var config = Make_ModuleConfig();
			var userGuid = new Guid("10000000-0000-0000-0000-000000000001");
			var userInfo = 5;
			UserInfoUpdate<uint> result = null;
			Action<UserInfoUpdate<uint>> callback = i => result = i;

			PortalRepository.Setup(m => m.ModuleGet("Authentication")).Returns(config);
			module.Load(PortalApplication.Object);

			module.AddUserInfoUpdateListener(callback);
			module.OnUserInfoUpdate(userGuid, userInfo);

			Assert.That(result, Is.Null);
		}

        private static Module Make_ModuleConfig()
        {
            return new Module
                {
					Configuration = "<Settings><ConnectionString>connectionstring</ConnectionString><Facebook AppId=\"some app id\" AppSecret=\"some app secret\"></Facebook><OAuth ClientId=\"Some id\" ClientSecret=\"Some secret\" AuthorizationEndpoint=\"http://awesome/Authorize\" TokenEndpoint=\"http://awesome/Token\" UserInfoEndpoint=\"http://awesome/UserInfo\" /></Settings>"
                };
        }
    }
}