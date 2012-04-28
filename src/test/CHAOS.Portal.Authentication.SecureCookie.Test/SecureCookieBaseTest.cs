using System.Linq;
using CHAOS.Portal.Authentication.SecureCookie.Data;
using CHAOS.Portal.Authentication.SecureCookie.Module;
using CHAOS.Portal.Test;
using NUnit.Framework;

namespace CHAOS.Portal.Authentication.SecureCookie.Test
{
    [TestFixture]
    public class SecureCookieBaseTest : TestBase
    {
        public SecureCookieModule SecureCookieModule { get; set; }

        public Data.SecureCookie SecureCookie { get; set; }

        [SetUp]
        public new void SetUp()
        {
            base.SetUp();

            SecureCookieModule = new SecureCookieModule(  );
			SecureCookieModule.Initialize( "<Settings ConnectionString=\"metadata=res://*/SecureCookieDB.csdl|res://*/SecureCookieDB.ssdl|res://*/SecureCookieDB.msl;provider=MySql.Data.MySqlClient;provider connection string=&quot;server=10.211.55.9;User Id=Portal;password=GECKONpbvu7000;Persist Security Info=True;database=SecureCookie&quot;\"/>" );

            using (var db = new SecureCookieEntities("metadata=res://*/SecureCookieDB.csdl|res://*/SecureCookieDB.ssdl|res://*/SecureCookieDB.msl;provider=MySql.Data.MySqlClient;provider connection string=\"server=10.211.55.9;User Id=Portal;password=GECKONpbvu7000;Persist Security Info=True;database=SecureCookie\""))
            {
				db.PreTest();

                db.SecureCookie_Create( UserAdministrator.GUID.ToByteArray(), new UUID().ToByteArray(), new UUID().ToByteArray(), UserAdministrator.SessionGUID.ToByteArray() );
                db.SecureCookie_Create( UserAdministrator.GUID.ToByteArray(), new UUID().ToByteArray(), new UUID().ToByteArray(), UserAdministrator.SessionGUID.ToByteArray() );
                db.SecureCookie_Create( UserAnonymous.GUID.ToByteArray(),     new UUID().ToByteArray(), new UUID().ToByteArray(), UserAnonymous.SessionGUID.ToByteArray() );

            	byte[] secureCookieGUID = new UUID().ToByteArray();
            	byte[] passwordGUID     = new UUID().ToByteArray();

                db.SecureCookie_Create( UserAdministrator.GUID.ToByteArray(), secureCookieGUID, passwordGUID, UserAdministrator.SessionGUID.ToByteArray() );
                SecureCookie = db.SecureCookie_Get( UserAdministrator.GUID.ToByteArray(), secureCookieGUID, passwordGUID ).First();
            }
        }
    }
}
