using Ninject.Modules;

namespace Amazon.SQS.MessageQueue
{
    public class MessageQueueModule : NinjectModule
    {
        public override void Load() 
        {
            Bind<IMessageQueue>().To<MessageQueue>();
        }
    }
}
