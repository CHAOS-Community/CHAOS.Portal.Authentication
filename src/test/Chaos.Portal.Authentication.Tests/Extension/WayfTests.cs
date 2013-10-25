namespace Chaos.Portal.Authentication.Tests.Extension
{
	using NUnit.Framework;
	using System;
	using Data.Dto;
	using Core.Data.Model;
	using Moq;

	[TestFixture]
	public class WayfTests : TestBase
	{
		[Test]
		public void Login_GivenExistingWayfId_ReturnUserInfoAndUpdateProfileAndAuthenticateSession()
		{
			var extension = Make_Wayf();

			var wayfId = "somerandomletters";
			var givenName = "Albert";
			var surName = "Einstein";
			var commonName = "Einer";

			var expected = new UserInfo
			{
				Guid = new Guid("10000000-0000-0000-0000-000000000001"),
				Email = "test@test.test"
			};
			var profile = new WayfProfile()
			{
				UserGuid = expected.Guid,
				WayfId = wayfId
			};
			var session = new Session
			{
				Guid = new Guid("12000000-0000-0000-0000-000000000021")
			};

			PortalRepository.Setup(m => m.SessionUpdate(session.Guid, expected.Guid)).Returns(new Session());
			PortalRepository.Setup(m => m.UserInfoGet(null, session.Guid, null, null)).Returns(new[] { expected });
			PortalRequest.SetupGet(p => p.Session).Returns(session);
			AuthenticationRepository.Setup(m => m.WayfProfileGet(wayfId)).Returns(profile);

			var result = extension.Login(wayfId, givenName, surName, commonName);

			PortalRepository.VerifyAll();

			Assert.That(result, Is.EqualTo(expected));
		}
	}
}