using System;
using System.Configuration;
using Agidea.Core.Interfaces;
using Agidea.Mailer;
using Agidea.Repository;
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

            Bind<AutoMapper.IMapper>().ToMethod(ctx => AutoMapperConfig.InitializeAutoMapper()).InSingletonScope();
        }
    }
}
