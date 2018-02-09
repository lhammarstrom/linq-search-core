using System.Collections.Generic;

namespace System.Linq.Search.Core.Extensions
{
    public static class StringExtensions
    {
        // joins a list of string to a string, pretty obvious
        public static string JoinToString(this IEnumerable<string> list, string delimiter = "") =>
            string.Join(delimiter, list);

        // Checks if a string is empty, null or whitespace
        public static bool IsEmpty(this string value) => string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value);
    }
}
