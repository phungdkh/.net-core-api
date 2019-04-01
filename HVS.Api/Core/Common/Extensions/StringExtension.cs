using System;

namespace HVS.Api.Core.Common.Extensions
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
