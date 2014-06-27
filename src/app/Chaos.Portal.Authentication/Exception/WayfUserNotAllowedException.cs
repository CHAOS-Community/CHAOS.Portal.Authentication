namespace Chaos.Portal.Authentication.Exception
{
	public class WayfUserNotAllowedException : LoginException
	{
		public WayfUserNotAllowedException() : base("The given Wayf user is not authorized to login")
		{
			
		}
	}
}