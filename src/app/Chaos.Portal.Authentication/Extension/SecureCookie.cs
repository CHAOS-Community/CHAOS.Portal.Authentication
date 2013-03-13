namespace Chaos.Portal.Authentication.Extension
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Chaos.Portal.Authentication.Data;
    using Chaos.Portal.Authentication.Exception;
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

        public SecureCookie(IAuthenticationRepository repository)
        {
            AuthenticationRepository = repository;
        }

        #endregion
        #region Properties

        public IAuthenticationRepository AuthenticationRepository { get; set; }

        #endregion
        #region Business Logic

        public IEnumerable<Data.Dto.SecureCookie> Get(ICallContext callContext)
        {
            return AuthenticationRepository.SecureCookieGet(null, callContext.User.Guid, null);
        }

        public ScalarResult Delete(ICallContext callContext, Guid secureCookieGuid)
        {
            var result = AuthenticationRepository.SecureCookieDelete(secureCookieGuid, callContext.User.Guid);

            return new ScalarResult((int)result);
        }

        public Data.Dto.SecureCookie Create(ICallContext callContext)
        {
            if (callContext.IsAnonymousUser) throw new LoginException("Anonymous users cannot create a SecureCookie");

            var userGuid         = callContext.User.Guid;
            var sessionGuid      = callContext.Session.Guid;
            var secureCookieGuid = Guid.NewGuid();
            var passwordGuid     = Guid.NewGuid();

            AuthenticationRepository.SecureCookieCreate(secureCookieGuid, userGuid, passwordGuid, sessionGuid);

            return AuthenticationRepository.SecureCookieGet(secureCookieGuid, userGuid, passwordGuid).First();
        }

        #endregion

        public Data.Dto.SecureCookie Login(ICallContext callContext, Guid secureCookieGuid, Guid passwordGuid)
        {
            var cookie = AuthenticationRepository.SecureCookieGet(secureCookieGuid, null, passwordGuid).FirstOrDefault();

            if(cookie == null) throw new LoginException("Cookie not found");

            AuthenticationRepository.SecureCookieUse(cookie.Guid, cookie.UserGuid, null);

            if(cookie.DateUsed != null) throw new SecureCookieAlreadyConsumedException("All the users cookies has been deleted");

            cookie.PasswordGuid = Guid.NewGuid();

            AuthenticationRepository.SecureCookieCreate(cookie.Guid, cookie.UserGuid, cookie.PasswordGuid, callContext.Session.Guid);
            PortalRepository.SessionUpdate(callContext.Session.Guid, cookie.UserGuid);

            return cookie;
        }
    }
}