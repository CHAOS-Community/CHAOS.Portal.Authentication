namespace CHAOS.Portal.Authentication.EmailPassword.Data
{
    using System;
    using System.Linq;

    using CHAOS.Data;
    using CHAOS.Data.MySql;
    using CHAOS.Portal.Authentication.EmailPassword.Data.Dto;
    using CHAOS.Portal.Authentication.EmailPassword.Data.Mapping;

    using MySql.Data.MySqlClient;

    public class EmailPasswordRepository : IEmailPasswordRepository
    {
        #region Construction

        static EmailPasswordRepository()
        {
            ReaderExtensions.Mappings.Add(typeof(EmailPassword), new EmailPasswordMapping());
        }

        public IEmailPasswordRepository WithConnectionString(string connectionString)
        {
            Gateway = new Gateway(connectionString);

            return this;
        }

        #endregion
        #region Properties

        public Gateway Gateway { get; set; }

        #endregion
        #region Business Logic

        public EmailPassword EmailPasswordGet( Guid guid, string password )
        {
            var results = Gateway.ExecuteQuery<EmailPassword>("EmailPassword_Get", new[]
                {
                    new MySqlParameter("UserGUID", guid.ToByteArray()), 
                    new MySqlParameter("Password", password) 
                });

            return results.FirstOrDefault();
        }

        #endregion
    }
}