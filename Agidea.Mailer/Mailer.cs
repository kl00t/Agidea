using System.Collections.Generic;
using Agidea.Core.Interfaces;

namespace Agidea.Mailer
{
    public class Mailer : IMailer
    {

        public Mailer(IMessageQueue messageQueue)
        {
            
        }

        public List<string> ReadFromQueue()
        {
            return null;
        }
    }
}