using System.Configuration;

namespace CHAOS.Portal.OAuth.Controllers
{
	public class PortalConfiguration : ConfigurationSection
	{
		[ConfigurationProperty("AuthKeyToken", IsRequired = true)]
		public string AuthKeyToken
		{
			get { return (string)this["AuthKeyToken"]; }
			set { this["AuthKeyToken"] = value; }
		}
	}
}