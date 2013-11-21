namespace Chaos.Portal.Authentication.Data.Mapping
{
    using System.Collections.Generic;
    using System.Data;

    using CHAOS.Data;
    using Model;

    public class FacebookUserMapping : IReaderMapping<FacebookUser>
    {
        #region Implementation of IReaderMapping<out AuthKey>

        public IEnumerable<FacebookUser> Map(IDataReader reader)
        {
            while(reader.Read())
            {
                yield return
                    new FacebookUser
                    {
                        Id = (ulong) reader.GetInt64("FacebookUserId"),
                        UserGuid = reader.GetGuid("UserGuid")
                    };
            }
        }

        #endregion
    }
}