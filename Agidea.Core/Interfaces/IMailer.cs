using System.Collections.Generic;
using Agidea.Core.Models;

namespace Agidea.Core.Interfaces
{
    public interface IMailer
    {
        List<Message> ReadFromQueue();

        bool DeleteFromQueue(List<Message> messages);

        void Send();

        void Send(List<Mail> mail);
    }
}