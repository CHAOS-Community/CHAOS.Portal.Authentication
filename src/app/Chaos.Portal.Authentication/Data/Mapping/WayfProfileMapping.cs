namespace Chaos.Portal.Authentication.Data.Mapping
{
	using System.Collections.Generic;
	using System.Data;
	using CHAOS.Data;
	using Dto;

	public class WayfProfileMapping : IReaderMapping<WayfProfile>
	{
		public IEnumerable<WayfProfile> Map(IDataReader reader)
		{
			while (reader.Read())
			{
				yield return
					new WayfProfile
					{
						UserGuid = reader.GetGuid("UserGuid"),
						WayfId = reader.GetString("WayfId"),
						GivenName = reader.GetString("GivenName"),
						SurName = reader.GetString("SurName"),
						DateCreated = reader.GetDateTime("DateCreated"),
						DateModified = reader.GetDateTimeNullable("DateModified")
					};
			}
		}
	}
}