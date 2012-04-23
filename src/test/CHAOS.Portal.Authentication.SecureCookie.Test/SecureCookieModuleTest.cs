using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CHAOS.Extensions;
using CHAOS.Portal.Authentication.SecureCookie.Exception;
using NUnit.Framework;

namespace CHAOS.Portal.Authentication.SecureCookie.Test
{
    [TestFixture]
    public class SecureCookieModuleTest : SecureCookieBaseTest
    {
        [Test]
        public void Should_Create_SecureCookie()
        {
            var cookie = SecureCookieModule.Create( AdminCallContext );
        
            Assert.IsNotNull( cookie );
            Assert.AreEqual( UserAdministrator.SessionGUID.ToByteArray(), cookie.SessionGUID.ToByteArray() );
        }

        [Test]
        public void Should_Get_The_Users_SecureCookies()
        {
            var cookies = SecureCookieModule.Get( AdminCallContext );

            Assert.Greater( cookies.Count(), 0 );

            foreach( var secureCookie in cookies )
            {
                Assert.AreEqual( secureCookie.UserGUID.ToByteArray(), UserAdministrator.GUID.ToByteArray() );
            }
        }

        [Test]
        public void Should_Delete_One_SecureCookie()
        {
            IList<string> cookies = new List<string>( );
            cookies.Add( SecureCookieModule.Create( AdminCallContext ).SecureCookieGUID.ToUUID().ToString() );

            var result = SecureCookieModule.Delete( AdminCallContext, cookies );

            Assert.AreEqual(1,result.Value);
        }

        [Test]
        public void Should_Delete_Multiple_SecureCookie()
        {
            IList<string> cookies = new List<string>( );
            cookies.Add( SecureCookieModule.Create( AdminCallContext ).SecureCookieGUID.ToUUID().ToString() );
            cookies.Add( SecureCookieModule.Create( AdminCallContext ).SecureCookieGUID.ToUUID().ToString() );
            cookies.Add( SecureCookieModule.Create( AdminCallContext ).SecureCookieGUID.ToUUID().ToString() );
            cookies.Add( SecureCookieModule.Create( AdminCallContext ).SecureCookieGUID.ToUUID().ToString() );
            cookies.Add( SecureCookieModule.Create( AdminCallContext ).SecureCookieGUID.ToUUID().ToString() );

            var result = SecureCookieModule.Delete( AdminCallContext, cookies );

            Assert.AreEqual(5,result.Value);
        }

        [Test]
        public void Should_Login()
        {
            var result = SecureCookieModule.Login( AdminCallContext, SecureCookie.SecureCookieGUID.ToUUID(), SecureCookie.PasswordGUID.ToUUID() );

            Assert.AreEqual( SecureCookie.SecureCookieGUID, result.SecureCookieGUID );
            Assert.IsNull( result.DateUsed );
            Assert.AreNotEqual( SecureCookie.PasswordGUID.ToByteArray(), result.PasswordGUID.ToByteArray() );
        }

        [Test, ExpectedException(typeof(SecureCookieAlreadyConsumedException))]
        public void Should_Throw_Exception_If_SecureCookie_Is_Used_More_Than_Once()
        {
            SecureCookieModule.Login( AdminCallContext, SecureCookie.SecureCookieGUID.ToUUID(), SecureCookie.PasswordGUID.ToUUID() );
            SecureCookieModule.Login( AdminCallContext, SecureCookie.SecureCookieGUID.ToUUID(), SecureCookie.PasswordGUID.ToUUID() );
        }
    }
}
