using System.Collections.Generic;

namespace QueryString
{
    public static class EnumerableExtension
    {
        public static string Join(this IEnumerable<string> values, string separator = "")
        {
            return string.Join(separator, values);
        }
    }
}
