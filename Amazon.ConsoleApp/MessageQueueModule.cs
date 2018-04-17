using Amazon.SES.Mailer;
using Amazon.SQS.MessageQueue;
using Ninject.Modules;

namespace Amazon.ConsoleApp
{
    public class MessageQueueModule : NinjectModule
    {
        public override void Load() 
        {
            Bind<IMessageQueue>().To<MessageQueue>();
            Bind<IMailer>().To<Mailer>();
        }
    }
}
