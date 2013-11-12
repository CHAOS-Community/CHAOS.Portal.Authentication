using System;

namespace Chaos.Portal.Authentication.Tests.Extension
{
    using Chaos.Portal.Authentication.Data.Model;
    using Chaos.Portal.Authentication.Extension;
    using Chaos.Portal.Core.Data.Model;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class SiteAccessTest : TestBase
    {
        [Test]
        public void Auth_ValidApiKey_ReturnAuthenticatedSession()
        {
            var extension   = Make_SiteAccessExtension();
            var apiKey      = "some key";
            var user        = Make_UserInfo();
            var sessionGuid = new Guid("00000000-0000-0000-0000-000000000001");
            AuthenticationRepository.Setup(m => m.SiteKeyGet(It.IsAny<string>())).Returns(new SiteKey { UserGuid = user.Guid });
            PortalRepository.Setup(m => m.SessionCreate(user.Guid)).Returns(new Session { Guid = sessionGuid });

            var result = extension.Auth(apiKey);

            Assert.That(result.Guid, Is.EqualTo(sessionGuid));
        }

        [Test, ExpectedException(typeof(Core.Exceptions.InsufficientPermissionsException))]
        public void Auth_InvalidApiKey_ThrowException()
        {
            var extension = Make_SiteAccessExtension();
            var apiKey = "some key";
            AuthenticationRepository.Setup(m => m.SiteKeyGet(It.IsAny<string>())).Returns((SiteKey)null);

            extension.Auth(apiKey);
        }

        [Test]
        public void Create_GivenSiteDoesntExist_ReturnSiteKey()
        {
            var extension = Make_SiteAccessExtension();
            var name      = "some site name";
            var user      = Make_UserInfo();
            PortalRequest.SetupGet(p => p.User).Returns(user);
            AuthenticationRepository.Setup(m => m.SiteKeyCreate(It.IsAny<string>(), user.Guid, name)).Returns(1);

            var result = extension.Create(name);

            Assert.That(result.Key, Is.Not.Null);
            Assert.That(result.UserGuid, Is.EqualTo(user.Guid));
        }

        [Test, ExpectedException(typeof(Core.Exceptions.InsufficientPermissionsException))]
        public void Create_GivenAnonymousUser_ThrowException()
        {
            var extension = Make_SiteAccessExtension();
            var name      = "some site name";
            PortalRequest.SetupGet(p => p.IsAnonymousUser).Returns(true);

            extension.Create(name);
        }
        
        [Test, ExpectedException(typeof(Core.Exceptions.UnhandledException))]
        public void Create_UnknownErrorOnDatabase_ThrowException()
        {
            var extension = Make_SiteAccessExtension();
            var name      = "some site name";
            var user = Make_UserInfo();
            PortalRequest.SetupGet(p => p.User).Returns(user);
            AuthenticationRepository.Setup(m => m.SiteKeyCreate(It.IsAny<string>(), user.Guid, name)).Returns(0);

            extension.Create(name);
        }

        private static UserInfo Make_UserInfo()
        {
            return new UserInfo{Guid = new Guid("00000000-0000-0000-0000-000000000001")};
        }

        private SiteAccess Make_SiteAccessExtension()
        {
            return (SiteAccess)new SiteAccess(PortalApplication.Object, AuthenticationRepository.Object).WithPortalRequest(PortalRequest.Object);
        }
    }
}
