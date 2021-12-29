using System;

namespace Common.Extensions.CustomExceptions
{
    [Serializable]
    public class SmtpException : Exception
    {
        public SmtpException()
        {

        }
        public SmtpException(string message) : base(message)
        {

        }
        public SmtpException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
