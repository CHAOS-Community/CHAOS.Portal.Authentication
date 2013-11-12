namespace Chaos.Portal.Authentication.Data.Mapping
{
    using System.Collections.Generic;
    using System.Data;

    using CHAOS.Data;

    using Chaos.Portal.Authentication.Data.Dto;
    using Chaos.Portal.Authentication.Data.Model;

    public class SiteKeyMapping : IReaderMapping<SiteKey>
    {
        #region Implementation of IReaderMapping<out SiteKey>

        public IEnumerable<SiteKey> Map(IDataReader reader)
        {
            while(reader.Read())
            {
                yield return
                    new SiteKey()
                        {
                            Key      = reader.GetString("Key"),
                            UserGuid = reader.GetGuid("UserGuid")
                        };
            }
        }

        #endregion
    }
}