using System;
using Chaos.Portal.Authentication.Data.Model;

namespace Chaos.Portal.Authentication.Data
{
	public interface IOAuthRepository
	{
		OAuthUser OAuthUserGet(string oAuthId);
		void OAuthUserUpdate(Guid userGuid, string oAuthId); 
	}
}