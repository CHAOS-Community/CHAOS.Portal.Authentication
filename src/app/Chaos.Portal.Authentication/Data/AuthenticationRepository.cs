namespace Chaos.Portal.Authentication.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using CHAOS.Data;
    using CHAOS.Data.MySql;

    using Chaos.Portal.Authentication.Data.Dto;
    using Chaos.Portal.Authentication.Data.Mapping;

    using MySql.Data.MySqlClient;

    public class AuthenticationRepository : IAuthenticationRepository
    {
        #region Construction

        static AuthenticationRepository()
        {
            ReaderExtensions.Mappings.Add(typeof(EmailPassword), new EmailPasswordMapping());
            ReaderExtensions.Mappings.Add(typeof(SecureCookie), new SecureCookieMapping());
        }

        public AuthenticationRepository(string connectionString)
        {
            Gateway = new Gateway(connectionString);
        }

        #endregion
        #region Properties

        public Gateway Gateway { get; set; }

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

	    public WayfProfile WayfProfileGet(string wayfId)
	    {
			var results = Gateway.ExecuteQuery<WayfProfile>("WayfProfile_Get", new[]
                {
                    new MySqlParameter("WayfId", wayfId) 
                });

			return results.FirstOrDefault();
	    }

	    public uint WayfProfileUpdate(Guid userGuid, string wayfId, string givenName, string surName, string commonName)
	    {
			var result = Gateway.ExecuteNonQuery("WayfProfile_Update", new[]
                {
                    new MySqlParameter("UserGUID", userGuid.ToByteArray()), 
                    new MySqlParameter("WayfId", wayfId),
                    new MySqlParameter("GivenName", givenName),
                    new MySqlParameter("SurName", surName),
                    new MySqlParameter("CommonName", commonName)
                });

			return (uint)result;
	    }

	    #endregion
    }
}