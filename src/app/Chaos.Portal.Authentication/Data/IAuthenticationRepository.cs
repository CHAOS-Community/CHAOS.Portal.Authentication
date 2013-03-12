namespace Chaos.Portal.Authentication.Data
{
    using System;
    using System.Collections.Generic;

    using Chaos.Portal.Authentication.Data.Dto;

    public interface IAuthenticationRepository
    {
        IEnumerable<SecureCookie> SecureCookieGet(Guid userGuid, Guid? secureCookieGuid, Guid? passwordGuid);

        uint SecureCookieDelete(Guid whereUserGuid, Guid whereSecureCookie);
    }
}