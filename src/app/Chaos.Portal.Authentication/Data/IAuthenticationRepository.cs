namespace Chaos.Portal.Authentication.Data
{
    using System;
    using System.Collections.Generic;
    using Dto;
    using Dto.v6;
    using Model;
    
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
		uint AuthKeyDelete(Guid userGuid, string name);

		WayfUser WayfProfileGet(string wayfId);
		uint WayfProfileUpdate(Guid userGuid, string wayfId);
        
        FacebookUser FacebookUserGet(ulong facebookId);
        uint FacebookUserCreate(ulong facebookUserId, Guid userGuid);
    }
}