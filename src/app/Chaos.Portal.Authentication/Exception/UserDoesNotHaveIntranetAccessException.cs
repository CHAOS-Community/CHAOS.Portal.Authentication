using System.Runtime.Serialization;

namespace Chaos.Portal.Authentication.Exception
{
    public class UserDoesNotHaveIntranetAccessException : System.Exception
    {
        public UserDoesNotHaveIntranetAccessException()
        {
        }

        public UserDoesNotHaveIntranetAccessException(string message) : base(message)
        {
        }

        public UserDoesNotHaveIntranetAccessException(string message, System.Exception innerException) : base(message, innerException)
        {
        }

        protected UserDoesNotHaveIntranetAccessException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}