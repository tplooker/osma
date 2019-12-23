using Autofac;
using Osma.Mobile.App.Services;

namespace Osma.Mobile.App.Droid
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