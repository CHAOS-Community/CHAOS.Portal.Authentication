using Chaos.Portal.Core.Data.Model;
using CHAOS.Serialization;

namespace Chaos.Portal.Authentication.OAuth
{
	public class LoginEndPoint : AResult
	{
		[Serialize]
		public string Uri { get; set; }
		[Serialize]
		public string StateCode { get; set; }
	}
}