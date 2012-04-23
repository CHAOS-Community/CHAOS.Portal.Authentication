using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;
using CHAOS.Extensions;
using CHAOS.Portal.Authentication.EmailPassword.Data;
using CHAOS.Portal.Authentication.Exception;
using CHAOS.Portal.Core;
using CHAOS.Portal.Core.Module;
using CHAOS.Portal.Data.EF;
using CHAOS.Portal.Exception;
using UserInfo = CHAOS.Portal.DTO.Standard.UserInfo;

namespace CHAOS.Portal.Authentication.EmailPassword.Module
{
    [Module("EmailPassword")]
    public class EmailPasswordModule : AModule
    {
        #region Properties

        public string               ConnectionString { get; set; }
        public MailAddress          FromEmailAddress { get; set; }
        public string               ChangePasswordRequestSubject { get; set; }
        public string               SMTPPassword { get; set; }
        public XslCompiledTransform ChangePasswordRequestEmailXSLT { get; set; }

        public EmailPasswordEntities NewEmailPasswordDataContext
        {
            get
            {
				return new EmailPasswordEntities( ConnectionString );
            }
        }

        #endregion
        #region Construction

        public override void Initialize( string configuration )
		{
            var config = XDocument.Parse( configuration ).Root;

            ConnectionString             = config.Attribute("ConnectionString").Value;
            FromEmailAddress             = new MailAddress( config.Attribute( "FromEmailAddress" ).Value );
            SMTPPassword                 = config.Attribute( "SMTPPassword" ).Value;
            ChangePasswordRequestSubject = config.Attribute("ChangePasswordRequestSubject").Value;
            ChangePasswordRequestEmailXSLT = new XslCompiledTransform();
            ChangePasswordRequestEmailXSLT.Load( XmlReader.Create( new StringReader( config.Element("ChangePasswordRequestEmail").Value ) ) );
        }

        #endregion
        #region Login (Email/password)

        [Datatype("EmailPassword","Login")]
        public UserInfo Login( ICallContext callContext, string email, string password )
        {
            using( var emailPasswordDB = NewEmailPasswordDataContext )
            using( var portalDB = new PortalEntities() )
            {
                var user = portalDB.UserInfo_Get( null, null, email ).FirstOrDefault();

                if( user == null || emailPasswordDB.EmailPassword_Get( user.GUID.ToByteArray(), GeneratePasswordHash( password ) ).FirstOrDefault() == null )
                    throw new LoginException( "Login failed, either email or password is incorrect" );

                int updateResult = portalDB.Session_Update( user.GUID.ToUUID().ToByteArray(), callContext.Session.GUID.ToByteArray(), null ).First().Value;
                
                if( updateResult == 0 )
                    throw new UnhandledException( "Session was not updated in database" );

                callContext.Cache.Remove( string.Format( "[UserInfo:sid={0}]", callContext.Session.GUID ) );
            }

            using( var portalDB = new PortalEntities() )
            {
                return  portalDB.UserInfo_Get( null, callContext.Session.GUID.ToByteArray(), null ).ToDTO().First();
            }
        }

        #endregion
        //#region Change Password

        //[Datatype("EmailPassword","ChangePasswordRequest")]
        //public ScalarResult ChangePasswordRequest( ICallContext callContext, string email, string password, string url )
        //{
        //    using( var db = new PortalEntities() )
        //    {
        //        var user = db.UserInfo_Get( null, null, email ).FirstOrDefault();

        //        if( user == null )
        //            throw new ArgumentException( "Email not found" ); // TODO: Replace with custom Exception

        //        var guid = new UUID();
        //        db.Ticket_Create( guid.ToByteArray(), (int?) TicketType.ChangePassword, string.Format( "<ChangePassword UserGUID=\"{0}\" PasswordHash=\"{1}\" />", user.GUID, GeneratePasswordHash( password ) ), null );
                
        //        // TODO: Make Send mail part of the portal SDK
        //        var message = new MailMessage();

        //        message.To.Add( new MailAddress( user.Email ) );
        //        message.From       = FromEmailAddress;
        //        message.Subject    = ChangePasswordRequestSubject;
        //        message.Body       = GenerateChangePasswordRequestEmail( guid, url ).ToString( SaveOptions.DisableFormatting );
        //        message.IsBodyHtml = true;
                
        //        var smtp  = new SmtpClient( "smtp.gmail.com", 587 );
        //        smtp.EnableSsl   = true;
        //        smtp.Credentials = new NetworkCredential( FromEmailAddress.Address, SMTPPassword );

        //        smtp.Send( message );

        //        return new ScalarResult( 1 );
        //    }
        //}

        //[Datatype("EmailPassword","ChangePassword")]
        //public Session ChangePassword( ICallContext callContext, string ticketGUID )
        //{
        //    using( var db = new PortalEntities() )
        //    {
        //        var ticket   = db.Ticket_Get( new UUID( ticketGUID ).ToByteArray() ).First();
        //        var xml      = XDocument.Parse( ticket.XML ).Root;
        //        var userGUID = new UUID( xml.Attribute("UserGUID").Value );
        //        var password = xml.Attribute("PasswordHash").Value;

        //        db.Ticket_Use( new UUID( ticketGUID ).ToByteArray() );
        //        db.Session_Update( userGUID.ToByteArray(), callContext.User.SessionGUID.ToByteArray(), null );

        //        using( EmailPasswordEntities edb = NewEmailPasswordDataContext )
        //        {
        //            if( edb.EmailPassword_Get( userGUID.ToByteArray(), null ).FirstOrDefault() == null )
        //                edb.EmailPassword_Create( userGUID.ToByteArray(), password );
        //            else
        //                edb.EmailPassword_Update( userGUID.ToByteArray(), password );
        //        }

        //        int? totalCount = 1;

        //        return db.Session_Get( callContext.Session.GUID.ToByteArray(), null ).First();
        //    }
        //}

        //private XDocument GenerateChangePasswordRequestEmail( UUID ticketGUID, string url)
        //{
        //    var source = XDocument.Parse(string.Format("<Xml><URL>{0}</URL></Xml>", HttpUtility.HtmlDecode(url).Replace("$Token$", ticketGUID.ToString() ).Replace("&", "&amp;")));
        //    var result = new XDocument();

        //    using( XmlWriter writer = result.CreateWriter() )
        //    {
        //        ChangePasswordRequestEmailXSLT.Transform( source.CreateReader(), writer );
        //    }

        //    return result;
        //}

        //#endregion
        #region Helper methods

        private string GeneratePasswordHash( string password )
        {
            byte[] byteHash = new SHA1Managed().ComputeHash( Encoding.Unicode.GetBytes( password ) );

             return BitConverter.ToString( byteHash ).Replace("-", "").ToLower();
        }

        #endregion
    }
}
