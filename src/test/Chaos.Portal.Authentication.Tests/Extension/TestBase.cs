namespace Chaos.Portal.Authentication.Tests.Extension
{
    using Chaos.Portal.Authentication.Data;
    using Chaos.Portal.Authentication.Extension;
    using Chaos.Portal.Data;

    using Moq;

    using NUnit.Framework;

    public class TestBase
    {
        protected SecureCookie Make_SecureCookie()
        {
            return new SecureCookie(AuthenticationRepository.Object, PortalRepository.Object);
        }

        protected Mock<IAuthenticationRepository> AuthenticationRepository { get; set; }
        protected Mock<IPortalRepository>         PortalRepository { get; set; }
        protected Mock<ICallContext>              CallContext { get; set; }

        [SetUp]
        public void SetUp()
        {
            CallContext              = new Mock<ICallContext>();
            AuthenticationRepository = new Mock<IAuthenticationRepository>();
            PortalRepository         = new Mock<IPortalRepository>();
        }
    }
}