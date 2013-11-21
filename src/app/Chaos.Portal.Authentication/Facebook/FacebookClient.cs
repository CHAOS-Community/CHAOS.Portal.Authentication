namespace Chaos.Portal.Authentication.Facebook
{
    using System.Web.UI;
    using Configuration;

    public class FacebookClient : IFacebookClient
    {
        public FacebookClient(FacebookSettings settings)
        {
            Settings = settings;
        }

        private FacebookSettings Settings { get; set; }
        
        public ulong GetUser(string signedRequest)
        {
            var client = new global::Facebook.FacebookClient
            {
                AppId = Settings.AppId,
                AppSecret = Settings.AppSecret
            };

//            var result = client.Get("oauth/access_token", new
//            {
//                client_id = client.AppId,
//                client_secret = client.AppSecret,
//                grant_type = "client_credentials"
//            });
//            client.AccessToken = result.access_token;

            var userId = (client.ParseSignedRequest(signedRequest) as dynamic).user_id;

            return userId;
        }
    }
}
