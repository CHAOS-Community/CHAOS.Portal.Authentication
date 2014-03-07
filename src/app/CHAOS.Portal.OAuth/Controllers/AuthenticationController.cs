using System.Configuration;
using System.Web.Mvc;
using DotNetAuth.OAuth2;

namespace CHAOS.Portal.OAuth.Controllers
{
    public class AuthenticationController : Controller
    {
		private ApplicationCredentials _credentials;
		private OAuth2SessionStateManager _oauth2StateManager;
	    private ProviderConfiguration _providerConfiguration;
	    private GenericProvider _provider;

		protected override System.IAsyncResult BeginExecute(System.Web.Routing.RequestContext requestContext, System.AsyncCallback callback, object state)
		{
			_oauth2StateManager = new OAuth2SessionStateManager(requestContext.HttpContext.Session);
			_providerConfiguration = (ProviderConfiguration)ConfigurationManager.GetSection("ProviderConfigurationGroup/ProviderConfiguration");
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
			var userProcessUri = Url.Action("ProcessLoginResponse", "Authentication", routeValues: null, protocol: Request.Url.Scheme);
			var redirectUri = OAuth2Process.GetAuthenticationUri(_provider, _credentials, userProcessUri, "", _oauth2StateManager);
			return Redirect(redirectUri.ToString());
        }

		public ActionResult ProcessLoginResponse()
		{
			var userProcessUri = Url.Action("ProcessLoginResponse", "Authentication", routeValues: null, protocol: Request.Url.Scheme);
			var response = OAuth2Process.ProcessUserResponse(_provider, _credentials, Request.Url, userProcessUri, _oauth2StateManager);
			response.Wait();
			Session["result"] = response.Result;
			return View();
		}
    }
}
