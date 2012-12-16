namespace CHAOS.Portal.Authentication.EmailPassword.Data
{
    public class EmailPasswordRepository : IEmailPasswordRepository
    {
        #region Fields

        private string _connectionString;

        #endregion
        #region Construction

        public IEmailPasswordRepository WithConnectionString(string connectionString)
        {
            _connectionString = connectionString;

            return this;
        }

        protected EmailPasswordEntities CreateEmailPasswordEntities()
        {
            return new EmailPasswordEntities(_connectionString);
        }

        #endregion
    }
}