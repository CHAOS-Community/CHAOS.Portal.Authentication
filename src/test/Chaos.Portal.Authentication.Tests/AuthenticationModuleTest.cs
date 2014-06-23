using System;
using Chaos.Portal.Authentication.Data;
using Moq;

namespace Chaos.Portal.Authentication.Tests
{
    using Configuration;
    using Core.Extension;
    using NUnit.Framework;

    [TestFixture]
    public class AuthenticationModuleTest : TestBase
    {
        [Test]
        public void Load_Extensions_AllExtensionsWereMapped()
        {
            var module = new AuthenticationModule();
            PortalApplication.Setup(m => m.GetSettings<AuthenticationSettings>("Authentication")).Returns(Make_AuthenticationSettings());

            module.Load(PortalApplication.Object);

            PortalApplication.Verify(m => m.MapRoute("/v5/EmailPassword", It.IsAny<Func<IExtension>>()));
            PortalApplication.Verify(m => m.MapRoute("/v5/SecureCookie", It.IsAny<Func<IExtension>>()));
            PortalApplication.Verify(m => m.MapRoute("/v6/EmailPassword", It.IsAny<Func<IExtension>>()));
            PortalApplication.Verify(m => m.MapRoute("/v6/AuthKey", It.IsAny<Func<IExtension>>()));
            PortalApplication.Verify(m => m.MapRoute("/v6/OAuth", It.IsAny<Func<IExtension>>()));
            PortalApplication.Verify(m => m.MapRoute("/v6/Wayf", It.IsAny<Func<IExtension>>()));
            PortalApplication.Verify(m => m.MapRoute("/v6/Facebook", It.IsAny<Func<IExtension>>()));
        }

        [Test]
        public void Load_ValidConfiguration()
        {
            var module = new AuthenticationModule();
            var config = Make_AuthenticationSettings();
            PortalApplication.Setup(m => m.GetSettings<AuthenticationSettings>("Authentication")).Returns(Make_AuthenticationSettings());

            module.Load(PortalApplication.Object);
        }

		[Test]
		public void OnUserInfoUpdate_GivenInfo_ShouldInvokeListener()
		{
			var module = new AuthenticationModule();
			var userGuid = new Guid("10000000-0000-0000-0000-000000000001");
			var userInfo = 5;
			UserInfoUpdate<int> result = null;
			Action<UserInfoUpdate<int>> callback = i => result = i;
            PortalApplication.Setup(m => m.GetSettings<AuthenticationSettings>("Authentication")).Returns(Make_AuthenticationSettings());

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
			var userGuid = new Guid("10000000-0000-0000-0000-000000000001");
			var userInfo = 5;
			UserInfoUpdate<uint> result = null;
			Action<UserInfoUpdate<uint>> callback = i => result = i;
            PortalApplication.Setup(m => m.GetSettings<AuthenticationSettings>("Authentication")).Returns(Make_AuthenticationSettings());

			module.Load(PortalApplication.Object);

			module.AddUserInfoUpdateListener(callback);
			module.OnUserInfoUpdate(userGuid, userInfo);

			Assert.That(result, Is.Null);
		}
    }
}