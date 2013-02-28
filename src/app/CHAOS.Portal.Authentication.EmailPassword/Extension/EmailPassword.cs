using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;

using CHAOS.Portal.Authentication.EmailPassword.Data;
using CHAOS.Portal.Authentication.Exception;
using Chaos.Portal;
using Chaos.Portal.Data.Dto;
using Chaos.Portal.Extension;

namespace CHAOS.Portal.Authentication.EmailPassword.Extension
{
    [PortalExtension(configurationName : "EmailPassword")]
    public class EmailPassword : AExtension
    {
        #region Properties

        public string               ConnectionString { get; set; }
        public MailAddress          FromEmailAddress { get; set; }
        public string               ChangePasswordRequestSubject { get; set; }
        public string               SmtpPassword { get; set; }
        public XslCompiledTransform ChangePasswordRequestEmailXslt { get; set; }

        public EmailPasswordEntities NewEmailPasswordDataContext
        {
            get
            {
				return new EmailPasswordEntities( ConnectionString );
            }
        }

        private IEmailPasswordRepository EmailPasswordRepository { get; set; }

        #endregion
        #region Initialization

        public override IExtension WithConfiguration( string configuration )
		{
            return WithConfiguration(configuration, new EmailPasswordRepository());
        }

        public IExtension WithConfiguration( string configuration, IEmailPasswordRepository emailPasswordRepository )
		{
            if (configuration == null)
                throw new NullReferenceException("Configuration for EmailPasswordModule cannot be null");

            var config = XDocument.Parse( configuration ).Root;

            ConnectionString               = config.Attribute("ConnectionString").Value;
            EmailPasswordRepository        = emailPasswordRepository.WithConnectionString(ConnectionString);
            FromEmailAddress               = new MailAddress( config.Attribute( "FromEmailAddress" ).Value );
            SmtpPassword                   = config.Attribute( "SMTPPassword" ).Value;
            ChangePasswordRequestSubject   = config.Attribute("ChangePasswordRequestSubject").Value;
            ChangePasswordRequestEmailXslt = new XslCompiledTransform();
            ChangePasswordRequestEmailXslt.Load( XmlReader.Create( new StringReader( config.Element("ChangePasswordRequestEmail").Value ) ) );

            return this;
		}
        
        #endregion
        #region Login (Email/password)

        public UserInfo Login( ICallContext callContext, string email, string password )
        {
            var user = PortalRepository.UserInfoGet(email);

            if (user == null) throw new LoginException("Login failed, either email or password is incorrect");

            using( var emailPasswordDB = NewEmailPasswordDataContext )
            {
                if( emailPasswordDB.EmailPassword_Get( user.Guid.ToByteArray(), GeneratePasswordHash( password ) ).FirstOrDefault() == null )
                    throw new LoginException( "Login failed, either email or password is incorrect" );
            }

            var result = PortalRepository.SessionUpdate(callContext.Session.Guid, user.Guid);

            if(result == null) throw new LoginException("Session could not be updated");

            return PortalRepository.UserInfoGet(null, callContext.Session.Guid, null).First();
        }

        #endregion
        #region Change Password

//        public ScalarResult ChangePasswordRequest(ICallContext callContext, string email, string password, string url)
//        {
//            var user = PortalRepository.GetUserInfo(email);
//
//            var guid = new Guid();
//            var xml  = string.Format("<ChangePassword UserGUID=\"{0}\" PasswordHash=\"{1}\" />", user.Guid, GeneratePasswordHash(password));
//            PortalRepository.CreateTicket(guid, (uint) Chaos.Portal.Data.TicketType.ChangePassword, xml, null);
//
//            // TODO: Make Send mail part of the portal SDK
//            var message = new MailMessage();
//
//            message.To.Add(new MailAddress(user.Email));
//            message.From       = FromEmailAddress;
//            message.Subject    = ChangePasswordRequestSubject;
//            message.Body       = GenerateChangePasswordRequestEmail(guid, url).ToString(SaveOptions.DisableFormatting);
//            message.IsBodyHtml = true;
//                
//            var smtp  = new SmtpClient("smtp.gmail.com", 587);
//            smtp.EnableSsl   = true;
//            smtp.Credentials = new NetworkCredential(FromEmailAddress.Address, SmtpPassword);
//
//            smtp.Send(message);
//
//            return new ScalarResult(1);
//        }

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

        private XDocument GenerateChangePasswordRequestEmail(Guid ticketGUID, string url)
        {
            var source = XDocument.Parse(string.Format("<Xml><URL>{0}</URL></Xml>", HttpUtility.HtmlDecode(url).Replace("$Token$", ticketGUID.ToString()).Replace("&", "&amp;")));
            var result = new XDocument();

            using (XmlWriter writer = result.CreateWriter())
            {
                ChangePasswordRequestEmailXslt.Transform(source.CreateReader(), writer);
            }

            return result;
        }

        #endregion
        #region Helper methods

        private string GeneratePasswordHash( string password )
        {
            var byteHash = SHA1.Create().ComputeHash(Encoding.Unicode.GetBytes(password));

             return BitConverter.ToString( byteHash ).Replace("-", "").ToLower();
        }

        #endregion
    }
}
