using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace HVS.Api.Core.Common.Utilities
{
    public static class EnumUtilities
    {
        public static string GetEnumName<T>(this T value)
        {
            try
            {
                var fi = value.GetType().GetField(value.ToString());

                var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

                return attributes.Length > 0 ? attributes[0].Description : value.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
