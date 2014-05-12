using DotNetAuth.Profiles;

namespace Chaos.Portal.Authentication.OAuth
{
	public interface IOAuthClient
	{
		LoginEndPoint GetLoginEndPoint(string callbackUrl);
		Profile ProcessLogin(string callbackUrl, string responseUrl, string stateCode);
	}
}