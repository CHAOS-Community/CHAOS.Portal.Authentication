namespace CHAOS.Portal.Authentication.EmailPassword.Data
{
    public interface IEmailPasswordRepository
    {
        IEmailPasswordRepository WithConnectionString(string connectionString);
    }
}