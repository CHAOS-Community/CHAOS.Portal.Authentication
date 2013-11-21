using Chaos.Portal.Core.Exceptions;

namespace Chaos.Portal.Authentication.Tests.Extension
{
    using System;

    using Chaos.Portal.Authentication.Data.Dto;
    using Chaos.Portal.Authentication.Exception;
    using Chaos.Portal.Core.Data.Model;
    using Data.Model;
    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class EmailPasswordTests : TestBase
    {
        [Test]
        public void Login_GivenValidEmailAndPassword_ReturnUserInfoAndAuthenticateSession()
        {
            var extension = Make_EmailPassword();
            var expected  = new UserInfo
                {
                    Guid  = new Guid("10000000-0000-0000-0000-000000000001"),
                    Email = "test@test.test"
                };
            var password  = "passw0rd";
            var session = new Session
                {
                    Guid = new Guid("12000000-0000-0000-0000-000000000021")
                };
            PortalRepository.Setup(m => m.UserInfoGet(expected.Email)).Returns(expected);
            PortalRepository.Setup(m => m.SessionUpdate(session.Guid, expected.Guid)).Returns(new Session());
            PortalRepository.Setup(m => m.UserInfoGet(null, session.Guid, null, null)).Returns(new[] {expected});
            PortalRequest.SetupGet(p => p.Session).Returns(session);
            AuthenticationRepository.Setup(m => m.EmailPasswordGet(expected.Guid, It.IsAny<string>())).Returns(new EmailPassword());

            var result = extension.Login(expected.Email, password);

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test, ExpectedException(typeof(LoginException))]
        public void Login_GivenInvalidEmail_ThrowsLoginException()
        {
            var extension = Make_EmailPassword();
            PortalRepository.Setup(m => m.UserInfoGet(It.IsAny<string>())).Throws(new ArgumentException());

            extension.Login("notvalid", "password");
        }

        [Test, ExpectedException(typeof(LoginException))]
        public void Login_GivenValidEmailAndWrongPassword_ThrowsLoginException()
        {
            var extension = Make_EmailPassword();
            var user = new UserInfo
            {
                Guid = new Guid("10000000-0000-0000-0000-000000000001"),
                Email = "test@test.test"
            };
            var password = "passw0rd";
            PortalRepository.Setup(m => m.UserInfoGet(user.Email)).Returns(user);
            AuthenticationRepository.Setup(m => m.EmailPasswordGet(user.Guid, "validpassword")).Returns(new EmailPassword());

            extension.Login(user.Email, password);
        }

	    [Test]
	    public void SetPassword_UserHasPermission_UpdatePassword()
	    {
			var extension = Make_EmailPassword();
			var user = new UserInfo
            {
                Guid = new Guid("10000000-0000-0000-0000-000000000001"),
                Email = "test@test.test",
				SystemPermissonsEnum = SystemPermissons.All
            };
			var newPassword = "passw0rd";

		    PortalRequest.Setup(r => r.User).Returns(user);
		    AuthenticationRepository.Setup(a => a.EmailPasswordUpdate(user.Guid, It.IsAny<string>())).Returns(1);

			var result = extension.SetPassword(user.Guid, newPassword);

			AuthenticationRepository.Verify(a => a.EmailPasswordUpdate(user.Guid, It.IsAny<string>()));

			Assert.That(result.Value, Is.EqualTo(1));
	    }

		[Test, ExpectedException(typeof(InsufficientPermissionsException))]
		public void SetPassword_UserDoesNotHavePermission_ThrowInsufficientPermissionsException()
		{
			var extension = Make_EmailPassword();
			var user = new UserInfo
			{
				Guid = new Guid("10000000-0000-0000-0000-000000000001"),
				Email = "test@test.test",
				SystemPermissonsEnum = SystemPermissons.None
			};
			var newPassword = "passw0rd";

			PortalRequest.Setup(r => r.User).Returns(user);

			var result = extension.SetPassword(user.Guid, newPassword);
		}
    }
}