﻿namespace Chaos.Portal.Authentication
{
	using System;

    using Configuration;
    using Core;
    using Core.Module;
    using Core.Request;
    using Data;
	using OAuth;

    public interface IAuthenticationModule : IModuleConfig
    {
        event RequestDelegate.PortalRequestHandler OnUserLoggedIn;
        event RequestDelegate.PortalRequestHandler OnUserCreated;

        IAuthenticationRepository AuthenticationRepository { get; }
        IPortalApplication PortalApplication { get; }
        IFacebookClient FacebookClient { get; set; }
        IOAuthClient OAuthClient { get; set; }
        AuthenticationSettings Settings { get; }
        void OnOnUserLoggedIn(RequestDelegate.PortalRequestArgs args);
        void OnOnUserCreated(RequestDelegate.PortalRequestArgs args);

	    void AddUserInfoUpdateListener<T>(Action<UserInfoUpdate<T>> listener);
	    void OnUserInfoUpdate<T>(Guid userGuid, T userInfo);
	    void OnOnWayfUserLoggedIn(WayfProfileArgs args);
	    event WayfHandler OnWayfUserLoggedIn;
    }
}