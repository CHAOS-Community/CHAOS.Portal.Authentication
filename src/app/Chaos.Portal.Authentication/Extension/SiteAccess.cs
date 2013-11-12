namespace Chaos.Portal.Authentication.Extension
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    using Chaos.Portal.Authentication.Data;
    using Chaos.Portal.Authentication.Data.Model;
    using Chaos.Portal.Core;
    using Chaos.Portal.Core.Data.Model;
    using Chaos.Portal.Core.Exceptions;
    using Chaos.Portal.Core.Extension;

    public class SiteAccess : AExtension
    {
        #region Properties

        public IAuthenticationRepository AuthenticationRepository { get; set; }

        #endregion
        #region initialization

        public SiteAccess(IPortalApplication portalApplication, IAuthenticationRepository authenticationRepository): base(portalApplication)
        {
            AuthenticationRepository = authenticationRepository;
        }

        #endregion
        #region business logic

        public Session Auth(string apiKey)
        {
            var hashed  = ToHash(apiKey);
            var site    = AuthenticationRepository.SiteKeyGet(hashed);

            if (site == null) throw new InsufficientPermissionsException("ApiKey not valid");

            var session = PortalRepository.SessionCreate(site.UserGuid);

            return session;
        }

        public SiteKey Create(string name)
        {
            if (Request.IsAnonymousUser) throw new InsufficientPermissionsException("Anonymous users cannot create SiteKeys");

            var userGuid = Request.User.Guid;
            var hashed   = ToHash(string.Format("{0}{1}{2}", name, userGuid, DateTime.Now));

            var result = AuthenticationRepository.SiteKeyCreate(ToHash(hashed), userGuid, name);

            if(result == 0) throw new UnhandledException("Key wasn't added to the database");

            return new SiteKey
                {
                    Key      = hashed,
                    UserGuid = userGuid
                };
        }

        private string ToHash(string secret)
        {
            var byteHash = SHA256.Create().ComputeHash(Encoding.Unicode.GetBytes(secret));

            return BitConverter.ToString(byteHash).Replace("-", "").ToLower();
        }

        #endregion
    }
}