using DotNetAuth.OAuth2;

namespace CHAOS.Portal.OAuth.Controllers
{
	public class GenericProvider : OAuth2ProviderDefinition
	{
		public string UserInfoEndpoint { get; set; }
	}
}