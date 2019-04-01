using System.Text.RegularExpressions;

namespace HVS.Api.Core.Common.Helpers
{
    /// <summary>
    /// String Helper
    /// </summary>
    public class StringHelper
    {
        /// <summary>
        /// Convert string to slug(url friendly) 
        /// EX: "String Helper" => "string-helper"
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ConvertTextToSlug(string text)
        {
            var str = Regex.Replace(text.ToLower(), "[^a-zA-Z0-9]+", "-", RegexOptions.None);
            str = str.Replace("--", "-");
            return str.Length >= 200 ? str.Substring(0, 200) : str;
        }
    }
}
