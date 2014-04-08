namespace Chaos.Portal.Authentication
{
	using System;

    using Configuration;
    using Core;
    using Core.Module;
    using Core.Request;
    using Data;
	using OAuth;

    public interface IAuthenticationModule : IModule
    {
        event RequestDelegate.PortalRequestHandler OnUserLoggedIn;
        event RequestDelegate.PortalRequestHandler OnUserCreated;

        IAuthenticationRepository AuthenticationRepository { get; }
        IPortalApplication PortalApplication { get; }
        IFacebookClient FacebookClient { get; set; }
        IOAuthClient OAuthClient { get; set; }
        AuthenticationSettings AuthenticationSettings { get; }
        void OnOnUserLoggedIn(RequestDelegate.PortalRequestArgs args);
        void OnOnUserCreated(RequestDelegate.PortalRequestArgs args);

	    void AddUserInfoUpdateListener<T>(Action<UserInfoUpdate<T>> listener);
	    void OnUserInfoUpdate<T>(Guid userGuid, T userInfo);
    }
}