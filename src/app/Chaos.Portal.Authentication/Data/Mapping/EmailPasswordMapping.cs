namespace Chaos.Portal.Authentication.Data.Mapping
{
    using System.Collections.Generic;
    using System.Data;

    using CHAOS.Data;

    using Chaos.Portal.Authentication.Data.Dto;

    public class EmailPasswordMapping : IReaderMapping<EmailPassword>
    {
        public IEnumerable<EmailPassword> Map(IDataReader reader)
        {
            while(reader.Read())
            {
                yield return new EmailPassword
                    {
                        UserGuid     = reader.GetGuid("UserGUID"),
                        Password     = reader.GetString("Password"),
                        DateCreated  = reader.GetDateTime("DateCreated"),
                        DateModified = reader.GetDateTime("DateModified")
                    };
            }
        }
    }
}
