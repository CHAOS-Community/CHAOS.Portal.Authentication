namespace Chaos.Portal.Authentication.Exception
{
	public class WayfUserNotAllowedException : LoginException
	{
		public WayfUserNotAllowedException(string attributes) : base(string.Format("The given Wayf user is not authorized to login. Wayf attributes: {0}", attributes))
		{
			
		}
	}
}