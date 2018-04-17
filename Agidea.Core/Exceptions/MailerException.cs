using System;
using System.Runtime.Serialization;

namespace Agidea.Core.Exceptions
{
    public class MailerException : Exception
    {
        public MailerException()
        {
        }

        public MailerException(string message) : base(message)
        {
        }

        public MailerException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MailerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}