using System.Collections.Generic;
using Agidea.Core.Models;

namespace Agidea.Core.Interfaces
{
    public interface IMessageQueue
    {
        string CreateQueue(string queueName);

        bool DeleteQueue(string queueUrl);

        string GetQueueUrl(string queueName);

        bool SendMessages(string queueUrl, List<Mail> emails);

        bool DeleteMessages(string queueUrl, List<Message> messages);

        List<Message> ReceiveMessages(string queueUrl);

        List<string> ListQueues();
    }
}
