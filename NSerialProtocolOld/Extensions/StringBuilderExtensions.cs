/*
The MIT License (MIT)

Copyright (c) 2016 Steven Gago

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

namespace NSerialPort.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal static class StringBuilderExtensions
    {
        internal static bool Contains(this StringBuilder stringBuilder, string value)
        {
            return stringBuilder.ToString().Contains(value);
        }


        internal static int CountOccurances(this StringBuilder stringBuilder, string value, StringComparison stringComparison)
        {
            int count = 0;
            int minIndex = 0;

            do
            {
                minIndex = stringBuilder.IndexOf(value, minIndex + value.Length, stringComparison);
                count++;
            }
            while (minIndex != -1);

            return count;
        }



        internal static int IndexOf(this StringBuilder stringBuilder, string value, StringComparison stringComparison)
        {
            return stringBuilder.ToString().IndexOf(value, stringComparison);
        }

        internal static int IndexOf(this StringBuilder stringBuilder, string value, int startIndex, StringComparison stringComparison)
        {
            return stringBuilder.ToString().IndexOf(value, startIndex, stringComparison);
        }


        internal static int LastIndexOf(this StringBuilder stringBuilder, string value, StringComparison stringComparison)
        {
            return stringBuilder.ToString().LastIndexOf(value, stringComparison);
        }

        internal static int LastIndexOf(this StringBuilder stringBuilder, string value, int startIndex, StringComparison stringComparison)
        {
            return stringBuilder.ToString().LastIndexOf(value, startIndex, stringComparison);
        }




        internal static void RemoveTo(this StringBuilder stringBuilder, int startIndex, string value, StringComparison stringComparison)
        {
            stringBuilder.Remove(startIndex, Math.Max(stringBuilder.IndexOf(value, stringComparison), 0));
        }

        internal static void RemoveThrough(this StringBuilder stringBuilder, int startIndex, string value, StringComparison stringComparison)
        {
            int indexOf = stringBuilder.IndexOf(value, stringComparison);

            if (indexOf != -1)
            {
                stringBuilder.Remove(startIndex, indexOf + value.Length);
            }
        }



        internal static void RemoveToLast(this StringBuilder stringBuilder, int startIndex, string value, StringComparison stringComparison)
        {
            stringBuilder.Remove(0, Math.Max(stringBuilder.LastIndexOf(value, stringComparison), 0));
        }

        internal static void RemoveThroughLast(this StringBuilder stringBuilder, int startIndex, string value, StringComparison stringComparison)
        {
            int lastIndexOf = stringBuilder.LastIndexOf(value, stringComparison);

            if (lastIndexOf != -1)
            {
                stringBuilder.Remove(startIndex, lastIndexOf + value.Length);
            }
        }
    }
}
