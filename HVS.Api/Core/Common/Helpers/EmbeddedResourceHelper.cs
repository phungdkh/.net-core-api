using HVS.Api.Core.Common.Reflections;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace HVS.Api.Core.Common.Helpers
{
    public class EmbeddedResourceHelper
    {
        /// <summary>
        /// Get embedded resource by short name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>The resource as stream if there is only 1 resource whose fullname ends with provided name. Null if no match. Throws Argument Exception if there are more than 1 matches</returns>
        public static Stream GetStream(string name)
        {
            var assemblies = ReflectionUtilities.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                //Skip dynamic assemblies
                if (assembly.IsDynamic)
                {
                    continue;
                }
                var resourceNames = assembly.GetManifestResourceNames().Where(r => r.Equals(name)).ToList();
                if (resourceNames.Count > 1)
                {
                    throw new ArgumentException(string.Format("Ambigous resource name: {0}", resourceNames.First()));
                }
                if (resourceNames.Any())
                {
                    return assembly.GetManifestResourceStream(resourceNames.First());
                }
            }
            throw new ArgumentException(string.Format("Missing resource name: {0}", name));
        }

        /// <summary>
        /// Get embedded resource by short name.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="namespace"></param>
        /// <param name="assembly">The assembly to look for resource, the calling assembly will be use if null is passed</param>
        /// <returns>The resource as string if there is only 1 resource whose fullname ends with provided name. Null if no match. Throws Argument Exception if there are more than 1 matches</returns>
        public static string GetString(string name, string @namespace, Assembly assembly = null)
        {
            if (!string.IsNullOrEmpty(@namespace))
            {
                name = string.Format("{0}.{1}", @namespace, name);
            }
            var stream = GetStream(name);
            using (var reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();
                return result;
            }
        }

        /// <summary>
        /// Get embedded resource by short name.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="namespace"></param>
        /// <param name="assembly">The assembly to look for resource, the calling assembly will be use if null is passed</param>
        /// <returns>The resource as string if there is only 1 resource whose fullname ends with provided name. Null if no match. Throws Argument Exception if there are more than 1 matches</returns>
        public static string GetNullableString(string name, string @namespace, Assembly assembly = null)
        {
            try
            {
                return GetString(name, @namespace, assembly);
            }
            catch (Exception)
            {
                //If cannot find the resource then return empty string
                return string.Empty;
            }
        }
    }
}
