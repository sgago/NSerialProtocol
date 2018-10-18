using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseDynamicLength
{
    class Program
    {
        static void Main(string[] args)
        {
            Encoding encoding = Encoding.ASCII;

            string data1 = encoding.GetString(new byte[] {
                    4,
                    92,
                    93,
                    94
            });

            string data2 = encoding.GetString(new byte[] {
                    7,  // LSB is first!!!
                    0,
                    0,
                    0,
                    92,
                    93,
                    94
            });

            string data3 = encoding.GetString(new byte[] {
                    4,
                    92,
                    93,
                    94,
                    4,
                    95,
                    96,
                    97,
            });

            int dynamicLength1 = GetLength(data1, 0, typeof(byte), encoding);
            int dynamicLength2 = GetLength(data2, 0, typeof(int), encoding);
            int dynamicLength3 = GetLength(data3, 0, typeof(byte), encoding);

            List<string> subStrings1 = SplitByLength(data1, dynamicLength1, encoding);
            List<string> subStrings2 = SplitByLength(data2, dynamicLength2, encoding);
            List<string> subStrings3 = SplitByLength(data3, dynamicLength3, encoding);
        }

        static int GetLength(string data, int index, Type type, Encoding encoding)
        {
            int length = -1;
            byte[] bytes = encoding.GetBytes(data);

            if (type == typeof(byte))
            {
                length = bytes[index];
            }
            //else if (property.PropertyType == typeof(sbyte))
            //{
            //    property.SetValue(serialFrame, binaryReader.ReadSByte());
            //}
            else if (type == typeof(char))
            {
                length = BitConverter.ToChar(bytes, index);
            }
            //else if (property.PropertyType == typeof(bool))
            //{
            //    property.SetValue(serialFrame, binaryReader.ReadBoolean());
            //}
            //else if (property.PropertyType == typeof(short))
            //{
            //    property.SetValue(serialFrame, binaryReader.ReadInt16());
            //}
            else if (type == typeof(int))
            {
                length = BitConverter.ToInt32(bytes, index);
            }
            //else if (property.PropertyType == typeof(long))
            //{
            //    property.SetValue(serialFrame, binaryReader.ReadInt64());
            //}
            //else if (property.PropertyType == typeof(ushort))
            //{
            //    property.SetValue(serialFrame, binaryReader.ReadUInt16());
            //}
            //else if (property.PropertyType == typeof(uint))
            //{
            //    property.SetValue(serialFrame, binaryReader.ReadUInt32());
            //}
            //else if (property.PropertyType == typeof(ulong))
            //{
            //    property.SetValue(serialFrame, binaryReader.ReadUInt64());
            //}
            //else if (property.PropertyType == typeof(float))
            //{
            //    property.SetValue(serialFrame, binaryReader.ReadSingle());
            //}
            //else if (property.PropertyType == typeof(double))
            //{
            //    property.SetValue(serialFrame, binaryReader.ReadDouble());
            //}
            //else if (property.PropertyType == typeof(decimal))
            //{
            //    property.SetValue(serialFrame, binaryReader.ReadDecimal());
            //}
            //else if (property.PropertyType == typeof(string))
            //{
            //    // TODO: Support for different string encodings?
            //    property.SetValue(serialFrame, binaryReader.ReadString());
            //}
            //else if (property.PropertyType == typeof(byte[]))
            //{
            //    property.SetValue(serialFrame, binaryReader.ReadBytes(
            //            (property.GetValue(serialFrame) as Array).Length
            //    ));
            //}
            //else if (property.PropertyType == typeof(char[]))
            //{
            //    property.SetValue(serialFrame, binaryReader.ReadChars(
            //            (property.GetValue(serialFrame) as Array).Length
            //    ));
            //}

            return length;
        }

        static List<string> SplitByLength(string value, int length, Encoding encoding)
        {

            string subString;
            byte[] bytes;
            byte[] subStringBytes;

            List<string> subStrings = new List<string>();

            do
            {
                // TODO: It is inefficient to reconvert value to bytes again and again
                bytes = encoding.GetBytes(value);

                subStringBytes = bytes.Take(length).ToArray();

                subString = encoding.GetString(subStringBytes);

                subStrings.Add(subString);

                // TODO: Similar to above, it is inefficient to do string manipulation
                // again and again.  Convert to a byte array once and chop that byte array up.
                // Will need to hold onto an index value for where we are in the array.
                value = RemoveThrough(value, subString);
            }
            while (!string.IsNullOrEmpty(value));

            return subStrings;
        }

        static string RemoveThrough(string str, string value, int startIndex = 0,
            StringComparison stringComparison = StringComparison.CurrentCulture)
        {
            int removeLength = str.IndexOf(value, startIndex, stringComparison) + value.Length;

            //removeLength = removeLength > str.Length ? str.Length : removeLength;
            removeLength = Math.Min(str.Length, removeLength);


            //string result = (removeLength < 0) ? str : str.Remove(startIndex, removeLength);

            string result = str.Remove(startIndex, removeLength);

            return result;
        }
    }
}
