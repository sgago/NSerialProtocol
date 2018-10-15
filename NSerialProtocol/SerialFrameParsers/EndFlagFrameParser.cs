using NSerialProtocol.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NSerialProtocol.FrameParsers
{
    public class EndFlagFrameParser : SerialFrameParser
    {
        /// <summary>
        /// Regex pattern for only a starting flag used to demarcate the
        /// start of the packet with an optional fixed length.
        /// </summary>
        private const string EndFlagPattern = @"((?!{0}).)*{0}";

        private int MAX_BUFFER_SIZE { get; set; } = 4096;

        private string InputBuffer { get; set; } = "";

        private string EndFlag { get; set; }

        private int FixedLength { get; set; } = 0;

        private bool IsFixedLength
        {
            get
            {
                return FixedLength > 0;
            }
        }

        /// <summary>
        /// Gets or sets the Regex object for parsing serial packets.
        /// </summary>
        private Regex SerialPacketRegex { get; set; }

        //public delegate void SerialFrameReceivedEventHandler(object sender, SerialFrameReceivedEventArgs e);

        //public delegate void SerialFrameErrorEventHandler(object sender, SerialFrameErrorEventArgs e);


        /// <summary>
        /// Indicates that a complete serial message has been received through a port represented by the
        /// SerialPort object.
        /// </summary>
        public override event SerialFrameReceivedEventHandler SerialFrameReceived;

        /// <summary>
        /// Indicates that a framing error has occured with data in the SerialPacketPrototype object.
        /// </summary>
        public override event SerialFrameErrorEventHandler SerialFrameError;



        public EndFlagFrameParser(string endFlag)
        {
            EndFlag = endFlag;

            SerialPacketRegex = GetRegex(EndFlagPattern, EndFlag);
        }

        /// <summary>
        /// Replaces all regex meta or special characters with their regex
        /// literal values.
        /// </summary>
        /// <param name="value">String to replace all regex special characters.</param>
        /// <returns>String with all regex meta characters replaced with their literal
        /// values.</returns>
        private string RegexMetaToLiteral(string value)
        {
            string result = string.Empty;

            if (!string.IsNullOrEmpty(value))
            {
                // Regex pattern of regex meta characters to replace
                string regexMetaCharactersPattern = @"(?=[\\^\$\{\}\[\]\(\)\.\*\+\?\|\<\>\&])";

                // Puts a "\" in front of all regex meta characters
                result = Regex.Replace(value, regexMetaCharactersPattern, @"\");
            }

            return result;
        }

        private string GetRegexPattern(string pattern, string endFlag)
        {
            string endRegexDelimiter = RegexMetaToLiteral(endFlag);

            pattern = string.Format(pattern, endRegexDelimiter);

            return pattern;
        }

        private Regex GetRegex(string pattern, string endFlag)
        {
            return new Regex(GetRegexPattern(pattern, endFlag), RegexOptions);
        }

        public override void Parse(string data)
        {
            IList<string> frames = null;

            // Grab data to clear other buffers
            // TODO: Worry about micro-optimization theatre later
            InputBuffer += data;

            if (InputBuffer.Length < MAX_BUFFER_SIZE)
            {
                // Parse packets ending delimiter and fixed length
                frames = SerialPacketRegex
                    .Matches(InputBuffer)
                    .Cast<Match>()
                    .Select(x => x.Value)
                    .ToList();

                // For each frame...
                for (int i = 0; i < frames.Count(); i++)
                {
                    // Are we check frame length and, if we are checking length, is the frame the right size?
                    if (IsFixedLength && frames[i].Length != FixedLength)
                    {
                        // Frame is a bad length, send a frame error event
                        SerialFrameError?.Invoke(this, new SerialFrameErrorEventArgs(frames[i], FrameErrorType.InvalidLength));
                    }
                    else
                    {
                        // Frame is good, send frame received event
                        SerialFrameReceived?.Invoke(this, new SerialFrameReceivedEventArgs(frames[i]));
                    }

                    // Clear this frame the InputBuffer to avoid an overrun
                    InputBuffer = InputBuffer.RemoveThrough(EndFlag, 0, StringComparison.Ordinal);
                }
            }
            else
            {
                // Uhoh, buffer overran its max size
                // Pass entire buffer via frame error event
                SerialFrameError?.Invoke(this, new SerialFrameErrorEventArgs(InputBuffer, FrameErrorType.BufferOverrun));

                // Drop the entire buffer
                InputBuffer = "";
            }
        }
    }
}
