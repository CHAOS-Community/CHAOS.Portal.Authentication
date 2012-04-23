using System.Linq;
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
			SecureCookieModule.Initialize( "<Settings ConnectionString=\"metadata=res://*/Data.SecureCookie.csdl|res://*/Data.SecureCookie.ssdl|res://*/Data.SecureCookie.msl;provider=MySql.Data.MySqlClient;provider connection string=&quot;server=10.211.55.9;User Id=Portal;password=GECKONpbvu7000;Persist Security Info=True;database=SecureCookie&quot;\"/>" );

            using (var db = new Data.SecureCookieEntities("metadata=res://*/Data.SecureCookie.csdl|res://*/Data.SecureCookie.ssdl|res://*/Data.SecureCookie.msl;provider=MySql.Data.MySqlClient;provider connection string=\"server=10.211.55.9;User Id=Portal;password=GECKONpbvu7000;Persist Security Info=True;database=SecureCookie\""))
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
