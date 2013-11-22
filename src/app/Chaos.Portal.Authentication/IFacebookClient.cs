namespace Chaos.Portal.Authentication
{
    public interface IFacebookClient
    {
        ulong GetUserId(string signedRequest);
    }
}