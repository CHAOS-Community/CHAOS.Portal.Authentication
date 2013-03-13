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

        public IEnumerable<SecureCookie> SecureCookieGet(Guid? userGuid, Guid? guid, Guid? passwordGuid)
        {
            var results = Gateway.ExecuteQuery<SecureCookie>("SecureCookie_Get", new[]
                {
                    new MySqlParameter("UserGuid", userGuid.HasValue ? userGuid.Value.ToByteArray() : null), 
                    new MySqlParameter("SecureCookieGuid", guid.HasValue ? guid.Value.ToByteArray() : null), 
                    new MySqlParameter("PasswordGuid", passwordGuid.HasValue ? passwordGuid.Value.ToByteArray() : null), 
                });

            return results;
        }

        public uint SecureCookieCreate(Guid userGuid, Guid guid, Guid passwordGuid, Guid sessionGuid)
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

        public uint SecureCookieDelete(Guid whereUserGuid, Guid whereGuid)
        {
            var result = Gateway.ExecuteNonQuery("SecureCookie_Delete", new[]
                {
                    new MySqlParameter("WhereUserGuid", whereUserGuid.ToByteArray()), 
                    new MySqlParameter("WhereSecureCookieGuid", whereGuid.ToByteArray()) 
                });

            return (uint)result;
        }

        public uint SecureCookieUse(Guid? userGuid, Guid? guid, Guid? passwordGuid)
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

        #endregion
    }
}