using System.Collections.Generic;

namespace System.Linq.Search.Core.Extensions
{
    public static class StringExtensions
    {
        // joins a list of string to a string, pretty obvious
        public static string JoinToString(this IEnumerable<string> list,
                                          string delimiter = "")
        {
            return string.Join(delimiter, list);
        }

        // Checks if a string is empty, null or whitespace
        public static bool IsBlank(this string value)
        {
            return string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value);
        }
    }
}