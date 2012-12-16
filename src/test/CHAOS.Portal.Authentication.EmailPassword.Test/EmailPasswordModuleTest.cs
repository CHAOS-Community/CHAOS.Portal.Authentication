using CHAOS.Portal.Authentication.Exception;
using NUnit.Framework;

namespace CHAOS.Portal.Authentication.EmailPassword.Test
{
    [TestFixture]
    public class EmailPasswordModuleTest : EmailPasswordBaseTest
    {
        //[Test]
        //public void Should_Login()
        //{
        //    var user = EmailPasswordModule.Login(AnonCallContext, UserAdministrator.Email, "pbvu7000");

        //    Assert.IsNotNull(user);
        //}

        //[Test,ExpectedException(typeof(LoginException))]
        //public void Should_Fail_Login()
        //{
        //    EmailPasswordModule.Login( AnonCallContext, UserAdministrator.Email, "wrong");
        //}

        //[Test]
        //public void Should_Send_Password_Change_RequestMail()
        //{
        //    EmailPasswordModule.ChangePasswordRequest( AdminCallContext, "jesper@geckon.com", "abcdefghijklmnopqrstuvwxyzæøå", "http://localhost:8080?redirectUrl=SOMEBASE64PATH&ticketGUID=$Token$" );
        //}

        //[Test]
        //public void Should_Send_Password_Change_Callback()
        //{
        //    using( var db = new PortalEntities() )
        //    {
        //        var guid = new UUID();
        //        db.Ticket_Create( guid.ToByteArray(), (int?) TicketType.ChangePassword, string.Format( "<ChangePassword UserGUID=\"{0}\" PasswordHash=\"{1}\" />", UserAdministrator.GUID, "somepasswordhash" ), null );

        //        var session = EmailPasswordModule.ChangePassword( AdminCallContext, guid.ToString() );

        //        Assert.AreEqual( UserAdministrator.SessionGUID, session.GUID );
        //    }
        //}
    }
}
