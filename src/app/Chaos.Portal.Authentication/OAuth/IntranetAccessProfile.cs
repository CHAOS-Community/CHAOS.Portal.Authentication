using DotNetAuth.Profiles;

namespace Chaos.Portal.Authentication.OAuth
{
	public class IntranetAccessProfile : Profile
	{
		 public bool HasIntranetAccess { get; set; }
	}
}