using CHAOS.Portal.Authentication.EmailPassword.Data;
using Moq;
using NUnit.Framework;

namespace CHAOS.Portal.Authentication.EmailPassword.Test
{
    public class EmailPasswordBaseTest 
    {
        public Extension.EmailPassword EmailPassword { get; set; }
        public Mock<IEmailPasswordRepository> EmailPasswordRepository { get; set; }

        [SetUp]
        public void SetUp()
        {
            EmailPasswordRepository.Setup(m => m.WithConnectionString("CONNECTION_STRING")).Returns(EmailPasswordRepository.Object);

            EmailPassword = new Extension.EmailPassword();
            EmailPassword.WithConfiguration("<Settings ConnectionString=\"CONNECTION_STRING\" ChangePasswordRequestSubject=\"Password change request\" SMTPPassword=\"NOREPLYpbvu7000\" FromEmailAddress=\"no-reply@chaos-community.org\">" +
                                                                               "<ChangePasswordRequestEmail><![CDATA[<?xml version=\"1.0\" encoding=\"UTF-16\"?>"+
                                                                                                            "<xsl:stylesheet xmlns:xsl=\"http://www.w3.org/1999/XSL/Transform\" version=\"1.0\" >"+
                                                                                                            "<xsl:template match=\"/\"><html xsl:version=\"1.0\" xmlns:xsl=\"http://www.w3.org/1999/XSL/Transform\" xmlns=\"http://www.w3.org/1999/xhtml\">" +
                                                                                                            "  <body>" +
                                                                                                            "     <a><xsl:attribute name=\"href\"><xsl:value-of select=\"//URL\" /></xsl:attribute>Follow the white rabbit</a>" +
                                                                                                            "  </body>" +
                                                                                                            "</html></xsl:template>" +
                                                                                                            "</xsl:stylesheet>]]></ChangePasswordRequestEmail></Settings>",
                                           EmailPasswordRepository.Object);
        }
    }
}
