using System;
using System.Collections.Generic;
using System.Text;

namespace HVS.Api.Core.Common.Utilities
{
    public static class StringExtension
    {
        public static string[] SplitToArray(this string text, string separator)
        {
            string[] separators = null;
            if (string.IsNullOrEmpty(separator))
            {
                separators = new string[] { "," };
            }
            else
            {
                separators = new string[] { separator };
            }
            return text.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
