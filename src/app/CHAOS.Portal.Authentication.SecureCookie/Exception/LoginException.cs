using System.Runtime.Serialization;

namespace CHAOS.Portal.Authentication.SecureCookie.Exception
{
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