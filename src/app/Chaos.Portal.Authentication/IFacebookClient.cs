namespace Chaos.Portal.Authentication
{
    public interface IFacebookClient
    {
        ulong GetUser(string signedRequest);
    }
}