using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HVS.Api.Core.Common.Reflections
{
    /// <summary>
    /// The Reflection Class
    /// </summary>
    public static class ReflectionUtilities
    {
        /// <summary>
        /// Get all properties from type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<PropertyInfo> GetAllPropertiesOfType(Type type)
        {
            return type.GetProperties().ToList();
        }

        /// <summary>
        /// Get all property name from type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<string> GetAllPropertyNamesOfType(Type type)
        {
            var properties = type.GetProperties();
            return properties.Select(p => p.Name).ToList();
        }

        /// <summary>
        /// Get  assemblies
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Assembly> GetAssemblies()
        {
            var currentAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            return currentAssemblies.Where(a => a.FullName.Contains("HVS.Api"));
        }
    }
}
