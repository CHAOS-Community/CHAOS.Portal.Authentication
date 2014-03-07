using System;

namespace Chaos.Portal.Authentication.Data.Model
{
	public class OAuthUser
	{
		public Guid UserGuid { get; set; }
		public string OAuthId { get; set; }
		public DateTime DateCreated { get; set; }
		public DateTime? DateModified { get; set; } 
	}
}