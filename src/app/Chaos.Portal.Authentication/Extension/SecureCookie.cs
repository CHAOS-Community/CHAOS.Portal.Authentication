namespace Chaos.Portal.Authentication.Extension
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Chaos.Portal.Authentication.Data;
    using Chaos.Portal.Authentication.Exception;
    using Chaos.Portal.Core;
    using Chaos.Portal.Core.Data.Model;
    using Chaos.Portal.Core.Extension;

    public class SecureCookie : AExtension
    {
        #region Initialization

        public SecureCookie(IPortalApplication portalApplication, IAuthenticationRepository repository): base(portalApplication)
        {
            AuthenticationRepository = repository;
        }

        #endregion
        #region Properties

        public IAuthenticationRepository AuthenticationRepository { get; set; }

        #endregion
        #region Business Logic

        public IEnumerable<Data.Dto.v6.SecureCookie> Get()
        {
            return AuthenticationRepository.SecureCookieGet(null, Request.User.Guid, null);
        }

        public ScalarResult Delete(Guid guid)
        {
            var result = AuthenticationRepository.SecureCookieDelete(guid, Request.User.Guid);

            return new ScalarResult((int)result);
        }

        public Data.Dto.v6.SecureCookie Create()
        {
            if (Request.IsAnonymousUser) throw new LoginException("Anonymous users cannot create a SecureCookie");

            var userGuid         = Request.User.Guid;
            var sessionGuid      = Request.Session.Guid;
            var secureCookieGuid = Guid.NewGuid();
            var passwordGuid     = Guid.NewGuid();

            AuthenticationRepository.SecureCookieCreate(secureCookieGuid, userGuid, passwordGuid, sessionGuid);

            return AuthenticationRepository.SecureCookieGet(secureCookieGuid, userGuid, passwordGuid).First();
        }

        #endregion

        public Data.Dto.v6.SecureCookie Login(Guid guid, Guid passwordGuid)
        {
            var cookie = AuthenticationRepository.SecureCookieGet(guid, null, passwordGuid).FirstOrDefault();

            if(cookie == null) throw new LoginException("Cookie not found");

            AuthenticationRepository.SecureCookieUse(cookie.Guid, cookie.UserGuid, null);

            if(cookie.DateUsed != null) throw new SecureCookieAlreadyConsumedException("All the users cookies has been deleted");

            cookie.PasswordGuid = Guid.NewGuid();

            AuthenticationRepository.SecureCookieCreate(cookie.Guid, cookie.UserGuid, cookie.PasswordGuid, Request.Session.Guid);
            PortalRepository.SessionUpdate(Request.Session.Guid, cookie.UserGuid);

            return cookie;
        }
    }
}