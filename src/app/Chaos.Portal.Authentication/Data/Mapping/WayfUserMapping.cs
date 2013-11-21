namespace Chaos.Portal.Authentication.Data.Mapping
{
	using System.Collections.Generic;
	using System.Data;
	using CHAOS.Data;
	using Dto;
	using Model;

    public class WayfUserMapping : IReaderMapping<WayfUser>
	{
		public IEnumerable<WayfUser> Map(IDataReader reader)
		{
			while (reader.Read())
			{
				yield return
					new WayfUser
					{
						UserGuid = reader.GetGuid("UserGuid"),
						WayfId = reader.GetString("WayfId"),
						DateCreated = reader.GetDateTime("DateCreated"),
						DateModified = reader.GetDateTimeNullable("DateModified")
					};
			}
		}
	}
}