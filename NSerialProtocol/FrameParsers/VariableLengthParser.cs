using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using global::NSerialProtocol.Extensions;

namespace NSerialProtocol.FrameParsers
{    
    public class VariableLengthParser : FrameParser, IFrameParser
    {
        private const int ExtendedAsciiCodepage = 437;

        private readonly Encoding ExtendedAsciiEncoding = Encoding.GetEncoding(ExtendedAsciiCodepage);

        private int LengthBytePosition { get; set; } = 0;

        private Type LengthType { get; set; } = typeof(int);

        private int LengthByteCount { get; set; } = sizeof(int);

        private int MinimumLength
        {
            get
            {
                return LengthByteCount + LengthBytePosition;
            }
        }

        public VariableLengthParser(int lengthBytePosition, Type lengthType)
        {
            LengthBytePosition = lengthBytePosition;
            LengthType = lengthType;
            LengthByteCount = Marshal.SizeOf(lengthType);
        }

        public override IList<string> Parse(IList<string> values)
        {
            int length = -1;
            string frame = string.Empty;
            byte[] frameBytes = null;
            byte[] valueBytes = null;
            List<string> matches = new List<string>();

            for (int i = 0; i < values.Count; i++)
            {
                // Is values[i] NOT null or the empty string?
                // There's no point in parsing if it's an empty value
                if (!string.IsNullOrEmpty(values[i]))
                {
                    do
                    {
                        // TODO: It is inefficient to reconvert value to bytes again and again
                        valueBytes = ExtendedAsciiEncoding.GetBytes(values[i]);

                        length = GetLength(values[i], LengthBytePosition, LengthType, ExtendedAsciiEncoding);

                        // First, is length greater than zero?  Otherwise, not enough bytes
                        // Second, do we have all the bytes in for the frame?
                        if (length >= 0 && valueBytes.Length >= length)
                        {
                            frameBytes = valueBytes.Take(length).ToArray();

                            frame = ExtendedAsciiEncoding.GetString(frameBytes);

                            matches.Add(frame);

                            // TODO: Similar to above, it is inefficient to do string manipulation
                            // again and again.  Convert to a byte array once and chop that byte array up.
                            // Will need to hold onto an index value for where we are in the array.
                            values[i] = values[i].RemoveThrough(frame);
                        }
                        else
                        {
                            // We don't have enough bytes to parse out the frame,
                            // so clear this value and move on
                            values[i] = string.Empty;
                        }
                    }
                    while (!string.IsNullOrEmpty(values[i]));
                }
            }

            return matches;
        }

        private int GetLength(string frame, int index, Type lengthType, Encoding encoding)
        {
            int length = -1;

            byte[] bytes = encoding.GetBytes(frame);

            // Do we have enough bytes of data to read the length?
            if (frame.Length >= MinimumLength)
            {
                if (lengthType == typeof(byte))
                {
                    length = bytes[index];
                }
                //else if (property.PropertyType == typeof(sbyte))
                //{
                //    property.SetValue(serialFrame, binaryReader.ReadSByte());
                //}
                else if (lengthType == typeof(char))
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
                else if (lengthType == typeof(int))
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
            }

            return length;
        }
    }
}
