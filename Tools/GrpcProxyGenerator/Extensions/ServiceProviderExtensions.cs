using System;

namespace GrpcProxyGenerator.Extensions
{
    public static class ServiceProviderExtensions
    {
        public static T GetServiceOrFail<T>(this IServiceProvider serviceProvider)
            where T : class
        {
            var service = serviceProvider.GetService(typeof(T));

            if (service == null)
            {
                throw new InvalidOperationException($"Couldn't find service {typeof(T)}");
            }

            var serviceTyped = service as T;

            return serviceTyped!;
        }
    }
}