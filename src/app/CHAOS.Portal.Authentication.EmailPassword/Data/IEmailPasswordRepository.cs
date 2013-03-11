namespace CHAOS.Portal.Authentication.EmailPassword.Data
{
    using System;

    public interface IEmailPasswordRepository
    {
        IEmailPasswordRepository WithConnectionString(string connectionString);

        Dto.EmailPassword EmailPasswordGet(Guid guid, string password);
    }
}