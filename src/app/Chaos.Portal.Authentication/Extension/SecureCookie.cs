namespace Chaos.Portal.Authentication.Extension
{
    using System.Collections.Generic;

    using Chaos.Portal.Authentication.Data;
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
            return AuthenticationRepository.SecureCookieGet(callContext.User.Guid, null, null);
        }

        #endregion
    }
}