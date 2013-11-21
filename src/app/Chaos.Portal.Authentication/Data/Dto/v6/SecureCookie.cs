namespace Chaos.Portal.Authentication.Data.Dto.v6
{
    using System;
    using CHAOS.Serialization;
    using Core.Data.Model;

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