namespace Chaos.Portal.Authentication.Configuration
{
    using CHAOS.Serialization;
    using CHAOS.Serialization.XML;
    using Core.Module;

    public class AuthenticationSettings : IModuleSettings
    {
        [Serialize]
        public string ConnectionString { get; set; }

        [Serialize]
        public FacebookSettings Facebook { get; set; }

        [Serialize]
        public OAuthSettings OAuth { get; set; }
        
        [Serialize]
        public PasswordSettings Password { get; set; }

        public AuthenticationSettings()
        {
            ConnectionString = "";
            Facebook = new FacebookSettings
                {
                    AppId = "",
                    AppSecret = ""
                };
            OAuth = new OAuthSettings
                {
                    AuthorizationEndpoint = "",
                    ClientId = "",
                    ClientSecret = "",
                    TokenEndpoint = "",
                    UserInfoEndpoint = ""
                };
            Password = new PasswordSettings
                {
                    UseSalt = false,
                    Iterations = 1
                };
        }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(ConnectionString);
        }
    }

    public class PasswordSettings
    {
        [Serialize]
        public bool UseSalt { get; set; }

        [Serialize]
        public uint Iterations { get; set; }
    }

    public class OAuthSettings
	{
		[SerializeXML(true)]
		public string ClientId { get; set; }
		[SerializeXML(true)]
		public string ClientSecret { get; set; }
		[SerializeXML(true)]
		public string AuthorizationEndpoint { get; set; }
		[SerializeXML(true)]
		public string TokenEndpoint { get; set; }
		[SerializeXML(true)]
		public string UserInfoEndpoint { get; set; }
	}

	public class FacebookSettings
    {
        [SerializeXML(true)]
        public string AppId { get; set; }

        [SerializeXML(true)]
        public string AppSecret { get; set; }
    }
}