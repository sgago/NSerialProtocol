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

        private int LengthIndex { get; set; } = 0;

        private Type LengthType { get; set; } = typeof(int);

        private int LengthByteCount { get; set; } = sizeof(int);

        private int MinimumLength
        {
            get
            {
                return LengthByteCount + LengthIndex;
            }
        }

        public VariableLengthParser(int lengthIndex, Type lengthType)
        {
            LengthIndex = lengthIndex;
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

                        length = GetLength(values[i], LengthIndex, LengthType, ExtendedAsciiEncoding);

                        // TODO: What is the length is 0 or negative!!!!!

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
            // FIXME: This should probably be a ulong
            // for stupidly, stupidly monsterous frame lengths that nobody
            // in their right mind should ever use.
            // ulong would would be ~16384 petabytes of frame
            // long would be ~8000 petabytes of frame
            // uint would be about ~4 gigabytes of frame
            // int would be ~2 gigabytes of frame
            // ushort would be ~64 kilobytes of frame
            // short would be ~32 kilobytes of frame
            int length = -1;

            byte[] bytes = encoding.GetBytes(frame);

            // Do we have enough bytes of data to read the length?
            if (frame.Length >= MinimumLength)
            {
                if (lengthType == typeof(byte))
                {
                    length = bytes[index];
                }
                else if (lengthType == typeof(sbyte))
                {
                    length = bytes[index];
                }
                else if (lengthType == typeof(char))
                {
                    length = frame[0];
                }
                else if (lengthType == typeof(short))
                {
                    length = BitConverter.ToInt16(bytes, index);
                }
                else if (lengthType == typeof(ushort))
                {
                    length = BitConverter.ToUInt16(bytes, index);
                }
                else if (lengthType == typeof(int))
                {
                    length = BitConverter.ToInt32(bytes, index);
                }
                else if (lengthType == typeof(uint))
                {
                    length = (int)BitConverter.ToUInt32(bytes, index);
                }
                else if (lengthType == typeof(long))
                {
                    length = BitConverter.ToInt32(bytes, index);
                }
                else if (lengthType == typeof(ulong))
                {
                    length = (int)BitConverter.ToUInt32(bytes, index);
                }
                else
                {
                    throw new NotSupportedException(
                        "The type " + lengthType.Name + " is not a supported length type.");
                }
            }

            return length;
        }
    }
}
