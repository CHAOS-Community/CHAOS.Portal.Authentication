namespace Chaos.Portal.Authentication.Data.Model
{
    using System;

    public class WayfUser
	{
		public Guid			UserGuid { get; set; }
		public string		WayfId { get; set; }
		public DateTime		DateCreated { get; set; }
		public DateTime?	DateModified { get; set; }
	}
}