using System;
using Chaos.Portal.Authentication.Data.Model;
using Chaos.Portal.Authentication.Extension.v6;
using Chaos.Portal.Core.Data.Model;
using Moq;
using NUnit.Framework;

namespace Chaos.Portal.Authentication.Tests.Extension
{
	[TestFixture]
	public class OAuthTest : TestBase
	{
		[Test]
		public void Login_GivenNewOAuthId_ReturnUserInfoAndUpdateProfileAndAuthenticateSession()
		{
			var extension = MakeOAuthExtension();

			var oAuthId = "somerandomletters";
			var email = "test@test.test";

			var expected = new UserInfo
			{
				Guid = new Guid("10000000-0000-0000-0000-000000000001"),
				Email = "test@test.test"
			};
			var callingUser = new UserInfo
			{
				Guid = new Guid("10000000-0000-0000-0000-000000000002"),
				Email = "test2@test.test",
				SystemPermissonsEnum = SystemPermissons.All
			};
			var sessionToAuthenticate = new Session
			{
				Guid = new Guid("12000000-0000-0000-0000-000000000021")
			};
			var managingUsersSession = new Session
			{
				Guid = new Guid("12000000-0000-0000-0000-000000000031"),
				UserGuid = callingUser.Guid
			};

			PortalRepository.Setup(m => m.SessionUpdate(sessionToAuthenticate.Guid, It.IsAny<Guid>())).Returns(new Session());
			PortalRepository.Setup(m => m.UserInfoGet(null, sessionToAuthenticate.Guid, null, null)).Returns(new[] { expected });
			PortalRepository.Setup(m => m.UserCreate(It.IsAny<Guid>(), email)).Returns(1);
			PortalRequest.SetupGet(p => p.Session).Returns(managingUsersSession);
			PortalRequest.SetupGet(p => p.User).Returns(callingUser);
			OAuthRepository.Setup(m => m.OAuthUserGet(oAuthId)).Returns((OAuthUser) null);

			var result = extension.Login(oAuthId, email, sessionToAuthenticate.Guid);

			OAuthRepository.Verify(m => m.OAuthUserUpdate(It.IsAny<Guid>(), oAuthId));

			PortalRepository.VerifyAll();

			Assert.That(result, Is.EqualTo(expected));
		}

		[Test]
		public void Login_GivenNewOAuthIdEmailExists_ReturnUserInfoAndUpdateProfileAndAuthenticateSession()
		{
			var extension = MakeOAuthExtension();

			var oAuthId = "somerandomletters";
			var email = "test@test.test";

			var expected = new UserInfo
			{
				Guid = new Guid("10000000-0000-0000-0000-000000000001"),
				Email = "test@test.test"
			};
			var callingUser = new UserInfo
			{
				Guid = new Guid("10000000-0000-0000-0000-000000000002"),
				Email = "test2@test.test",
				SystemPermissonsEnum = SystemPermissons.All
			};
			var sessionToAuthenticate = new Session
			{
				Guid = new Guid("12000000-0000-0000-0000-000000000021")
			};
			var managingUsersSession = new Session
			{
				Guid = new Guid("12000000-0000-0000-0000-000000000031"),
				UserGuid = callingUser.Guid
			};

			PortalRepository.Setup(m => m.SessionUpdate(sessionToAuthenticate.Guid, expected.Guid)).Returns(new Session());
			PortalRepository.Setup(m => m.UserInfoGet(null, sessionToAuthenticate.Guid, null, null)).Returns(new[] { expected });
			PortalRepository.Setup(m => m.UserInfoGet(null, null, email, null)).Returns(new[] { expected });
			PortalRequest.SetupGet(p => p.Session).Returns(managingUsersSession);
			PortalRequest.SetupGet(p => p.User).Returns(callingUser);
			OAuthRepository.Setup(m => m.OAuthUserGet(oAuthId)).Returns((OAuthUser)null);

			var result = extension.Login(oAuthId, email, sessionToAuthenticate.Guid);

			OAuthRepository.Verify(m => m.OAuthUserUpdate(expected.Guid, oAuthId));

			PortalRepository.VerifyAll();

			Assert.That(result, Is.EqualTo(expected));
		}

		[Test]
		public void Login_GivenExistingOAuthId_ReturnUserInfoAndUpdateProfileAndAuthenticateSession()
		{
			var extension = MakeOAuthExtension();

			var oAuthId = "somerandomletters";
			var email = "test@test.test";

			var expected = new UserInfo
			{
				Guid = new Guid("10000000-0000-0000-0000-000000000001"),
				Email = "test@test.test"
			};
			var callingUser = new UserInfo
			{
				Guid = new Guid("10000000-0000-0000-0000-000000000002"),
				Email = "test2@test.test",
				SystemPermissonsEnum = SystemPermissons.All
			};
			var oAuthUser = new OAuthUser()
			{
				UserGuid = expected.Guid,
				OAuthId = oAuthId
			};
			var sessionToAuthenticate = new Session
			{
				Guid = new Guid("12000000-0000-0000-0000-000000000021")
			};
			var managingUsersSession = new Session
			{
				Guid = new Guid("12000000-0000-0000-0000-000000000031"),
				UserGuid = callingUser.Guid
			};

			PortalRepository.Setup(m => m.SessionUpdate(sessionToAuthenticate.Guid, expected.Guid)).Returns(new Session());
			PortalRepository.Setup(m => m.UserInfoGet(null, sessionToAuthenticate.Guid, null, null)).Returns(new[] { expected });
			PortalRequest.SetupGet(p => p.Session).Returns(managingUsersSession);
			PortalRequest.SetupGet(p => p.User).Returns(callingUser);
			OAuthRepository.Setup(m => m.OAuthUserGet(oAuthId)).Returns(oAuthUser);

			var result = extension.Login(oAuthId, email, sessionToAuthenticate.Guid);

			PortalRepository.VerifyAll();

			Assert.That(result, Is.EqualTo(expected));
		}

		private OAuth MakeOAuthExtension()
		{
			return (OAuth)new OAuth(AuthenticationModule.Object).WithPortalRequest(PortalRequest.Object);
		}
	}
}