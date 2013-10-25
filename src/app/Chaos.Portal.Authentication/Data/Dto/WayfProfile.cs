using System;

namespace Chaos.Portal.Authentication.Data.Dto
{
	public class WayfProfile
	{
		public Guid			UserGuid { get; set; }
		public string		WayfId { get; set; }
		public string		GivenName { get; set; }
		public string		SurName { get; set; }
		public string		CommonName { get; set; }
		public DateTime		DateCreated { get; set; }
		public DateTime?	DateModified { get; set; }
	}
}