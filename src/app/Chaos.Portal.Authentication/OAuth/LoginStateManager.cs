using DotNetAuth.Profiles;

namespace Chaos.Portal.Authentication.OAuth
{
	public class LoginStateManager : ILoginStateManager
	{
		public string StateCode { get; set; }

		public void SaveTemp(string stateCode)
		{
			StateCode = stateCode;
		}

		public string LoadTemp()
		{
			return StateCode;
		}

		public void ClearTemp()
		{
			StateCode = null;
		}
	}
}