using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSerialProtocol.Extensions
{
    internal static class StringExtensions
    {
        internal static string RemoveTo(this string str, string value, int startIndex = 0, StringComparison stringComparison = StringComparison.CurrentCulture)
        {
            int removeLength = str.IndexOf(value, startIndex, stringComparison);
            string result = (removeLength < 0) ? str : str.Remove(startIndex, removeLength);
            return result;
        }

        internal static string RemoveThrough(this string str, string value, int startIndex = 0, StringComparison stringComparison = StringComparison.CurrentCulture)
        {
            int removeLength = str.IndexOf(value, startIndex, stringComparison) + value.Length;
            string result = (removeLength < 0) ? str : str.Remove(startIndex, removeLength);
            return result;
        }

        internal static string RemoveFirstOccurenceOf(this string str, string value, int startIndex = 0, StringComparison stringComparison = StringComparison.CurrentCulture)
        {
            int startIndexOfOccurence = str.IndexOf(value, startIndex);
            int lengthOfOccurence = value.Length;

            string result = (startIndexOfOccurence < 0) ? str : str.Remove(startIndexOfOccurence, lengthOfOccurence);

            return result;
        }


    }
}
