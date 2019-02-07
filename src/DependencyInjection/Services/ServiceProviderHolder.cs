using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Azure.WebJobs.Host.Bindings;

namespace Willezone.Azure.WebJobs.Extensions.DependencyInjection
{
    internal class ServiceProviderHolder
    {
        private readonly ConcurrentDictionary<Guid, IServiceScope> _scopes = new ConcurrentDictionary<Guid, IServiceScope>();
        private readonly IServiceProvider _serviceProvider;

        public ServiceProviderHolder(IServiceProvider serviceProvider) =>
            _serviceProvider = serviceProvider ?? throw new InvalidOperationException("No service provider provided!");

        public void RemoveScope(Guid functionInstanceId)
        {
            if (_scopes.TryRemove(functionInstanceId, out var scope))
            {
                scope.Dispose();
            }
        }

        public object GetRequiredService(Guid functionInstanceId, Type serviceType, BindingContext context)
        {
            var scopeFactory = _serviceProvider.GetService<IServiceScopeFactory>();
            if (scopeFactory == null)
            {
                throw new InvalidOperationException("The current service provider does not support scoping!");
            }

            var scope = _scopes.GetOrAdd(functionInstanceId, (_) => CreateScope(scopeFactory, context));
            return scope.ServiceProvider.GetRequiredService(serviceType);
        }

        private static IServiceScope CreateScope(IServiceScopeFactory scopeFactory, BindingContext context)
        {
            var scope = scopeFactory.CreateScope();

            if (!context.BindingData.TryGetValue("UserProperties", out var properties))
                return scope;

            if (!(properties is Dictionary<string, object> propertiesDict))
                return scope;

            var messagePropertiesProvider = scope.ServiceProvider.GetService<IMessagePropertiesProvider>() as MessagePropertiesProvider;

            if (messagePropertiesProvider == null)
                return scope;

            messagePropertiesProvider.SetProperties(propertiesDict);

            return scope;
        }
    }
}
