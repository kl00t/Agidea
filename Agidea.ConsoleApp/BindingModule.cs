using Agidea.Core.Interfaces;
using Agidea.Repository;
using Agidea.Storage;
using Ninject.Modules;

namespace Agidea.ConsoleApp
{
    public class BindingModule : NinjectModule
    {
        public override void Load() 
        {
            Bind<IMessageQueue>().To<MessageQueue.MessageQueue>();
            Bind<IMailer>().To<Mailer.Mailer>();
            Bind<IEmailRepository>().To<EmailRepository>();
            Bind<IFileStorageProvider>().To<AmazonS3Provider>();

            Bind<AutoMapper.IMapper>().ToMethod(ctx => AutoMapperConfig.InitializeAutoMapper()).InSingletonScope();
        }
    }
}
