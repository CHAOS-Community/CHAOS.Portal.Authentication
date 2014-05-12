using Chaos.Portal.Authentication.Configuration;
using DotNetAuth.OAuth2;

namespace Chaos.Portal.Authentication.OAuth
{
	internal class GenericProvider : OAuth2ProviderDefinition
	{
		public string UserInfoEndpoint { get; set; }

		public GenericProvider(OAuthSettings configuration)
		{
			AuthorizationEndpointUri = configuration.AuthorizationEndpoint;
			TokenEndpointUri = configuration.TokenEndpoint;
			UserInfoEndpoint = configuration.UserInfoEndpoint;
		}
	}
}