namespace Chaos.Portal.Authentication.Tests.Extension
{
    using System;
    using System.Linq;

    using Chaos.Portal.Authentication.Data;
    using Chaos.Portal.Authentication.Extension;
    using Chaos.Portal.Data.Dto;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class SecureCookieTests
    {
        protected Mock<ICallContext> CallContext { get; set; }
        protected Mock<IAuthenticationRepository> AuthenticationRepository { get; set; }

        [SetUp]
        public void SetUp()
        {
            CallContext              = new Mock<ICallContext>();
            AuthenticationRepository = new Mock<IAuthenticationRepository>();
        }

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

        private SecureCookie Make_SecureCookie()
        {
            return new SecureCookie(AuthenticationRepository.Object);
        }
    }
}