namespace CHAOS.Portal.Authentication.SecureCookie.Extension
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    using Chaos.Portal;

    using CHAOS.Portal.Authentication.SecureCookie.Data;

    using Chaos.Portal.Exceptions;
    using Chaos.Portal.Extension;
    using Chaos.Portal.Data.Dto;

    [PortalExtension(configurationName : "SecureCookie")]
    public class SecureCookie : AExtension
    {
        #region Properties

        private string ConnectionString { get; set; }

		private SecureCookieEntities NewSecureCookieDataContext
        {
			get { return new SecureCookieEntities( ConnectionString ); }
        }

        #endregion
        #region Initialization

        public override IExtension WithConfiguration( string configuration )
        {
            var config = XDocument.Parse( configuration ).Root;

            ConnectionString = config.Attribute( "ConnectionString" ).Value;

            return this;
        }

        #endregion
        #region Login

//		public Data.SecureCookie Login( ICallContext callContext, UUID guid, UUID passwordGUID )
//        {
//			Data.SecureCookie cookie;
//
//            using( var db = NewSecureCookieDataContext )
//            {
//               cookie = db.SecureCookie_Get( null, guid.ToByteArray(), passwordGUID.ToByteArray() ).FirstOrDefault();
//        
//                if( cookie == null )
//                    throw new LoginException( "Cookie doesn't exist" );
//
//                // Cookies must only be used once
//                if( cookie.DateUsed != null )
//                {
//                    db.SecureCookie_Update( cookie.UserGUID.ToByteArray(), null, null );
//                    throw new SecureCookieAlreadyConsumedException( "The SecureCookie has already been consumed, all the users cookies has been deleted" );
//                }
//
//				var userGUID         = cookie.UserGUID.ToByteArray();
//            	var secureCookieGUID = cookie.SecureCookieGUID.ToByteArray();
//            	var newPasswordGUID  = new UUID().ToByteArray();
//
//                db.SecureCookie_Update( null, cookie.SecureCookieGUID.ToByteArray(), null );
//                db.SecureCookie_Create( userGUID, secureCookieGUID, newPasswordGUID, callContext.User.SessionGuid.Value.ToByteArray() );
//
//                cookie = db.SecureCookie_Get( userGUID, secureCookieGUID, newPasswordGUID ).First();
//            }
//
//            PortalRepository.SessionUpdate(cookie.UserGUID, callContext.Session.Guid);
//
//          //  callContext.Cache.Remove( string.Format( "[UserInfo:sid={0}]", callContext.Session.GUID ) );
//
//            return cookie;
//        }

        #endregion
    }
}
