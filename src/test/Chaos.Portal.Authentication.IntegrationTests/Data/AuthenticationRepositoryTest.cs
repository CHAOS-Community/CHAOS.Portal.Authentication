namespace Chaos.Portal.Authentication.IntegrationTests.Data
{
    using System;

    using Chaos.Portal.Authentication.Data.Dto;

    using NUnit.Framework;

    [TestFixture]
    public class AuthenticationRepositoryTest : TestBase
    {
        [Test]
        public void EmailPasswordGet_ThatExist_ReturnEmailPassword()
        {
            var expected = Make_EmailPasswordThatExist();

            var actual = AuthenticationRepository.EmailPasswordGet(expected.UserGuid, expected.Password);

            Assert.That(actual.UserGuid, Is.EqualTo(expected.UserGuid));
            Assert.That(actual.Password, Is.EqualTo(expected.Password));
            Assert.That(actual.DateCreated, Is.EqualTo(expected.DateCreated));
            Assert.That(actual.DateModified, Is.EqualTo(expected.DateModified));
        }

        private EmailPassword Make_EmailPasswordThatExist()
        {
            return new EmailPassword
                {
                    UserGuid     = new Guid("00000010-0000-0000-0000-000000000001"),
                    Password     = "somepassword",
                    DateCreated  = new DateTime(2000,01,01),
                    DateModified = new DateTime(2000,01,02)
                };
        }
    }
}