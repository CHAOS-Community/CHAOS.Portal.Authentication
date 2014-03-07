using System.Web;

namespace CHAOS.Portal.OAuth.Controllers
{
	public class OAuth2SessionStateManager : DotNetAuth.OAuth2.IOAuth20StateManager
	{
		private readonly HttpSessionStateBase _session;

		public OAuth2SessionStateManager(HttpSessionStateBase session)
		{
			_session = session;
		}
		public string GetState()
		{
			_session["tempStateCode"] = System.Guid.NewGuid().ToString();
			return _session["tempStateCode"] as string;
		}
		public bool CheckState(string stateCode)
		{
			var expectedValue = _session["tempStateCode"] as string;
			return expectedValue == stateCode;
		}
	}
}