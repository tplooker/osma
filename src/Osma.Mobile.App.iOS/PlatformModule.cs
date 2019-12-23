using Autofac;
using Hyperledger.Aries.Configuration;
using Microsoft.Extensions.Options;
using Osma.Mobile.App.Services;
using Osma.Mobile.App.Services.Interfaces;

namespace Osma.Mobile.App.iOS
{
    public class PlatformModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterModule(new CoreModule());
            builder.RegisterModule(new ServicesModule());

            builder.Register(container =>
            {
                var service = container.Resolve<IKeyValueStoreService>();
                return Options.Create(service.GetData<AgentOptions>("AgentOptions"));
            });
        }
    }
}