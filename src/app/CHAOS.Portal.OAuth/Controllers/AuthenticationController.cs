using System;
using System.Configuration;
using System.Web.Mvc;
using CHAOS.Portal.Client.Extensions;
using CHAOS.Portal.Client.Standard;
using DotNetAuth.OAuth2;

namespace CHAOS.Portal.OAuth.Controllers
{
    public class AuthenticationController : Controller
    {
		public bool LoginSuccessful { get; private set; }

		private ApplicationCredentials _credentials;
		private OAuth2SessionStateManager _oauth2StateManager;
	    private ProviderConfiguration _providerConfiguration;
	    private PortalConfiguration _portalConfiguration;
	    private GenericProvider _provider;
	    private Guid _sessionGuid;

		protected override System.IAsyncResult BeginExecute(System.Web.Routing.RequestContext requestContext, System.AsyncCallback callback, object state)
		{
			if (requestContext.HttpContext.Request.QueryString["sessionGuid"] == null) throw new Exception("Missing sessiongGuid parameter");

			_oauth2StateManager = new OAuth2SessionStateManager(requestContext.HttpContext.Session);
			_providerConfiguration = (ProviderConfiguration)ConfigurationManager.GetSection("OAuthConfiguration/ProviderConfiguration");
			_portalConfiguration = (PortalConfiguration)ConfigurationManager.GetSection("OAuthConfiguration/PortalConfiguration");
			_sessionGuid = Guid.Parse(requestContext.HttpContext.Request.QueryString["sessionGuid"]);

			_credentials = new ApplicationCredentials
			{
				AppId = _providerConfiguration.ClientId,
				AppSecretId = _providerConfiguration.ClientSecret
			};
			_provider = new GenericProvider
			{
				AuthorizationEndpointUri = _providerConfiguration.AuthorizationEndpoint,
				TokenEndpointUri = _providerConfiguration.TokenEndpoint
			};

			return base.BeginExecute(requestContext, callback, state);
		}

        public ActionResult Login()
        {
			var userProcessUri = Uri.EscapeDataString(Url.Action("ProcessLoginResponse", "Authentication", routeValues: null, protocol: Request.Url.Scheme));
			var redirectUri = OAuth2Process.GetAuthenticationUri(_provider, _credentials, userProcessUri, "", _oauth2StateManager);

			return Redirect(redirectUri.ToString());
        }

		public ActionResult ProcessLoginResponse()
		{
			var userProcessUri = Url.Action("ProcessLoginResponse", "Authentication", routeValues: null, protocol: Request.Url.Scheme);
			var response = OAuth2Process.ProcessUserResponse(_provider, _credentials, Request.Url, userProcessUri, _oauth2StateManager);
			response.Wait();

			LoginSuccessful = response.Result.Succeed;

			if (LoginSuccessful)
			{
				var oAuthId = "";
				var email = "";

				var client = new PortalClient();
				client.Session().Create().Synchronous().ThrowError(); ;
				client.AuthKey().Login(_portalConfiguration.AuthKeyToken).Synchronous().ThrowError();
				client.OAuth().Login(oAuthId, email, _sessionGuid).Synchronous().ThrowError();
			}

			return View();
		}
    }
}
