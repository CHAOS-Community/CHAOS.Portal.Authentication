﻿namespace Chaos.Portal.Authentication.Data
{
    using System;
    using System.Collections.Generic;

    using Chaos.Portal.Authentication.Data.Dto;

    public interface IAuthenticationRepository
    {
        IEnumerable<SecureCookie> SecureCookieGet(Guid? guid, Guid? userGuid, Guid? passwordGuid);
        uint SecureCookieCreate(Guid guid, Guid userGuid, Guid passwordGuid, Guid sessionGuid);
        uint SecureCookieDelete(Guid whereGuid, Guid whereUserGuid);
        uint SecureCookieUse(Guid? guid, Guid? userGuid, Guid? passwordGuid);

        EmailPassword EmailPasswordGet(Guid guid, string password);
    }
}