using DotNetAuth.OAuth2;
using DotNetAuth.Profiles;

namespace Chaos.Portal.Authentication.OAuth
{
	internal class GenericLoginProviderDefinition : LoginProviderDefinition
	{
		private readonly GenericProvider _genericProvider;

		public GenericLoginProviderDefinition(GenericProvider genericProvider)
		{
			_genericProvider = genericProvider;
			ProtocolType = ProtocolTypes.OAuth2;
		}

		public override ProfileProperty[] GetSupportedProperties()
		{
			return new[] {
             ProfileProperty.UniqueID,
             ProfileProperty.DisplayName,
             ProfileProperty.Email        
            };
		}

		public override Profile ParseProfile(string content)
		{
			var json = Newtonsoft.Json.Linq.JObject.Parse(content);
			var profile = new IntranetAccessProfile
			{
				UniqueID = json.First.First.Value<string>("id"),
				DisplayName = json.First.First.Value<string>("name"),
				Email = json.First.First.Value<string>("email"),
				HasIntranetAccess = json.First.First.Value<bool>("intranet")
			};
			return profile;
		}

		public override string GetProfileEndpoint(ProfileProperty[] requiredProperties)
		{
			return _genericProvider.UserInfoEndpoint;
		}

		public override string GetRequiredScope(ProfileProperty[] requiredProperties)
		{
			return "";
		}

		public override OAuth2ProviderDefinition GetOAuth2Definition()
		{
			return _genericProvider;
		}
	}
}