namespace Chaos.Portal.Authentication.Tests.Extension
{
    using System;
    using Authentication.Extension.v6;
    using Core.Data.Model;
    using Core.Exceptions;
    using Data.Model;
    using Exception;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class FacebookTests : TestBase
    {
        [Test]
        public void Login_ValidSignedRequestAndUserAlreadyExist_CreateAndReturnAnAuthenticatedSession()
        {
            var extension = Make_FacebookExtension();
            var signedRequest = "valid request";
            var fbUser = Make_FacebookUser();
            var session = Make_Session();
            FacebookClient.Setup(m => m.GetUser(signedRequest)).Returns(fbUser.Id);
            AuthenticationRepository.Setup(m => m.FacebookUserGet(fbUser.Id)).Returns(fbUser);
            PortalRepository.Setup(m => m.SessionCreate(fbUser.UserGuid)).Returns(session);

            var result = extension.Login(signedRequest);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Guid, Is.EqualTo(session.Guid));
            PortalRepository.Verify(m => m.UserCreate(It.IsAny<Guid>(), It.IsAny<string>()), Times.Never());
        }

        [Test]
        public void Login_ValidSignedRequestAndUserDoesntExist_CreateUserAndReturnAuthenticatedSession()
        {
            var extension = Make_FacebookExtension();
            var signedRequest = "valid request";
            var facebookId = 1ul;
            var session = Make_Session();
            FacebookClient.Setup(m => m.GetUser(signedRequest)).Returns(facebookId);
            AuthenticationRepository.Setup(m => m.FacebookUserGet(facebookId)).Throws(new UnhandledException());
            PortalRepository.Setup(m => m.SessionCreate(It.IsAny<Guid>())).Returns(session);

            var result = extension.Login(signedRequest);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Guid, Is.EqualTo(session.Guid));
            PortalRepository.Verify(m => m.UserCreate(It.IsAny<Guid>(), It.IsAny<string>()));
            AuthenticationRepository.Verify(m => m.FacebookUserCreate(facebookId, It.IsAny<Guid>()));
        }

        [Test, ExpectedException(typeof(LoginException))]
        public void Login_InvalidSignedRequest_ThrowException()
        {
            var extension = Make_FacebookExtension();
            var signedRequest = "invalid request";
            FacebookClient.Setup(m => m.GetUser(signedRequest)).Throws(new LoginException());

            extension.Login(signedRequest);
        }

        private static FacebookUser Make_FacebookUser()
        {
            return new FacebookUser
            {
                Id = 1,
                UserGuid = new Guid("11000000-0000-0000-0000-000000000011")
            };
        }

        private static Session Make_Session()
        {
            return new Session { Guid = new Guid("10000000-0000-0000-0000-000000000001") };
        }

        private Facebook Make_FacebookExtension()
        {
            return (Facebook) new Facebook(AuthenticationModule.Object).WithPortalRequest(PortalRequest.Object);
        }
    }
}
