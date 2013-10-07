using Chaos.Portal.Authentication.Data.Dto;

namespace Chaos.Portal.Authentication.Tests
{
    using Chaos.Portal.Authentication.Data;
    using Chaos.Portal.Authentication.Extension;
    using Chaos.Portal.Core;
    using Chaos.Portal.Core.Data;
    using Chaos.Portal.Core.Request;

    using Moq;

    using NUnit.Framework;

    public class TestBase
    {
        protected Mock<IAuthenticationRepository> AuthenticationRepository { get; set; }
        protected Mock<IPortalApplication>        PortalApplication { get; set; }
        protected Mock<IPortalRepository>         PortalRepository { get; set; }
        protected Mock<IPortalRequest>            PortalRequest { get; set; }

        [SetUp]
        public void SetUp()
        {
            AuthenticationRepository = new Mock<IAuthenticationRepository>();
            PortalApplication        = new Mock<IPortalApplication>();
            PortalRepository         = new Mock<IPortalRepository>();
            PortalRequest            = new Mock<IPortalRequest>();

            PortalApplication.SetupGet(p => p.PortalRepository).Returns(PortalRepository.Object);
        }

        protected EmailPassword Make_EmailPassword()
        {
            return (EmailPassword)new EmailPassword(PortalApplication.Object, AuthenticationRepository.Object).WithPortalRequest(PortalRequest.Object);
        }

        protected SecureCookie Make_SecureCookie()
        {
            return (SecureCookie)new SecureCookie(PortalApplication.Object, AuthenticationRepository.Object).WithPortalRequest(PortalRequest.Object);
        }

		protected Wayf Make_Wayf()
		{
			return (Wayf)new Wayf(PortalApplication.Object, AuthenticationRepository.Object).WithPortalRequest(PortalRequest.Object);
		}
    }
}