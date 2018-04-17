using System;
using System.Collections.Generic;
using System.Configuration;
using Agidea.Core.Interfaces;
using Agidea.Core.Models;

namespace Agidea.Mailer
{
    public class Mailer : IMailer
    {
        private static readonly string MailerQueueName = ConfigurationManager.AppSettings["MailerQueueName"];

        private readonly IMessageQueue _messageQueue;
        private readonly ISmtpEmailService _smtpEmailService;
        public Mailer(IMessageQueue messageQueue, ISmtpEmailService smtpEmailService)
        {
            _messageQueue = messageQueue ?? throw new ArgumentNullException(nameof(messageQueue));
            _smtpEmailService = smtpEmailService ?? throw new ArgumentNullException(nameof(smtpEmailService));
        }

        public bool DeleteFromQueue(List<Message> messages)
        {
            // Delete sent email from the message queue.
            throw new NotImplementedException();
        }

        public List<Message> ReadFromQueue()
        {
            var queueUrl = _messageQueue.GetQueueUrl(MailerQueueName);

            var messages = _messageQueue.ReceiveMessages(queueUrl);

            // TODO: Send Messages

            return null;
        }

        public void Send()
        {
            throw new NotImplementedException();
        }

        public void Send(List<Mail> mail)
        {
            throw new NotImplementedException();
        }
    }
}