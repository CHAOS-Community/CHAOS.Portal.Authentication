using CHAOS.Portal.Authentication.EmailPassword.Data;
using CHAOS.Portal.Authentication.EmailPassword.Module;
using CHAOS.Portal.Test;
using NUnit.Framework;

namespace CHAOS.Portal.Authentication.EmailPassword.Test
{
    public class EmailPasswordBaseTest : TestBase
    {
        public EmailPasswordModule EmailPasswordModule { get; set; }

        [SetUp]
        public new void SetUp()
        {
            base.SetUp();

            EmailPasswordModule = new EmailPasswordModule();
            EmailPasswordModule.Initialize("<Settings ConnectionString=\"metadata=res://*/EmailPasswordDB.csdl|res://*/EmailPasswordDB.ssdl|res://*/EmailPasswordDB.msl;provider=MySql.Data.MySqlClient;provider connection string=&quot;server=10.211.55.9;User Id=Portal;password=GECKONpbvu7000;Persist Security Info=True;database=EmailPassword&quot;\" ChangePasswordRequestSubject=\"Password change request\" SMTPPassword=\"NOREPLYpbvu7000\" FromEmailAddress=\"no-reply@chaos-community.org\">" +
                                                                               "<ChangePasswordRequestEmail><![CDATA[<?xml version=\"1.0\" encoding=\"UTF-16\"?>"+
                                                                                                            "<xsl:stylesheet xmlns:xsl=\"http://www.w3.org/1999/XSL/Transform\" version=\"1.0\" >"+
                                                                                                            "<xsl:template match=\"/\"><html xsl:version=\"1.0\" xmlns:xsl=\"http://www.w3.org/1999/XSL/Transform\" xmlns=\"http://www.w3.org/1999/xhtml\">" +
                                                                                                            "  <body>" +
                                                                                                            "     <a><xsl:attribute name=\"href\"><xsl:value-of select=\"//URL\" /></xsl:attribute>Follow the white rabbit</a>" +
                                                                                                            "  </body>" +
                                                                                                            "</html></xsl:template>" +
                                                                                                            "</xsl:stylesheet>]]></ChangePasswordRequestEmail></Settings>");

            using (var db = new EmailPasswordEntities("metadata=res://*/EmailPasswordDB.csdl|res://*/EmailPasswordDB.ssdl|res://*/EmailPasswordDB.msl;provider=MySql.Data.MySqlClient;provider connection string=\"server=10.211.55.9;User Id=Portal;password=GECKONpbvu7000;Persist Security Info=True;database=EmailPassword\""))
            {
				db.PreTest();

				db.EmailPassword_Create( UserAdministrator.GUID.ToByteArray(), "822f47daa187585d4e2475cad5cb06b4ed7f0ea7" );
            }


        }
    }
}
