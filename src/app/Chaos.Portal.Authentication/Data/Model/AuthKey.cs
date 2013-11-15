namespace Chaos.Portal.Authentication.Data.Model
{
    using System;

    using CHAOS.Serialization;

    using Core.Data.Model;

    public class AuthKey : AResult
    {
        [Serialize]
        public string Token { get; set; }

		[Serialize]
        public Guid UserGuid{ get; set; }

	    public AuthKey()
	    {
		    
	    }

		public AuthKey(string token, Guid userGuid)
		{
			Token = token;
			UserGuid = userGuid;
		}
    }
}