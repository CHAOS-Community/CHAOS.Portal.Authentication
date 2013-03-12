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
        protected Mock<IPortalApplication>        PortalApplication { get; set; }
        protected Mock<IPortalRepository>         PortalRepository { get; set; }
        protected Mock<ICallContext>              CallContext { get; set; }

        [SetUp]
        public void SetUp()
        {
            AuthenticationRepository = new Mock<IAuthenticationRepository>();
            PortalApplication        = new Mock<IPortalApplication>();
            PortalRepository         = new Mock<IPortalRepository>();
            CallContext              = new Mock<ICallContext>();

            PortalApplication.SetupGet(p => p.PortalRepository).Returns(PortalRepository.Object);
        }

        protected EmailPassword Make_EmailPassword()
        {
            return (EmailPassword)new EmailPassword(AuthenticationRepository.Object).WithPortalApplication(PortalApplication.Object);
        }
    }
}