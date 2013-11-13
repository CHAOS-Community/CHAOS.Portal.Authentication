using System;

namespace Chaos.Portal.Authentication.Data.Dto
{
	public class WayfUser
	{
		public Guid			UserGuid { get; set; }
		public string		WayfId { get; set; }
		public DateTime		DateCreated { get; set; }
		public DateTime?	DateModified { get; set; }
	}
}