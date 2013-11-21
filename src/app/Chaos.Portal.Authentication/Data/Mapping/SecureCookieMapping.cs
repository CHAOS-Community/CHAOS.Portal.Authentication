namespace Chaos.Portal.Authentication.Data.Mapping
{
    using System.Collections.Generic;
    using System.Data;

    using CHAOS.Data;

    using Chaos.Portal.Authentication.Data.Dto;
    using Dto.v6;

    public class SecureCookieMapping : IReaderMapping<SecureCookie>
    {
        #region Implementation of IReaderMapping<out EmailPassword>

        public IEnumerable<SecureCookie> Map(IDataReader reader)
        {
            while(reader.Read())
            {
                yield return
                    new SecureCookie
                        {
                            Guid = reader.GetGuid("SecureCookieGUID"),
                            PasswordGuid = reader.GetGuid("PasswordGUID"),
                            UserGuid = reader.GetGuid("UserGUID"),
                            DateCreated = reader.GetDateTime("DateCreated"),
                            DateUsed = reader.GetDateTimeNullable("DateUsed")
                        };
            }
        }

        #endregion
    }
}