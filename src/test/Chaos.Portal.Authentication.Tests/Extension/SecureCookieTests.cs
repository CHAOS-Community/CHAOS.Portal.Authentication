namespace Chaos.Portal.Authentication.Tests.Extension
{
    using System;
    using System.Linq;

    using Chaos.Portal.Authentication.Exception;
    using Chaos.Portal.Data.Dto;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class SecureCookieTests : TestBase
    {
        [Test]
        public void Get_GivenExistingSession_ReturnSecureCookie()
        {
            var extension = Make_SecureCookie();
            var expected  = new Data.Dto.SecureCookie();
            var userGuid  = new Guid("10000000-0000-0000-0000-000000000001");
            CallContext.SetupGet(p => p.User).Returns(new UserInfo{Guid = userGuid});
            AuthenticationRepository.Setup(m => m.SecureCookieGet(userGuid, null, null)).Returns(new[] { expected });

            var result = extension.Get(CallContext.Object).First();

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void Delete_GivenExistingSession_ReturnOne()
        {
            var extension  = Make_SecureCookie();
            var expected   = 1u;
            var userGuid   = new Guid("10000000-0000-0000-0000-000000000001");
            var cookieGuid = new Guid("12000000-0000-0000-0000-000000000021");
            CallContext.SetupGet(p => p.User).Returns(new UserInfo{Guid = userGuid});
            AuthenticationRepository.Setup(m => m.SecureCookieDelete(userGuid, cookieGuid)).Returns(1u);

            var result = extension.Delete(CallContext.Object, cookieGuid);

            Assert.That(result.Value, Is.EqualTo(expected));
        }

        [Test]
        public void Create_IsAuthenticated_CreateAndReturnSecureCookie()
        {
            var extension   = Make_SecureCookie();
            var expected    = new Data.Dto.SecureCookie();
            var userGuid    = new Guid("10000000-0000-0000-0000-000000000001");
            var sessionGuid = new Guid("12000000-0000-0000-0000-000000000021");
            CallContext.SetupGet(p => p.User).Returns(new UserInfo { Guid = userGuid });
            CallContext.SetupGet(p => p.Session).Returns(new Session{ Guid = sessionGuid });
            AuthenticationRepository.Setup(m => m.SecureCookieGet(userGuid, It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(new[] { expected });

            var result = extension.Create(CallContext.Object);

            Assert.That(result, Is.EqualTo(expected));
            AuthenticationRepository.Verify(m => m.SecureCookieCreate(userGuid, It.IsAny<Guid>(), It.IsAny<Guid>(), sessionGuid));
        }

        [Test, ExpectedException(typeof(LoginException))]
        public void Create_IsNotAuthenticated_ThrowLoginException()
        {
            var extension = Make_SecureCookie();
            CallContext.SetupGet(p => p.IsAnonymousUser).Returns(true);

            extension.Create(CallContext.Object);
        }
    }
}