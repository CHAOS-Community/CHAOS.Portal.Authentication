﻿namespace Chaos.Portal.Authentication.IntegrationTests.Data
{
    using System;
    using System.Linq;
    using Authentication.Data.Dto.v6;
    using Authentication.Data.Model;
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

        [Test]
        public void SecureCookieCreate_GivenCookieDoesntExist_Return1()
        {
            var expected = 1u;
            var cookie = Make_SecureCookieThatDoesntExist();
            var sessionGuid = new Guid("00000012-0000-0000-0005-000000000021");

            var actual = AuthenticationRepository.SecureCookieCreate(cookie.Guid, cookie.UserGuid, cookie.PasswordGuid, sessionGuid);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void SecureCookieGet_GivenCookieExist_ReturnCookie()
        {
            var expected = Make_SecureCookieThatExist();

            var actual = AuthenticationRepository.SecureCookieGet(expected.Guid, expected.UserGuid, expected.PasswordGuid).First();

            Assert.That(actual.Guid, Is.EqualTo(expected.Guid));
            Assert.That(actual.PasswordGuid, Is.EqualTo(expected.PasswordGuid));
            Assert.That(actual.UserGuid, Is.EqualTo(expected.UserGuid));
            Assert.That(actual.DateCreated, Is.EqualTo(expected.DateCreated));
            Assert.That(actual.DateUsed, Is.EqualTo(expected.DateUsed));
        }

        [Test]
        public void SecureCookieDelete_GivenCookieExist_ReturnOne()
        {
            var cookie = Make_SecureCookieThatExist();
            var expected = 1u;

            var actual = AuthenticationRepository.SecureCookieDelete(cookie.Guid, cookie.UserGuid);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void SecureCookieUse_GivenCookieExist_ReturnOne()
        {
            var cookie = Make_SecureCookieThatExist();
            var expected = 1u;

            var actual = AuthenticationRepository.SecureCookieUse(cookie.Guid, cookie.UserGuid, cookie.PasswordGuid);

            Assert.That(actual, Is.EqualTo(expected));
        }

        private EmailPassword Make_EmailPasswordThatExist()
        {
            return new EmailPassword
                {
                    UserGuid     = new Guid("00000010-0000-0000-0000-000000000001"),
                    Password     = "somepassword",
                    DateCreated  = new DateTime(2000, 12, 31),
                    DateModified = new DateTime(2001, 12, 31)
                };
        }

        private SecureCookie Make_SecureCookieThatDoesntExist()
        {
            return new SecureCookie
                {
                    Guid        = new Guid("00000012-0000-0000-0000-000000000021"),
                    PasswordGuid = new Guid("00003012-0000-0000-0000-000000000321"),
                    UserGuid    = new Guid("00003412-0000-0000-0000-000000004321"),
                    DateUsed    = null
                };
        }
        
        private SecureCookie Make_SecureCookieThatExist()
        {
            return new SecureCookie
                {
                    Guid            = new Guid("00000012-0000-0000-0000-000000000021"),
                    PasswordGuid    = new Guid("00003012-0000-0000-0000-000000000321"),
                    UserGuid        = new Guid("00000020-0000-0000-0000-000000000002"),
                    DateCreated     = new DateTime(2000, 12, 31),
                    DateUsed        = null
                };
        }
    }
}