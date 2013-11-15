using System.Collections.Generic;
using System.Linq;

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

    public class AuthKey : AExtension
    {
        #region Properties

        public IAuthenticationRepository AuthenticationRepository { get; set; }

        #endregion
        #region initialization

        public AuthKey(IPortalApplication portalApplication, IAuthenticationRepository authenticationRepository): base(portalApplication)
        {
            AuthenticationRepository = authenticationRepository;
        }

        #endregion
        #region business logic

        public Session Login(string token)
        {
            var hashed  = ToHash(token);
            var authKey = AuthenticationRepository.AuthKeyGet(null, hashed).SingleOrDefault();

            if (authKey == null) throw new InsufficientPermissionsException("Token not valid");

            var session = PortalRepository.SessionCreate(authKey.UserGuid);

            return session;
        }

        public Data.Model.AuthKey Create(string name)
        {
            if (Request.IsAnonymousUser) throw new InsufficientPermissionsException("Anonymous users cannot create AuthorizationKeys");

            var userGuid = Request.User.Guid;
            var hashed   = ToHash(string.Format("{0}{1}{2}", name, userGuid, DateTime.Now));

            var result = AuthenticationRepository.AuthKeyCreate(ToHash(hashed), userGuid, name);

            if(result == 0) throw new UnhandledException("AuthKey wasn't added to the database");

	        return new Data.Model.AuthKey(hashed, name, userGuid);
        }

	    public IList<Data.Model.AuthKey> Get()
	    {
		    var authKeys = AuthenticationRepository.AuthKeyGet(Request.User.Guid, null);

		    foreach (var authKey in authKeys)
			    authKey.Token = null; //Tokens should not be available again

		    return authKeys;
	    }

		public ScalarResult Delete(string name)
		{
			var result = AuthenticationRepository.AuthKeyDelete(Request.User.Guid, name);

			if(result != 1)
				throw new ArgumentException(string.Format("AuthKey \"{0}\" not found", name));

			return new ScalarResult(1);
		}

        private string ToHash(string secret)
        {
            var byteHash = SHA256.Create().ComputeHash(Encoding.Unicode.GetBytes(secret));

            return BitConverter.ToString(byteHash).Replace("-", "").ToLower();
        }

        #endregion
    }
}