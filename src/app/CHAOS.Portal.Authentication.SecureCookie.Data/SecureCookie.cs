using System;
using CHAOS.Serialization;
using Chaos.Portal.Data.Dto;

namespace CHAOS.Portal.Authentication.SecureCookie.Data
{
    public partial class SecureCookie : IResult
	{
		#region Properties

		public string Fullname
		{
			get { return GetType().FullName; }
		}

		[Serialize("GUID")]
		public UUID pSecureCookieGUID
		{
			get { return new UUID( SecureCookieGUID.ToByteArray() ); }
			set { SecureCookieGUID = new Guid( value.ToByteArray() ); }
		}

		[Serialize("PasswordGUID")]
		public UUID pPasswordGUID
		{
			get { return new UUID( PasswordGUID.ToByteArray() ); }
			set 
			{ 
				if( value != null )
					PasswordGUID = new Guid( value.ToByteArray() );
			}
		}

		[Serialize("UserGUID")]
        public UUID pUserGUID
		{
			get { return new UUID( UserGUID.ToByteArray() ); }
			set { UserGUID = new Guid( value.ToByteArray() ); }
		}

		[Serialize("DateCreated")]
		public DateTime pDateCreated
		{
			get { return DateCreated; }
			set { DateCreated = value; }
		}

		[Serialize("DateUsed")]
		public DateTime? pDateUsed
		{
			get { return DateUsed; }
			set { DateUsed = value; }
		}

		#endregion
	}
}
