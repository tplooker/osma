using Autofac;
using System;

namespace Osma.Mobile.App.Services
{
    public class ServiceProvider : IServiceProvider
    {
        private IComponentContext _context;

        public ServiceProvider(IComponentContext context)
        {
            _context = context;
        }

        public object GetService(Type serviceType) => _context.Resolve(serviceType);
    }
}