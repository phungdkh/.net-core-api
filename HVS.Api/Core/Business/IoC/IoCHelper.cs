using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace HVS.Api.Core.Business.IoC
{
    public class IoCHelper
    {
        private static ServiceProvider _serviceProvider;

        /// <summary>
        /// Set the service provider
        /// </summary>
        /// <param name="serviceProvider"></param>
        public static void SetServiceProvider(ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Get instance
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetInstance<T>() where T : class
        {
            return _serviceProvider.GetService<T>();
        }

        /// <summary>
        /// Get all instances
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> GetInstances<T>() where T : class
        {
            return _serviceProvider.GetServices<T>();
        }
    }
}
