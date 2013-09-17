using System;
using Chaos.Portal.Core;
using Chaos.Portal.Core.Data.Model;
using Chaos.Portal.Core.Extension;

namespace Chaos.Portal.Authentication.Extension
{
	public class Wayf : AExtension
	{
		public Wayf(IPortalApplication portalApplication) : base(portalApplication)
		{
		}

		public ScalarResult Login(string wayfId)
		{
			throw new NotImplementedException();

			return new ScalarResult(1);
		}
	}
}