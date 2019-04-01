using System;
using System.ComponentModel;
using System.Reflection;

namespace HVS.Api.Core.Common.Helpers
{
    /// <summary>
    /// Enum helper methods
    /// </summary>
    public static class EnumHelpers
    {
        /// <summary>
        /// Get Enum description extension
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetEnumDescription(this Enum value)
        {
            DescriptionAttribute[] attributes = null;
            if (value.ToString()!=null)
            {
                try
                {
                    FieldInfo fi = value.GetType().GetField(value.ToString());
                    attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                }
                catch (NullReferenceException e)
                {

                    //throw e;
                }
            }
            if (attributes != null &&
                attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            else
            {
                return value.ToString();
            }
        }
    }
}
