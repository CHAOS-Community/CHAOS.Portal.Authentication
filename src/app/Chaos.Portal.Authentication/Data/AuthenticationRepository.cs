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
            throw new NotImplementedException();
        }

        public uint SecureCookieCreate(Guid userGuid, Guid guid, Guid passwordGuid, Guid sessionGuid)
        {
            throw new NotImplementedException();
        }

        public uint SecureCookieDelete(Guid whereUserGuid, Guid guid)
        {
            throw new NotImplementedException();
        }

        public uint SecureCookieUse(Guid? userGuid, Guid? guid, Guid? passwordGuid)
        {
            throw new NotImplementedException();
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