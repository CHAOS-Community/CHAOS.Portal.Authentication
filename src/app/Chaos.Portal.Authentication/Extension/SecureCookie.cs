namespace Chaos.Portal.Authentication.Extension
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Chaos.Portal.Authentication.Data;
    using Chaos.Portal.Authentication.Exception;
    using Chaos.Portal.Data;
    using Chaos.Portal.Data.Dto;
    using Chaos.Portal.Extension;

    public class SecureCookie : AExtension
    {
        #region Overrides of AExtension

        public override IExtension WithConfiguration(string configuration)
        {
            return this;
        }

        #endregion
        #region Initialization

        public SecureCookie(IAuthenticationRepository repository, IPortalRepository portalRepository)
        {
            AuthenticationRepository = repository;
            PortalRepository = portalRepository;
        }

        #endregion
        #region Properties

        public IAuthenticationRepository AuthenticationRepository { get; set; }
        public IPortalRepository PortalRepository { get; set; }

        #endregion
        #region Business Logic

        public IEnumerable<Data.Dto.SecureCookie> Get(ICallContext callContext)
        {
            return AuthenticationRepository.SecureCookieGet(callContext.User.Guid, null, null);
        }

        public ScalarResult Delete(ICallContext callContext, Guid secureCookieGuid)
        {
            var result = AuthenticationRepository.SecureCookieDelete(callContext.User.Guid, secureCookieGuid);

            return new ScalarResult((int)result);
        }

        public Data.Dto.SecureCookie Create(ICallContext callContext)
        {
            if (callContext.IsAnonymousUser) throw new LoginException("Anonymous users cannot create a SecureCookie");

            var userGuid         = callContext.User.Guid;
            var sessionGuid      = callContext.Session.Guid;
            var secureCookieGuid = Guid.NewGuid();
            var passwordGuid     = Guid.NewGuid();

            AuthenticationRepository.SecureCookieCreate(userGuid, secureCookieGuid, passwordGuid, sessionGuid);

            return AuthenticationRepository.SecureCookieGet(userGuid, secureCookieGuid, passwordGuid).First();
        }

        #endregion

        public Data.Dto.SecureCookie Login(ICallContext callContext, Guid secureCookieGuid, Guid passwordGuid)
        {
            var cookie = AuthenticationRepository.SecureCookieGet(null, secureCookieGuid, passwordGuid).FirstOrDefault();

            if(cookie == null) throw new LoginException("Cookie not found");

            AuthenticationRepository.SecureCookieUse(cookie.UserGuid, cookie.Guid, null);

            if(cookie.DateUsed != null) throw new SecureCookieAlreadyConsumedException("All the users cookies has been deleted");

            cookie.PasswordGuid = Guid.NewGuid();

            AuthenticationRepository.SecureCookieCreate(cookie.UserGuid, cookie.Guid, cookie.PasswordGuid, callContext.Session.Guid);
            PortalRepository.SessionUpdate(callContext.Session.Guid, cookie.UserGuid);

            return cookie;
        }
    }
}