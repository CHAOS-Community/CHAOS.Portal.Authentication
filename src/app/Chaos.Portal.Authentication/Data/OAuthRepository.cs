using System;
using System.Linq;
using CHAOS.Data;
using CHAOS.Data.MySql;
using Chaos.Portal.Authentication.Data.Mapping;
using Chaos.Portal.Authentication.Data.Model;
using MySql.Data.MySqlClient;

namespace Chaos.Portal.Authentication.Data
{
	class OAuthRepository : IOAuthRepository
	{
		private readonly Gateway _gateway;

		static OAuthRepository()
		{
			ReaderExtensions.Mappings.Add(typeof(OAuthUser), new OAuthUserMapping());
		}

		public OAuthRepository(Gateway gateway)
		{
			_gateway = gateway;
		}

		public OAuthUser OAuthUserGet(string oAuthId)
		{
			var results = _gateway.ExecuteQuery<OAuthUser>("OAuthUsers_Get", new[]
                {
                    new MySqlParameter("OAuthId", oAuthId)
                });

			return results.FirstOrDefault();
		}

		public void OAuthUserUpdate(Guid userGuid, string oAuthId)
		{
			_gateway.ExecuteQuery<OAuthUser>("OAuthUsers_Get", new[]
                {
					new MySqlParameter("UserGuid", userGuid.ToByteArray()),
                    new MySqlParameter("OAuthId", oAuthId)
                });
		}
	}
}