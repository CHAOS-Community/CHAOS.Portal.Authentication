using System;

namespace Chaos.Portal.Authentication.Data
{
	public class UserInfoUpdate<T>
	{
		public Guid UserGuid { get; private set; }
		public T UserInfo { get; private set; }

		public UserInfoUpdate(Guid userGuid, T userInfo)
		{
			UserGuid = userGuid;
			UserInfo = userInfo;
		}
	}
}