using System.Collections.Generic;
using System.Data;
using CHAOS.Data;
using Chaos.Portal.Authentication.Data.Model;

namespace Chaos.Portal.Authentication.Data.Mapping
{
	public class OAuthUserMapping : IReaderMapping<OAuthUser>
	{
		public IEnumerable<OAuthUser> Map(IDataReader reader)
		{
			while (reader.Read())
			{
				yield return new OAuthUser
				{
					UserGuid = reader.GetGuid("UserGuid"),
					OAuthId = reader.GetString("OAuthId"),
					DateCreated = reader.GetDateTime("DateCreated"),
					DateModified = reader.GetDateTime("DateModified")
				};
			}
		}
	}
}