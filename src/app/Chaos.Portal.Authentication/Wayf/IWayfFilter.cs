using System.Collections.Generic;

namespace Chaos.Portal.Authentication.Wayf
{
	public interface IWayfFilter
	{
		bool Validate(IDictionary<string, IList<string>> attributes);
	}
}