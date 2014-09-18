using Chaos.Portal.Authentication.OAuth;
using Chaos.Portal.Authentication.Wayf;
using Moq.Protected;

namespace Chaos.Portal.Authentication.Tests
{
  using System;
  using Configuration;
  using Core.Data.Model;
  using Data;
  using Authentication.Extension;
  using Core;
  using Core.Data;
  using Core.Request;
  using Moq;
  using NUnit.Framework;

  public class TestBase
  {
    protected Mock<IAuthenticationRepository> AuthenticationRepository { get; set; }
    protected Mock<IOAuthRepository> OAuthRepository { get; set; }
    protected Mock<IPortalApplication> PortalApplication { get; set; }
    protected Mock<IPortalRepository> PortalRepository { get; set; }
    protected Mock<IPortalRequest> PortalRequest { get; set; }
    protected Mock<IFacebookClient> FacebookClient { get; set; }
    protected Mock<IOAuthClient> OAuthClient { get; set; }
    protected Mock<IAuthenticationModule> AuthenticationModule { get; set; }
    protected Mock<IWayfFilter> WayfFilter { get; set; }


    [SetUp]
    public void SetUp()
    {
      AuthenticationRepository = new Mock<IAuthenticationRepository>();
      OAuthRepository = new Mock<IOAuthRepository>();
      PortalApplication = new Mock<IPortalApplication>();
      PortalRepository = new Mock<IPortalRepository>();
      PortalRequest = new Mock<IPortalRequest>();
      FacebookClient = new Mock<IFacebookClient>();
      OAuthClient = new Mock<IOAuthClient>();
      AuthenticationModule = new Mock<IAuthenticationModule>();
      WayfFilter = new Mock<IWayfFilter>();

      AuthenticationModule.SetupGet(p => p.PortalApplication).Returns(PortalApplication.Object);
      AuthenticationModule.SetupGet(p => p.FacebookClient).Returns(FacebookClient.Object);
      AuthenticationModule.SetupGet(p => p.OAuthClient).Returns(OAuthClient.Object);
      AuthenticationModule.SetupGet(p => p.AuthenticationRepository).Returns(AuthenticationRepository.Object);
      AuthenticationRepository.SetupGet(r => r.OAuth).Returns(OAuthRepository.Object);
      PortalApplication.SetupGet(p => p.PortalRepository).Returns(PortalRepository.Object);
    }

    protected EmailPassword Make_EmailPassword()
    {
      return
        (EmailPassword)
        new EmailPassword(PortalApplication.Object, AuthenticationRepository.Object,
                          Make_AuthenticationSettings().Password).WithPortalRequest(PortalRequest.Object);
    }

    protected SecureCookie Make_SecureCookie()
    {
      return
        (SecureCookie)
        new SecureCookie(PortalApplication.Object, AuthenticationRepository.Object).WithPortalRequest(
          PortalRequest.Object);
    }

    protected Authentication.Extension.Wayf Make_Wayf()
    {
      return
        (Authentication.Extension.Wayf)
        new Authentication.Extension.Wayf(PortalApplication.Object, AuthenticationRepository.Object, WayfFilter.Object)
          .WithPortalRequest(PortalRequest.Object);
    }

    protected static UserInfo Make_UserInfo()
    {
      return new UserInfo
        {
          Guid = new Guid("11000000-0000-0000-0000-000000000011")
        };
    }

    protected static AuthenticationSettings Make_AuthenticationSettings()
    {
      return new AuthenticationSettings
        {
          ConnectionString = "connectionstring",
          Facebook = new FacebookSettings
            {
              AppId = "some app id",
              AppSecret = "some app secret"
            },
          OAuth = new OAuthSettings
            {
              ClientId = "Some id",
              AuthorizationEndpoint = "http://awesome/Authorize",
              ClientSecret = "Some secret",
              TokenEndpoint = "http://awesome/Token",
              UserInfoEndpoint = "http://awesome/UserInfo"
            }
        };
    }
  }
}