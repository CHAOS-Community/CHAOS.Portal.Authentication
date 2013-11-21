namespace Chaos.Portal.Authentication.Facebook
{
    using Configuration;

    public class FacebookClient : IFacebookClient
    {
        public FacebookClient(FacebookSettings facebook)
        {
            
        }

        public ulong GetUser(string signedRequest)
        {
            return 0;
        }
    }
}
