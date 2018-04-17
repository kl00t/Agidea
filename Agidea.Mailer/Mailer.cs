using System;
using System.Collections.Generic;
using Agidea.Core.Interfaces;
using Agidea.Core.Models;

namespace Agidea.Mailer
{
    public class Mailer : IMailer
    {
        private readonly IMessageQueue _messageQueue;

        public Mailer(IMessageQueue messageQueue)
        {
            _messageQueue = messageQueue ?? throw new ArgumentNullException(nameof(messageQueue));
        }

        public bool DeleteFromQueue(List<Message> messages)
        {
            // Delete sent email from the message queue.
            throw new NotImplementedException();
        }

        public List<Message> ReadFromQueue()
        {
            var messages = _messageQueue.ReceiveMessages();

            // TODO: Send Messages

            return null;
        }
    }
}