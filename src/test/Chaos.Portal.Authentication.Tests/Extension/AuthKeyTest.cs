using System;
using System.Collections.Generic;
using System.Linq;
using Chaos.Portal.Authentication.Data.Model;

namespace Chaos.Portal.Authentication.Tests.Extension
{
	using Chaos.Portal.Core.Data.Model;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class AuthKeyTest : TestBase
    {
        [Test]
        public void Login_ValidApiKey_ReturnAuthenticatedSession()
        {
            var extension   = Make_SiteAccessExtension();
            var token       = "some token";
            var user        = Make_UserInfo();
            var sessionGuid = new Guid("00000000-0000-0000-0000-000000000001");
            AuthenticationRepository.Setup(m => m.AuthKeyGet(null, It.IsAny<string>())).Returns(new List<AuthKey>{new AuthKey { UserGuid = user.Guid }});
            PortalRepository.Setup(m => m.SessionCreate(user.Guid)).Returns(new Session { Guid = sessionGuid });

            var result = extension.Login(token);

            Assert.That(result.Guid, Is.EqualTo(sessionGuid));
        }

        [Test, ExpectedException(typeof(Core.Exceptions.InsufficientPermissionsException))]
		public void Login_InvalidApiKey_ThrowException()
        {
            var extension = Make_SiteAccessExtension();
            var token = "some token";
            AuthenticationRepository.Setup(m => m.AuthKeyGet(null, It.IsAny<string>())).Returns(new List<AuthKey>());

            extension.Login(token);
        }

        [Test]
        public void Create_GivenSiteDoesntExist_ReturnSiteKey()
        {
            var extension = Make_SiteAccessExtension();
            var name      = "some app name";
            var user      = Make_UserInfo();
            PortalRequest.SetupGet(p => p.User).Returns(user);
            AuthenticationRepository.Setup(m => m.AuthKeyCreate(It.IsAny<string>(), user.Guid, name)).Returns(1);

            var result = extension.Create(name);

            Assert.That(result.Token, Is.Not.Null);
            Assert.That(result.UserGuid, Is.EqualTo(user.Guid));
        }

        [Test, ExpectedException(typeof(Core.Exceptions.InsufficientPermissionsException))]
        public void Create_GivenAnonymousUser_ThrowException()
        {
            var extension = Make_SiteAccessExtension();
            var name      = "some app name";
            PortalRequest.SetupGet(p => p.IsAnonymousUser).Returns(true);

            extension.Create(name);
        }
        
        [Test, ExpectedException(typeof(Core.Exceptions.UnhandledException))]
        public void Create_UnknownErrorOnDatabase_ThrowException()
        {
            var extension = Make_SiteAccessExtension();
            var name      = "some app name";
            var user = Make_UserInfo();
            PortalRequest.SetupGet(p => p.User).Returns(user);
            AuthenticationRepository.Setup(m => m.AuthKeyCreate(It.IsAny<string>(), user.Guid, name)).Returns(0);

            extension.Create(name);
        }

	    [Test]
	    public void Get_UserHasNoAuthKeys_ReturnEmptyList()
	    {
			var extension = Make_SiteAccessExtension();
			var user = Make_UserInfo();
			PortalRequest.SetupGet(p => p.User).Returns(user);

			AuthenticationRepository.Setup(m => m.AuthKeyGet(user.Guid, null)).Returns(new List<AuthKey>());

		    var result = extension.Get();

			Assert.That(result.Count, Is.EqualTo(0));
	    }

		[Test]
		public void Get_UserhasAuthKeys_ReturnListOfAuthKeysWithOutTokens()
		{
			var extension = Make_SiteAccessExtension();
			var user = Make_UserInfo();
			var expected = new List<AuthKey> {new AuthKey("Token 1", "My App 1", user.Guid), new AuthKey("Token 2", "My App 2", user.Guid)};

			PortalRequest.SetupGet(p => p.User).Returns(user);
			AuthenticationRepository.Setup(m => m.AuthKeyGet(user.Guid, null)).Returns(expected);

			var result = extension.Get();

			Assert.That(result.Count, Is.EqualTo(2));
			Assert.That(result.Count(a => a.Token != null), Is.EqualTo(0));
		}

	    [Test]
	    public void Delete_GivenExistingAuthKey_DeleteKeyReturnOne()
	    {
			var extension = Make_SiteAccessExtension();
			var user = Make_UserInfo();
		    var key = new AuthKey("some token", "My App", user.Guid);

			PortalRequest.SetupGet(p => p.User).Returns(user);
			AuthenticationRepository.Setup(m => m.AuthKeyDelete(user.Guid, key.Name)).Returns(1).Verifiable();

		    var result = extension.Delete(key.Name);

			AuthenticationRepository.Verify();

			Assert.That(result.Value, Is.EqualTo(1));
	    }

		[Test, ExpectedException(typeof(ArgumentException))]
		public void Delete_GivenNonExistingAuthKey_ThrowArgumentException()
		{
			var extension = Make_SiteAccessExtension();
			var user = Make_UserInfo();
			var noneExistingKey = new AuthKey("some token", "My App", user.Guid);

			PortalRequest.SetupGet(p => p.User).Returns(user);
			AuthenticationRepository.Setup(m => m.AuthKeyDelete(user.Guid, noneExistingKey.Name)).Returns(0).Verifiable();

			extension.Delete(noneExistingKey.Name);
		}

	    private static UserInfo Make_UserInfo()
        {
            return new UserInfo{Guid = new Guid("00000000-0000-0000-0000-000000000001")};
        }

        private Authentication.Extension.AuthKey Make_SiteAccessExtension()
        {
            return (Authentication.Extension.AuthKey)new Authentication.Extension.AuthKey(PortalApplication.Object, AuthenticationRepository.Object).WithPortalRequest(PortalRequest.Object);
        }
    }
}
