namespace Chaos.Portal.Authentication.Data.Model
{
    using System;

    using CHAOS.Serialization;

    using Chaos.Portal.Core.Data.Model;

    public class SiteKey : AResult
    {
        [Serialize]
        public string Key { get; set; }

        public Guid UserGuid{ get; set; }
    }
}