namespace Chaos.Portal.Authentication.Extension.v6
{
    using System;
    using Core.Data.Model;
    using Core.Exceptions;
    using Core.Extension;
    using Core.Request;
    using Data.Model;

    public class Facebook : AExtension
    {
        private const string DefaultEmail = "N/A";

        public IAuthenticationModule AuthenticationModule { get; set; }

        public Facebook(IAuthenticationModule authenticationModule) : base(authenticationModule.PortalApplication)
        {
            AuthenticationModule = authenticationModule;
        }

        public Session Login(string signedRequest)
        {
            var facebookUserId = AuthenticationModule.FacebookClient.GetUser(signedRequest);
            var user = GetUser(facebookUserId);
            
            var session = AuthenticateSession(user); 

            AuthenticationModule.OnOnUserLoggedIn(new RequestDelegate.PortalRequestArgs(Request));

            return session;
        }

        private Session AuthenticateSession(FacebookUser user)
        {
            return IsSessionProvided() 
                   ? PortalRepository.SessionUpdate(Request.Session.Guid, user.UserGuid)
                   : PortalRepository.SessionCreate(user.UserGuid) ;
        }

        private bool IsSessionProvided()
        {
            return Request.Session != null;
        }

        private FacebookUser GetUser(ulong facebookUserId)
        {
            try
            {
                return GetExistingUser(facebookUserId);
            }
            catch (UnhandledException)
            {
                return CreateNewUser(facebookUserId);
            }
        }

        private FacebookUser GetExistingUser(ulong facebookUserId)
        {
            return AuthenticationModule.AuthenticationRepository.FacebookUserGet(facebookUserId);
        }

        private FacebookUser CreateNewUser(ulong facebookUserId)
        {
            var userGuid = Guid.NewGuid();

            PortalRepository.UserCreate(userGuid, DefaultEmail);
            AuthenticationModule.AuthenticationRepository.FacebookUserCreate(facebookUserId, userGuid);

            AuthenticationModule.OnOnUserCreated(new RequestDelegate.PortalRequestArgs(Request));

            return new FacebookUser
            {
                Id = facebookUserId,
                UserGuid = userGuid
            };
        }
    }
}