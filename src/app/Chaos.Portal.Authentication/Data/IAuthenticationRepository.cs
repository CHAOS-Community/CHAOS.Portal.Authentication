namespace Chaos.Portal.Authentication.Data
{
    using System;
    using System.Collections.Generic;

    using Chaos.Portal.Authentication.Data.Dto;

    public interface IAuthenticationRepository
    {
        IEnumerable<SecureCookie> SecureCookieGet(Guid? userGuid, Guid? guid, Guid? passwordGuid);
        uint SecureCookieCreate(Guid userGuid, Guid guid, Guid passwordGuid, Guid sessionGuid);
        uint SecureCookieDelete(Guid whereUserGuid, Guid guid);
        uint SecureCookieUse(Guid? userGuid, Guid? guid, Guid? passwordGuid);
    }
}