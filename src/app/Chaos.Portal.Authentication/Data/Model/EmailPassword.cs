namespace Chaos.Portal.Authentication.Data.Model
{
    using System;

    public class EmailPassword
    {
        public Guid UserGuid { get; set; }

        public string Password { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateModified { get; set; }
    }
}