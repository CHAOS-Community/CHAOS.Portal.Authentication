namespace Chaos.Portal.Authentication.Tests.Extension
{
    using System;

    using Chaos.Portal.Authentication.Data.Dto;
    using Chaos.Portal.Authentication.Exception;
    using Chaos.Portal.Core.Data.Model;

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
            PortalRepository.Setup(m => m.UserInfoGet(null, session.Guid, null)).Returns(new[] {expected});
            PortalRequest.SetupGet(p => p.Session).Returns(session);
            AuthenticationRepository.Setup(m => m.EmailPasswordGet(expected.Guid, It.IsAny<string>())).Returns(new EmailPassword());

            var result = extension.Login(expected.Email, password);

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test, ExpectedException(typeof(LoginException))]
        public void Login_GivenInvalidEmail_ThrowsLoginException()
        {
            var extension = Make_EmailPassword();

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
    }
}