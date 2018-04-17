using System.Collections.Generic;
using Agidea.Core.Models;

namespace Agidea.Core.Interfaces
{
    public interface IMessageQueue
    {
        string GetQueueUrl(string queueName);

        bool SendMessages(List<Mail> emails);

        bool DeleteMessages(List<Message> messages);

        List<Message> ReceiveMessages();
    }
}
