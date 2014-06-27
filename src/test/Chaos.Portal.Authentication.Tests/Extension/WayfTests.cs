using System.Collections.Generic;
using Chaos.Portal.Authentication.Exception;
using Moq;

namespace Chaos.Portal.Authentication.Tests.Extension
{
    using Data.Model;
    using NUnit.Framework;
	using System;
	using Data.Dto;
	using Core.Data.Model;

	[TestFixture]
	public class WayfTests : TestBase
	{
		[Test]
		public void Login_GivenExistingWayfId_ReturnUserInfoAndUpdateProfileAndAuthenticateSession()
		{
			var extension = Make_Wayf();

			var wayfId = "somerandomletters";
			var email = "test@test.test";
			var attributeData = string.Format("{{'eduPersonTargetedID': ['{0}'], 'mail': ['{1}']}}", wayfId, email);

			var expected = new UserInfo
			{
				Guid = new Guid("10000000-0000-0000-0000-000000000001"),
				Email = "test@test.test"
			};
			var callingUser  = new UserInfo
			{
				Guid = new Guid("10000000-0000-0000-0000-000000000002"),
				Email = "test2@test.test",
				SystemPermissonsEnum = SystemPermissons.All
			};
			var profile = new WayfUser()
			{
				UserGuid = expected.Guid,
				WayfId = wayfId
			};
			var sessionToAuthenticate = new Session
			{
				Guid = new Guid("12000000-0000-0000-0000-000000000021")
			};

			PortalRepository.Setup(m => m.SessionUpdate(sessionToAuthenticate.Guid, expected.Guid)).Returns(new Session()).Verifiable();
			PortalRepository.Setup(m => m.UserInfoGet(null, sessionToAuthenticate.Guid, null, null)).Returns(new[] { expected }).Verifiable();
			PortalRequest.SetupGet(p => p.User).Returns(callingUser).Verifiable();
			AuthenticationRepository.Setup(m => m.WayfProfileGet(wayfId)).Returns(profile).Verifiable();
			WayfFilter.Setup(f => f.Validate(It.IsAny<IDictionary<string, IList<string>>>())).Returns(true).Verifiable();

			var result = extension.Login(attributeData, sessionToAuthenticate.Guid);

			PortalRepository.Verify();
			PortalRequest.Verify();
			AuthenticationRepository.Verify();
			WayfFilter.Verify();

			Assert.That(result, Is.EqualTo(expected));
		}

		[Test, ExpectedException(typeof(WayfUserNotAllowedException))]
		public void Login_GivenNotAllowedWayfUser_ThrowWayfUserNotAllowedException()
		{
			var extension = Make_Wayf();

			var wayfId = "somerandomletters";
			var email = "test@test.test";
			var attributeData = string.Format("{{'eduPersonTargetedID': ['{0}'], 'mail': ['{1}']}}", wayfId, email);

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
			var profile = new WayfUser()
			{
				UserGuid = expected.Guid,
				WayfId = wayfId
			};
			var sessionToAuthenticate = new Session
			{
				Guid = new Guid("12000000-0000-0000-0000-000000000021")
			};

			PortalRepository.Setup(m => m.SessionUpdate(sessionToAuthenticate.Guid, expected.Guid)).Returns(new Session()).Verifiable();
			PortalRepository.Setup(m => m.UserInfoGet(null, sessionToAuthenticate.Guid, null, null)).Returns(new[] { expected }).Verifiable();
			PortalRequest.SetupGet(p => p.User).Returns(callingUser).Verifiable();
			AuthenticationRepository.Setup(m => m.WayfProfileGet(wayfId)).Returns(profile).Verifiable();
			WayfFilter.Setup(f => f.Validate(It.IsAny<IDictionary<string, IList<string>>>())).Returns(false).Verifiable();

			extension.Login(attributeData, sessionToAuthenticate.Guid);
		}

		[Test]
		public void Login_GivenNoEmail_CreateUser()
		{
			var extension = Make_Wayf();

			var wayfId = "somerandomletters";
			string email = null;
			var attributeData = string.Format("{{'eduPersonTargetedID': ['{0}'], 'mail': [{1}]}}", wayfId, email);

			var expected = new UserInfo
			{
				Guid = new Guid("10000000-0000-0000-0000-000000000001"),
				Email = null
			};
			var callingUser = new UserInfo
			{
				Guid = new Guid("10000000-0000-0000-0000-000000000002"),
				Email = "test2@test.test",
				SystemPermissonsEnum = SystemPermissons.All
			};
			var profile = new WayfUser()
			{
				UserGuid = expected.Guid,
				WayfId = wayfId
			};
			var sessionToAuthenticate = new Session
			{
				Guid = new Guid("12000000-0000-0000-0000-000000000021")
			};

			PortalRepository.Setup(m => m.SessionUpdate(sessionToAuthenticate.Guid, expected.Guid)).Returns(new Session()).Verifiable();
			PortalRepository.Setup(m => m.UserInfoGet(null, sessionToAuthenticate.Guid, null, null)).Returns(new[] { expected }).Verifiable();
			PortalRepository.Setup(m => m.UserInfoGet(null, null, null, null)).Throws(new Exception("UserGet should not be called when email is null"));
			PortalRequest.SetupGet(p => p.User).Returns(callingUser).Verifiable();
			AuthenticationRepository.Setup(m => m.WayfProfileGet(wayfId)).Returns(profile).Verifiable();
			WayfFilter.Setup(f => f.Validate(It.IsAny<IDictionary<string, IList<string>>>())).Returns(true).Verifiable();

			var result = extension.Login(attributeData, sessionToAuthenticate.Guid);

			PortalRepository.Verify();
			PortalRequest.Verify();
			AuthenticationRepository.Verify();

			Assert.That(result, Is.EqualTo(expected));
		}
	}
}