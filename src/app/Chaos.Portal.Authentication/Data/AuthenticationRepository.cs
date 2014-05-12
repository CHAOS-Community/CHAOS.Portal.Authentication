namespace Chaos.Portal.Authentication.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using CHAOS.Data;
    using CHAOS.Data.MySql;
    using Core.Exceptions;
    using Dto;
    using Dto.v6;
    using Mapping;
    using Model;

    using MySql.Data.MySqlClient;

    public class AuthenticationRepository : IAuthenticationRepository
    {
        #region Construction

        static AuthenticationRepository()
        {
            ReaderExtensions.Mappings.Add(typeof(EmailPassword), new EmailPasswordMapping());
            ReaderExtensions.Mappings.Add(typeof(FacebookUser), new FacebookUserMapping());
            ReaderExtensions.Mappings.Add(typeof(SecureCookie), new SecureCookieMapping());
            ReaderExtensions.Mappings.Add(typeof(WayfUser), new WayfUserMapping());
            ReaderExtensions.Mappings.Add(typeof(AuthKey), new AuthKeyMapping());
        }

        public AuthenticationRepository(string connectionString)
        {
            Gateway = new Gateway(connectionString);

			OAuth = new OAuthRepository(Gateway);
        }

        #endregion
        #region Properties

        public Gateway Gateway { get; set; }
		public IOAuthRepository OAuth { get; private set; }

        #endregion
        #region Business Logic

	    

	    public IEnumerable<SecureCookie> SecureCookieGet(Guid? guid, Guid? userGuid, Guid? passwordGuid)
        {
            var results = Gateway.ExecuteQuery<SecureCookie>("SecureCookie_Get", new[]
                {
                    new MySqlParameter("UserGuid", userGuid.HasValue ? userGuid.Value.ToByteArray() : null), 
                    new MySqlParameter("SecureCookieGuid", guid.HasValue ? guid.Value.ToByteArray() : null), 
                    new MySqlParameter("PasswordGuid", passwordGuid.HasValue ? passwordGuid.Value.ToByteArray() : null), 
                });

            return results;
        }

        public uint SecureCookieCreate(Guid guid, Guid userGuid, Guid passwordGuid, Guid sessionGuid)
        {
            var result = Gateway.ExecuteNonQuery("SecureCookie_Create", new[]
                {
                    new MySqlParameter("SecureCookieGuid", guid.ToByteArray()), 
                    new MySqlParameter("PasswordGuid", passwordGuid.ToByteArray()), 
                    new MySqlParameter("UserGuid", userGuid.ToByteArray()), 
                    new MySqlParameter("SessionGuid", sessionGuid.ToByteArray()) 
                });

            return (uint)result;
        }

        public uint SecureCookieDelete(Guid whereGuid, Guid whereUserGuid)
        {
            var result = Gateway.ExecuteNonQuery("SecureCookie_Delete", new[]
                {
                    new MySqlParameter("WhereUserGuid", whereUserGuid.ToByteArray()), 
                    new MySqlParameter("WhereSecureCookieGuid", whereGuid.ToByteArray()) 
                });

            return (uint)result;
        }

        public uint SecureCookieUse(Guid? guid, Guid? userGuid, Guid? passwordGuid)
        {
            var result = Gateway.ExecuteNonQuery("SecureCookie_Use", new[]
                {
                    new MySqlParameter("WhereUserGuid", userGuid.HasValue ? userGuid.Value.ToByteArray() : null), 
                    new MySqlParameter("WhereSecureCookieGuid", guid.HasValue ? guid.Value.ToByteArray() : null), 
                    new MySqlParameter("WherePasswordGuid", passwordGuid.HasValue ? passwordGuid.Value.ToByteArray() : null)
                });

            return (uint)result;
        }

        public EmailPassword EmailPasswordGet( Guid guid, string password )
        {
            var results = Gateway.ExecuteQuery<EmailPassword>("EmailPassword_Get", new[]
                {
                    new MySqlParameter("UserGuid", guid.ToByteArray()), 
                    new MySqlParameter("Password", password) 
                });

            return results.FirstOrDefault();
        }

		public uint EmailPasswordUpdate(Guid userGuid, string password)
	    {
			var result = Gateway.ExecuteNonQuery("EmailPassword_Update", new[]
                {
                    new MySqlParameter("UserGUID", userGuid.ToByteArray()), 
                    new MySqlParameter("NewPassword", password) 
                });

			return (uint)result;
	    }

		public IList<AuthKey> AuthKeyGet(Guid? userGuid, string token)
		{
			var results = Gateway.ExecuteQuery<AuthKey>("AuthKey_Get", new[]
                {
                    new MySqlParameter("UserGuid", userGuid.HasValue ? userGuid.Value.ToByteArray() : null),
                    new MySqlParameter("Token", token) 
                });

			return results;
		}

	    public uint AuthKeyCreate(string token, Guid userGuid, string name)
		{
			var results = Gateway.ExecuteNonQuery("AuthKey_Create", new[]
                {
                    new MySqlParameter("Token", token),
                    new MySqlParameter("UserGuid", userGuid.ToByteArray()),
                    new MySqlParameter("Name", name)
                });

			return (uint)results;
		}

		public uint AuthKeyDelete(Guid userGuid, string name)
		{
			var results = Gateway.ExecuteNonQuery("AuthKey_Delete", new[]
                {
                    new MySqlParameter("userGuid", userGuid.ToByteArray()),
                    new MySqlParameter("name", name) 
                });

			return (uint)results;
		}

	    public WayfUser WayfProfileGet(string wayfId)
	    {
			var results = Gateway.ExecuteQuery<WayfUser>("WayfProfile_Get", new[]
                {
                    new MySqlParameter("WayfId", wayfId) 
                });

			return results.FirstOrDefault();
	    }

	    public uint WayfProfileUpdate(Guid userGuid, string wayfId)
	    {
			var result = Gateway.ExecuteNonQuery("WayfProfile_Update", new[]
                {
                    new MySqlParameter("UserGUID", userGuid.ToByteArray()), 
                    new MySqlParameter("WayfId", wayfId)
                });

			return (uint)result;
	    }

        public FacebookUser FacebookUserGet(ulong facebookId)
        {
            var results = Gateway.ExecuteQuery<FacebookUser>("Facebook_User_Join_Get", new[]
            {
                new MySqlParameter("FacebookUserId", facebookId)
            });

            var first = results.FirstOrDefault();

            if(first == null)
                throw new UnhandledException("User not found in database");

            return first;
        }

        public uint FacebookUserCreate(ulong facebookUserId, Guid userGuid)
        {
            var result = Gateway.ExecuteNonQuery("Facebook_User_Join_Create", new[]
            {
                new MySqlParameter("FacebookUserId", facebookUserId),
                new MySqlParameter("UserGuid", userGuid.ToByteArray())
            });

            if(result == 0)
                throw new UnhandledException("Associating Facebook user with chaos failed in database");

            return (uint) result;
        }

        #endregion
    }
}