namespace Chaos.Portal.Authentication.Configuration
{
    using CHAOS.Serialization;
    using CHAOS.Serialization.XML;

    public class Settings
    {
        [Serialize]
        public string ConnectionString { get; set; }

        [Serialize]
        public FacebookSettings Facebook { get; set; }
    }

    public class FacebookSettings
    {
        [SerializeXML(true)]
        public string AppId { get; set; }

        [SerializeXML(true)]
        public string AppSecret { get; set; }
    }
}