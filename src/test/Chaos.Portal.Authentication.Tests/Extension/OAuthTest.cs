using System;
using Chaos.Portal.Authentication.Data.Model;
using Chaos.Portal.Authentication.OAuth;
using Chaos.Portal.Core.Data.Model;
using DotNetAuth.Profiles;
using Moq;
using NUnit.Framework;

namespace Chaos.Portal.Authentication.Tests.Extension
{
	[TestFixture]
	public class OAuthTest : TestBase
	{
		[Test]
		public void GetLoginEndPoint_GivenCallbackUrl_ReturnEndPoint()
		{
			var extension = MakeOAuthExtension();
			var callbackUrl = "http://MyCallback.test";
			var endpoint = new LoginEndPoint();

			OAuthClient.Setup(c => c.GetLoginEndPoint(callbackUrl)).Returns(endpoint);

			var result = extension.GetLoginEndPoint(callbackUrl);

			Assert.AreEqual(endpoint, result);
		}

		[Test]
		public void ProcessLogin_GivenNewValidOAuthLogin_ReturnAndAuthorizeSession()
		{
			var extension = MakeOAuthExtension();
			var callbackUrl = "http://MyCallback.test";
			var responseUrl = "http://MyResponse.test";
			var stateCode = "MyCode";
			var session = Make_Session();
			var oAuthUser = Make_OAuthUser();
			var oAuthProfile = Make_OAuthProfile();
			oAuthProfile.UniqueID = oAuthUser.OAuthId;

			OAuthClient.Setup(c => c.ProcessLogin(callbackUrl, responseUrl, stateCode)).Returns(oAuthProfile);
			OAuthRepository.Setup(r => r.OAuthUserGet(oAuthUser.OAuthId)).Returns((OAuthUser) null);
			OAuthRepository.Setup(r => r.OAuthUserUpdate(It.IsAny<Guid>(), oAuthUser.OAuthId)).Verifiable();
			PortalRepository.Setup(r => r.UserCreate(It.IsAny<Guid>(), oAuthProfile.Email)).Returns(1).Verifiable();
			PortalRepository.Setup(r => r.SessionCreate(It.IsAny<Guid>())).Returns(session).Verifiable();

			var result = extension.ProcessLogin(callbackUrl, responseUrl, stateCode);

			OAuthRepository.Verify();
			PortalRepository.Verify();
			
			Assert.That(result, Is.Not.Null);
			Assert.That(result.Guid, Is.EqualTo(session.Guid));
		}

		private Profile Make_OAuthProfile()
		{
			return new Profile
			{
				Email = "test@test.test"
			};
		}

		private OAuthUser Make_OAuthUser()
		{
			return new OAuthUser
			{
				OAuthId = "MyID",
				UserGuid = new Guid("12000000-0000-0000-0000-000000000021"),
				DateCreated = DateTime.Now,
				DateModified = DateTime.Now
			};
		}

		private Authentication.Extension.v6.OAuth MakeOAuthExtension()
		{
			return (Authentication.Extension.v6.OAuth)new Authentication.Extension.v6.OAuth(AuthenticationModule.Object).WithPortalRequest(PortalRequest.Object);
		}

		private static Session Make_Session()
		{
			return new Session { Guid = new Guid("10000000-0000-0000-0000-000000000001") };
		}
	}
}