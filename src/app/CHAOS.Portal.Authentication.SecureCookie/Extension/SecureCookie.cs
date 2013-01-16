using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using CHAOS.Portal.Authentication.Exception;
using CHAOS.Portal.Authentication.SecureCookie.Data;
using CHAOS.Portal.Authentication.SecureCookie.Exception;
using Chaos.Portal;
using Chaos.Portal.Data.Dto.Standard;
using Chaos.Portal.Data.EF;
using Chaos.Portal.Exceptions;
using Chaos.Portal.Extension;

namespace CHAOS.Portal.Authentication.SecureCookie.Extension
{
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
        #region CREATE

		public Data.SecureCookie Create( ICallContext callContext )
        {
            if( callContext.IsAnonymousUser )
                throw new InsufficientPermissionsException( "Anonumous users cannot create a SecureCookie" );

            using( var db = NewSecureCookieDataContext )
            {
				byte[] secureCookieGUID = new UUID().ToByteArray();
				byte[] userGUID         = callContext.User.GUID.ToByteArray();
				byte[] passwordGUID     = new UUID().ToByteArray();
				byte[] sessionGUID      = callContext.Session.GUID.ToByteArray();

                db.SecureCookie_Create( userGUID, secureCookieGUID, passwordGUID, sessionGUID );

                return db.SecureCookie_Get( userGUID, secureCookieGUID, passwordGUID ).First();
            }
        }

        #endregion
        #region GET

        public IEnumerable<Data.SecureCookie> Get( ICallContext callContext )
        {
            using( var db = NewSecureCookieDataContext )
            {
                return db.SecureCookie_Get( callContext.User.GUID.ToByteArray(), null, null ).ToList();
            }
        }

        #endregion
        #region Delete

        public ScalarResult Delete( ICallContext callContext, IList<string> GUIDs )
        {
            using( var db = NewSecureCookieDataContext )
            {
                foreach( var secureCookieGUID in GUIDs.Select( s => new UUID( s )) )
                {
                    db.SecureCookie_Delete( callContext.User.GUID.ToByteArray(), secureCookieGUID.ToByteArray() );
                }
                
                return new ScalarResult( 1 );
            }
        }

        #endregion
        #region Login

		public Data.SecureCookie Login( ICallContext callContext, UUID guid, UUID passwordGUID )
        {
			Data.SecureCookie cookie;

            using( var db = NewSecureCookieDataContext )
            {
               cookie = db.SecureCookie_Get( null, guid.ToByteArray(), passwordGUID.ToByteArray() ).FirstOrDefault();
        
                if( cookie == null )
                    throw new LoginException( "Cookie doesn't exist" );

                // Cookies must only be used once
                if( cookie.DateUsed != null )
                {
                    db.SecureCookie_Update( cookie.UserGUID.ToByteArray(), null, null );
                    throw new SecureCookieAlreadyConsumedException( "The SecureCookie has already been consumed, all the users cookies has been deleted" );
                }

				var userGUID         = cookie.UserGUID.ToByteArray();
            	var secureCookieGUID = cookie.SecureCookieGUID.ToByteArray();
            	var newPasswordGUID  = new UUID().ToByteArray();

                db.SecureCookie_Update( null, cookie.SecureCookieGUID.ToByteArray(), null );
                db.SecureCookie_Create( userGUID, secureCookieGUID, newPasswordGUID, callContext.User.SessionGUID.ToByteArray() );

                cookie = db.SecureCookie_Get( userGUID, secureCookieGUID, newPasswordGUID ).First();
            }

            using( var db = new PortalEntities() )
            {
                db.Session_Update( cookie.UserGUID.ToByteArray(), callContext.Session.GUID.ToByteArray(), null );
            }

          //  callContext.Cache.Remove( string.Format( "[UserInfo:sid={0}]", callContext.Session.GUID ) );

            return cookie;
        }

        #endregion
    }
}
