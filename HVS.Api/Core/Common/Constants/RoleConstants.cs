using System;

namespace HVS.Api.Core.Common.Constants
{
    /// <summary>
    /// 
    /// </summary>
    public static class RoleConstants
    {
        /// <summary>
        /// Super Admin
        /// </summary>
        public const string SA = "075c1072-92a2-4f99-ac11-bf985b23da6e";
        public static Guid SaId = new Guid(SA);

        public const string AllRole = SA;
    }
}
