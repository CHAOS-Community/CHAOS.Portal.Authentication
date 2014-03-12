using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;
using CHAOS.Portal.Client.Extensions;
using CHAOS.Portal.Client.Standard;
using CHAOS.Portal.OAuth.Models;
using DotNetAuth.OAuth2;
using DotNetAuth.Profiles;

namespace CHAOS.Portal.OAuth.Controllers
{
    public class AuthenticationController : Controller
    {
		private ProviderConfiguration _providerConfiguration;
		private PortalConfiguration _portalConfiguration;
		
		private LoginProvider _provider;
		private DefaultLoginStateManager _stateManager;
	    private ProfileProperty[] _requiredProperties;

		protected override IAsyncResult BeginExecute(System.Web.Routing.RequestContext requestContext, System.AsyncCallback callback, object state)
		{
			_providerConfiguration = (ProviderConfiguration)ConfigurationManager.GetSection("OAuthConfiguration/ProviderConfiguration");
			_portalConfiguration = (PortalConfiguration)ConfigurationManager.GetSection("OAuthConfiguration/PortalConfiguration");

			_provider = new LoginProvider
			{
				AppId = _providerConfiguration.ClientId,
				AppSecret = _providerConfiguration.ClientSecret,
				Definition = new GenericLoginProviderDefinition(new GenericProvider
				{
					AuthorizationEndpointUri = _providerConfiguration.AuthorizationEndpoint,
					TokenEndpointUri = _providerConfiguration.TokenEndpoint,
					UserInfoEndpoint = _providerConfiguration.UserInfoEndpoint
				})
			};

			_stateManager = new DefaultLoginStateManager(requestContext.HttpContext.Session);
			_requiredProperties = new[] { ProfileProperty.Email, ProfileProperty.DisplayName, ProfileProperty.UniqueID };

			return base.BeginExecute(requestContext, callback, state);
		}

        public ActionResult Login()
        {
			if (Request.QueryString["sessionGuid"] == null) throw new Exception("Missing sessiongGuid parameter");

	        Session["sessionGuid"] = Request.QueryString["sessionGuid"];

			var userProcessUri = Url.Action("ProcessLoginResponse", "Authentication", null,  Request.Url.Scheme);
			var authorizationUrl = GetAuthenticationUri(_provider, userProcessUri, _stateManager, _requiredProperties);
	        authorizationUrl.Wait();

			return Redirect(authorizationUrl.Result.AbsoluteUri);
        }

		public ActionResult ProcessLoginResponse()
		{
			if (Session["sessionGuid"] == null) throw new Exception("Missing sessiongGuid session variable");
			var sessionGuid = Guid.Parse(Session["sessionGuid"].ToString());
			
			var userProcessUri = Url.Action("ProcessLoginResponse", "Authentication", null, Request.Url.Scheme);

			var response = DotNetAuth.Profiles.Login.GetProfile(_provider, Request.Url, userProcessUri, new DefaultLoginStateManager(Session), _requiredProperties);
			response.Wait();

			var result = new ProcessLoginResponseModel {LoginSuccessful = !response.IsFaulted && response.Result.UniqueID != null};

			if (result.LoginSuccessful)
			{
				var client = new PortalClient {ServicePath = _portalConfiguration.ServicePath};
				client.Session().Create().Synchronous().ThrowError(); ;
				client.AuthKey().Login(_portalConfiguration.AuthKeyToken).Synchronous().ThrowError();
				client.OAuth().Login(response.Result.UniqueID, response.Result.Email, sessionGuid).Synchronous().ThrowError();
			}

			return View(result);
		}

		private static Task<Uri> GetAuthenticationUri(LoginProvider provider, string loginProcessUri, ILoginStateManager stateManager, ProfileProperty[] requiredProperties)
		{
			var generalLoginStataManager = new LoginStateManager(stateManager);
			switch (provider.Definition.ProtocolType)
			{
				case ProtocolTypes.OAuth2:
					var scope = provider.Definition.GetRequiredScope(requiredProperties);
					return Task.Factory.StartNew(() => OAuth2Process.GetAuthenticationUri(provider.Definition.GetOAuth2Definition(), provider.GetOAuth2Credentials(), Uri.EscapeDataString(loginProcessUri), scope, generalLoginStataManager));
				default:
					throw new Exception("Invalid provider. Provider's protocol type is not set or supported.");
			}
		}
    }
}
