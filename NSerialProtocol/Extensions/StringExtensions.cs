using System;

namespace NSerialProtocol.Extensions
{

    internal static class StringExtensions
    {
        /// <summary>
        /// Removes all characters up to, but not including, a given string value.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="value"></param>
        /// <param name="startIndex"></param>
        /// <param name="stringComparison"></param>
        /// <returns></returns>
        internal static string RemoveTo(this string str, string value, int startIndex = 0,
            StringComparison stringComparison = StringComparison.CurrentCulture)
        {
            int removeLength = str.IndexOf(value, startIndex, stringComparison);

            string result = (removeLength < 0) ? str : str.Remove(startIndex, removeLength);

            return result;
        }

        internal static string RemoveToLast(this string str, string value, int startIndex = 0,
            StringComparison stringComparison = StringComparison.CurrentCulture)
        {
            int removeLength = str.LastIndexOf(value, startIndex, stringComparison);
            string result = (removeLength < 0) ? str : str.Remove(startIndex, removeLength);
            return result;
        }

        /// <summary>
        /// Removes all characters up to and including a given string value.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="value"></param>
        /// <param name="startIndex"></param>
        /// <param name="stringComparison"></param>
        /// <returns></returns>
        internal static string RemoveThrough(this string str, string value,
            int startIndex = 0, StringComparison stringComparison = StringComparison.CurrentCulture)
        {
            int removeLength = str.IndexOf(value, startIndex, stringComparison) + value.Length;
            string result = (removeLength < 0) ? str : str.Remove(startIndex, removeLength);
            return result;
        }

        internal static string RemoveThroughLast(this string str, string value,
            int startIndex = 0, StringComparison stringComparison = StringComparison.CurrentCulture)
        {
            int removeLength = str.LastIndexOf(value, startIndex, stringComparison) + value.Length;
            string result = (removeLength < 0) ? str : str.Remove(startIndex, removeLength);
            return result;
        }

        /// <summary>
        /// Removes the first occurence of given string value.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="value"></param>
        /// <param name="startIndex"></param>
        /// <param name="stringComparison"></param>
        /// <returns></returns>
        internal static string RemoveFirstOccurenceOf(this string str, string value,
            int startIndex = 0, StringComparison stringComparison = StringComparison.CurrentCulture)
        {
            int startIndexOfOccurence = str.IndexOf(value, startIndex);
            int lengthOfOccurence = value.Length;

            string result = (startIndexOfOccurence < 0) ? str : str.Remove(startIndexOfOccurence, lengthOfOccurence);

            return result;
        }


    }
}
