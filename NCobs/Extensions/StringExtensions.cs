namespace NCobs.Extensions
{
    using System.Collections.Generic;
    using System.Linq;

    public static class StringExtensions
    {
        public static IEnumerable<string> Split(this string str, int maxLength)
        {
            for (int i = 0; i < str.Length / maxLength; i++)
            {
                yield return str.Substring(i * maxLength, maxLength);
            }
        }

        public static string TakeTo(this string str, string value)
        {
            return string.Concat(str.Take(str.IndexOf(value)));
        }
    }
}
