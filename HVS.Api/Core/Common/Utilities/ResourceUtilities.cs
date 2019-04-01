using HVS.Api.Core.Common.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace HVS.Api.Core.Common.Utilities
{
    public static class ResourceUtilities
    {
        private const string ResourceNamespace = "HVS.Api.Core.Business.Resources";

        /// <summary>
        /// Get resource content
        /// </summary>
        /// <param name="resourceName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetResourceContent(string resourceName, ResourceType type = ResourceType.None)
        {
            if (type == ResourceType.None)
            {
                return EmbeddedResourceHelper.GetString(resourceName, ResourceNamespace);
            }
            return EmbeddedResourceHelper.GetString(string.Format("{0}.{1}", type.GetEnumName(), resourceName), ResourceNamespace);
        }

        /// <summary>
        /// Get module resource content
        /// </summary>
        /// <param name="resourceName"></param>
        /// <param name="moduleResourceNameSpace"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetModuleResourceContent(string resourceName, string moduleResourceNameSpace, ResourceType type = ResourceType.None)
        {
            if (type == ResourceType.None)
            {
                return EmbeddedResourceHelper.GetString(resourceName, ResourceNamespace);
            }
            return EmbeddedResourceHelper.GetString(string.Format("{0}.{1}", type.GetEnumName(), resourceName), moduleResourceNameSpace);
        }

        public enum ResourceType
        {
            [Description("EmailTemplates")]
            EmailTemplate = 1,
            None
        }
    }
}
