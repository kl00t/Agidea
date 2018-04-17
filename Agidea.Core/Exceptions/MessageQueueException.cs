using System;
using System.Runtime.Serialization;

namespace Agidea.Core.Exceptions
{
    public class MessageQueueException : Exception
    {
        public MessageQueueException()
        {
        }

        public MessageQueueException(string message) : base(message)
        {
        }

        public MessageQueueException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MessageQueueException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}