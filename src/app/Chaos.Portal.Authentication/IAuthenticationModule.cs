namespace Chaos.Portal.Authentication
{
    using Core;
    using Core.Module;
    using Core.Request;
    using Data;

    public interface IAuthenticationModule : IModule
    {
        event RequestDelegate.PortalRequestHandler OnUserLoggedIn;
        event RequestDelegate.PortalRequestHandler OnUserCreated;

        IAuthenticationRepository AuthenticationRepository { get; }
        IPortalApplication PortalApplication { get; }
        IFacebookClient FacebookClient { get; set; }
        void OnOnUserLoggedIn(RequestDelegate.PortalRequestArgs args);
        void OnOnUserCreated(RequestDelegate.PortalRequestArgs args);
    }
}