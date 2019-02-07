using System;
using Microsoft.Extensions.DependencyInjection;

namespace Willezone.Azure.WebJobs.Extensions.DependencyInjection
{
    /// <summary>
    /// Defines the interface of builder that creates an instance of an <see cref="IServiceProvider"/>.
    /// </summary>
    public interface IServiceProviderBuilder
    {
        /// <summary>
        /// Creates an instance of an <see cref="IServiceProvider"/>.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        IServiceProvider Build(ServiceCollection serviceCollection);
    }
}
