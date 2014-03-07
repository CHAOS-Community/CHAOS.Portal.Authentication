using System.Configuration;

namespace CHAOS.Portal.OAuth.Controllers
{
	public class ProviderConfiguration : ConfigurationSection
	{
		[ConfigurationProperty("ClientId", IsRequired = true)]
		public string ClientId
		{
			get { return (string) this["ClientId"]; }
			set { this["ClientId"] = value; }
		}

		[ConfigurationProperty("ClientSecret", IsRequired = true)]
		public string ClientSecret
		{
			get { return (string)this["ClientSecret"]; }
			set { this["ClientSecret"] = value; }
		}

		[ConfigurationProperty("AuthorizationEndpoint", IsRequired = true)]
		public string AuthorizationEndpoint
		{
			get { return (string)this["AuthorizationEndpoint"]; }
			set { this["AuthorizationEndpoint"] = value; }
		}

		[ConfigurationProperty("TokenEndpoint", IsRequired = true)]
		public string TokenEndpoint
		{
			get { return (string)this["TokenEndpoint"]; }
			set { this["TokenEndpoint"] = value; }
		}

		[ConfigurationProperty("UserInfoEndpoint", IsRequired = true)]
		public string UserInfoEndpoint
		{
			get { return (string)this["UserInfoEndpoint"]; }
			set { this["UserInfoEndpoint"] = value; }
		}
	}
}