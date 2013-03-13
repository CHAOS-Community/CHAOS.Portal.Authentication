namespace Chaos.Portal.Authentication.Data.Dto
{
    using System;

    using CHAOS.Serialization;

    using Chaos.Portal.Data.Dto;

    public class SecureCookie : AResult
    {
        [Serialize]
        public Guid Guid { get; set; }

        public Guid UserGuid { get; set; }

        [Serialize]
        public Guid PasswordGuid { get; set; }
        
        public DateTime? DateUsed { get; set; }

        public DateTime DateCreated { get; set; }
    }
}