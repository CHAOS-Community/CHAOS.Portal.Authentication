namespace Chaos.Portal.Authentication.Data.Mapping
{
    using System.Collections.Generic;
    using System.Data;

    using CHAOS.Data;

    using Chaos.Portal.Authentication.Data.Dto;
    using Chaos.Portal.Authentication.Data.Model;

    public class SiteKeyMapping : IReaderMapping<AuthKey>
    {
        #region Implementation of IReaderMapping<out AuthKey>

        public IEnumerable<AuthKey> Map(IDataReader reader)
        {
            while(reader.Read())
            {
                yield return
                    new AuthKey()
                        {
                            Token      = reader.GetString("Token"),
                            UserGuid = reader.GetGuid("UserGuid")
                        };
            }
        }

        #endregion
    }
}