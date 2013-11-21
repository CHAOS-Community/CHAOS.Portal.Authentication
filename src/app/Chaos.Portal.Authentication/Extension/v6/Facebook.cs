namespace Chaos.Portal.Authentication.Extension.v6
{
    using System;
    using Core;
    using Core.Data.Model;
    using Core.Exceptions;
    using Core.Extension;
    using Data;
    using Data.Model;

    public class Facebook : AExtension
    {
        private const string DefaultEmail = "N/A";

        public IAuthenticationRepository AuthenticationRepository { get; set; }
        public IFacebookClient FacebookClient { get; set; }

        public Facebook(IPortalApplication portalApplication, IAuthenticationRepository authenticationRepository, IFacebookClient facebookClient) : base(portalApplication)
        {
            AuthenticationRepository = authenticationRepository;
            FacebookClient = facebookClient;
        }

        public Session Login(string signedRequest)
        {
            var facebookUserId = FacebookClient.GetUser(signedRequest);
            var user = GetUser(facebookUserId);
            
            var session = PortalRepository.SessionCreate(user.UserGuid);
            
            return session;
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
            return AuthenticationRepository.FacebookUserGet(facebookUserId);
        }

        private FacebookUser CreateNewUser(ulong facebookUserId)
        {
            var userGuid = Guid.NewGuid();

            PortalRepository.UserCreate(userGuid, DefaultEmail);
            AuthenticationRepository.FacebookUserCreate(facebookUserId, userGuid);

            return new FacebookUser
            {
                Id = facebookUserId,
                UserGuid = userGuid
            };
        }
    }
}