using Agidea.Core.Interfaces;
using Ninject.Modules;

namespace Agidea.ConsoleApp
{
    public class MessageQueueModule : NinjectModule
    {
        public override void Load() 
        {
            Bind<IMessageQueue>().To<MessageQueue.MessageQueue>();
            Bind<IMailer>().To<Mailer.Mailer>();
        }
    }
}
