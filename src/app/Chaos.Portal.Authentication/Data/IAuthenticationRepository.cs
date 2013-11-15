namespace Chaos.Portal.Authentication.Data
{
    using System;
    using System.Collections.Generic;
    using Chaos.Portal.Authentication.Data.Dto;
    using Chaos.Portal.Authentication.Data.Model;
    
    public interface IAuthenticationRepository
    {
        IEnumerable<SecureCookie> SecureCookieGet(Guid? guid, Guid? userGuid, Guid? passwordGuid);
        uint SecureCookieCreate(Guid guid, Guid userGuid, Guid passwordGuid, Guid sessionGuid);
        uint SecureCookieDelete(Guid whereGuid, Guid whereUserGuid);
        uint SecureCookieUse(Guid? guid, Guid? userGuid, Guid? passwordGuid);

        EmailPassword EmailPasswordGet(Guid guid, string password);
		uint EmailPasswordUpdate(Guid userGuid, string password);

		uint AuthKeyCreate(string token, Guid userGuid, string name);
		IList<AuthKey> AuthKeyGet(Guid? userGuid, string token);
		uint AuthKeyDelete(string token);

		WayfUser WayfProfileGet(string wayfId);
		uint WayfProfileUpdate(Guid userGuid, string wayfId);
    }
}