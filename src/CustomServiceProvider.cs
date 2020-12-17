using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotChocolate.Fetching;
using SimpleInjector;

namespace CyclicalDependency
{
    public class CustomServiceProvider : IServiceProvider
    {
        private Container Container { get; }
        private IServiceProvider FallbackServiceProvider { get; }

        private static IEnumerable<Type> FallbackList => new[]
        {
            typeof(IBatchDispatcher)
        };

        private static bool IsFallbackType(Type serviceType) => FallbackList.Any(serviceType.IsAssignableFrom);

        public CustomServiceProvider(
            Container container,
            IServiceProvider fallbackServiceProvider)
        {
            Container = container;
            FallbackServiceProvider = fallbackServiceProvider;
        }

        public object GetService(Type serviceType) =>
            (IsFallbackType(serviceType)
                ? FallbackServiceProvider
                : Container).GetService(serviceType);
    }
}
