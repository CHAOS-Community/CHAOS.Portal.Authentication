using System;
using System.Threading.Tasks;
using Chaos.Portal.Authentication.Configuration;
using DotNetAuth.OAuth2;
using DotNetAuth.Profiles;

namespace Chaos.Portal.Authentication.OAuth
{
	public class OAuthClient : IOAuthClient
	{
		private readonly LoginProvider _provider;
		private readonly ProfileProperty[] _profileProperties = { ProfileProperty.Email, ProfileProperty.DisplayName, ProfileProperty.UniqueID };

		public OAuthClient(OAuthSettings configuration)
		{
			_provider = new LoginProvider
			{
				AppId = configuration.ClientId,
				AppSecret = configuration.ClientSecret,
				Definition = new GenericLoginProviderDefinition(new GenericProvider(configuration))
			};
		}

		public LoginEndPoint GetLoginEndPoint(string callbackUrl)
		{
			var stateManager = new LoginStateManager();
			var authorizationUrl = GetAuthenticationUri(_provider, callbackUrl, stateManager, _profileProperties);
			authorizationUrl.Wait();

			return new LoginEndPoint {StateCode = stateManager.StateCode, Uri = authorizationUrl.Result.AbsoluteUri};
		}

		public Profile ProcessLogin(string callbackUrl, string responseUrl, string stateCode)
		{
			var stateManager = new LoginStateManager {StateCode = stateCode};

			var response = Login.GetProfile(_provider, new Uri(responseUrl), callbackUrl, stateManager, _profileProperties);
			response.Wait();

			if(response.IsFaulted || response.Result.UniqueID == null) throw new System.Exception("Failed to get profile");

			return response.Result;
		}

		private static Task<Uri> GetAuthenticationUri(LoginProvider provider, string loginProcessUri, ILoginStateManager stateManager, ProfileProperty[] requiredProperties)
		{
			var generalLoginStataManager = new DotNetAuth.Profiles.LoginStateManager(stateManager);
			switch (provider.Definition.ProtocolType)
			{
				case ProtocolTypes.OAuth2:
					var scope = provider.Definition.GetRequiredScope(requiredProperties);
					return Task.Factory.StartNew(() => OAuth2Process.GetAuthenticationUri(provider.Definition.GetOAuth2Definition(), provider.GetOAuth2Credentials(), Uri.EscapeDataString(loginProcessUri), scope, generalLoginStataManager));
				default:
					throw new System.Exception("Invalid provider. Provider's protocol type is not set or supported.");
			}
		}
	}
}