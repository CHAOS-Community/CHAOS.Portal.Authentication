namespace Chaos.Portal.Authentication.Data.Dto
{
    using System;

    using CHAOS.Serialization;

    public class SecureCookie
    {
        [Serialize]
        public Guid Guid { get; set; }

        public Guid UserGuid { get; set; }

        [Serialize]
        public Guid PasswordGuid { get; set; }
        
        public DateTime? DateUsed { get; set; }
    }
}