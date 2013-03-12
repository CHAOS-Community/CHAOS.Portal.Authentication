namespace Chaos.Portal.Authentication.Exception
{
    using System.Runtime.Serialization;

    public class SecureCookieAlreadyConsumedException : System.Exception
    {
        public SecureCookieAlreadyConsumedException()
        {
        }

        public SecureCookieAlreadyConsumedException(string message) : base(message)
        {
        }

        public SecureCookieAlreadyConsumedException(string message, System.Exception innerException) : base(message, innerException)
        {
        }

        protected SecureCookieAlreadyConsumedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}