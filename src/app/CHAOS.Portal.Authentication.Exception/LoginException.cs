using System.Runtime.Serialization;

namespace CHAOS.Portal.Authentication.Exception
{
    public class LoginException : System.Exception
    {
        public LoginException()
        {
        }

        public LoginException(string message) : base(message)
        {
        }

        public LoginException(string message, System.Exception innerException) : base(message, innerException)
        {
        }

        protected LoginException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}