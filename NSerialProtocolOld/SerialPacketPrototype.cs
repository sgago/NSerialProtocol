namespace NSerialProtocol
{
    using EventArgs;
    using NByteStuff;
    using NCobs.Extensions;
    using NFec;
    using NSerialPort.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    // TODO: Separate the maximum packet length via a MaxPacketLength(LENGTH_OF_PACKET_INTEGER) method
    // TODO: from DynamicLength and FixedLenght.  This will be really nice!
    /*
     * Instead do a .DynamicLengthAt(1, LengthType.ASCII OR LengthType.Binary).MaximumLength(NUMBER_OF_BYTES)
     *  - Throw an error if packet length cannot be represented due to the maximum length being specified
     */

    /// <summary>
    ///
    /// </summary>
    public class SerialPacketPrototype : ISerialPacketPrototype
    {
        /// <summary>
        /// Regex pattern for no delimiters or fixed length packets.
        /// </summary>
        private const string NoFlagsPattern = @"^.*$";

        /// <summary>
        /// Regex pattern for only a starting flag used to demarcate the
        /// start of the packet with an optional fixed length.
        /// </summary>
        private const string StartFlagPattern = @"{0}((?!{0}).){2}";

        /// <summary>
        /// Regex pattern for only an ending flag used to demarcate the
        /// end of the packet with an optional fixed length.
        /// </summary>
        private const string EndFlagPattern = @"((?!{1}).){2}{1}";

        /// <summary>
        /// Regex pattern for a packet of fixed length without delimiters.
        /// </summary>
        private const string FixedLengthPattern = @".{2}";

        /// <summary>
        /// Regex pattern for a packet consisting of a starting and ending flags and an
        /// optional fixed length.
        /// </summary>
        private const string StartFlagEndFlagDelimitersPattern = @"{0}((?!{1}).){2}{1}";

        /// <summary>
        /// Regex options to use when parsing serial packets.
        /// </summary>
        private RegexOptions RegexOptions = RegexOptions.CultureInvariant | RegexOptions.Multiline;

        /// <summary>
        /// Gets or sets the fixed length regex repitition quantifier specifiying
        /// the exact message size.
        /// </summary>
        private string FixedLengthString { get; set; } = "*";

        /// <summary>
        /// Gets or sets the Regex object for parsing serial packets.
        /// </summary>
        private Regex SerialPacketRegex { get; set; }

        private ISerialPacket Prototype { get; set; } = new SerialPacket();


        // TODO: Fix this auto generated code:
        Action<StringBuilder> ISerialPacketPrototype.Parse { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        private Action<StringBuilder> Parse;

        /// <summary>
        /// Represents the method that will handle the MessageReceived event of a SerialPacketPrototype
        /// object.
        /// </summary>
        /// <param name="sender">The sender of the event, which is the SerialPacketPrototype object.</param>
        /// <param name="e">A SerialMessageReceivedEventArgs object that contains the event data.</param>
        public delegate void SerialPacketReceivedEventHandler(object sender, SerialPacketReceivedEventArgs e);

        /// <summary>
        /// Represents the method that will handle the ErrorReceived event of a SerialPacketPrototype
        /// object.
        /// </summary>
        /// <param name="sender">The sender of the event, which is the SerialPacketPrototype object.</param>
        /// <param name="e">A FrameErrorReceivedEventArgs object that contains the event data.</param>
        public delegate void SerialPacketErrorEventHandler(object sender, FrameErrorReceivedEventArgs e);

        /// <summary>
        /// Indicates that a complete serial message has been received through a port represented by the
        /// SerialPort object.
        /// </summary>
        public event SerialPacketReceivedEventHandler SerialPacketReceived;

        /// <summary>
        /// Indicates that a framing error has occured with data in the SerialPacketPrototype object.
        /// </summary>
        public event SerialPacketErrorEventHandler SerialPacketError;

        /// <summary>
        /// Initializes a new instance of the SerialPacketPrototype class.
        /// </summary>
        public SerialPacketPrototype()
        {
            // Specify the default packet parsing algrothim
            Parse = (sb) => DefaultParse(sb);

            // Specify the default starting serial packet regex
            SerialPacketRegex = GetSerialPacketRegex();
        }

        public ISerialPacketPrototype AddField(string key, IField field)
        {
            Prototype.AddField(key, field);

            return this;
        }

        public ISerialPacketPrototype AddStaticField(string key, int index, int length, string value, int order)
        {
            ISerialPacketPrototype result = this;

            if (!string.IsNullOrEmpty(value))
            {
                result = AddField(key, new Field(index, length, value, order));
            }

            return result;
        }

        public ISerialPacketPrototype AddStaticField(string key, int index, int length, string value)
        {
            ISerialPacketPrototype result = this;

            if (!string.IsNullOrEmpty(value))
            {
                result = AddField(key, new Field(index, length, value));
            }

            return result;
        }

        public ISerialPacketPrototype AddDynamicField(string key, int index, string value, int order)
        {
            return AddField(key, new Field(index, 0, value, order, false));
        }

        public ISerialPacketPrototype AddDynamicField(string key, int index)
        {
            return AddField(key, new Field(index, 0, "", false));
        }

        public void RemoveField(string key)
        {
            Prototype.RemoveField(key);
        }

        /// <summary>
        /// Specifies that serial packets will have a leading
        /// flag or delimiter used to demarcate the start of a serial
        /// packet.
        /// </summary>
        /// <param name="flag">Flag that demarcates the start of a serial packet.</param>
        /// <returns>A reference to this ISerialPackets object.</returns>
        public ISerialPacketPrototype StartFlag(string flag)
        {
            Prototype.SetStartFlag(flag);

            SerialPacketRegex = GetSerialPacketRegex();

            return this;
        }

        /// <summary>
        /// Specifies that serial packets will have a lagging flag
        /// or delimiter used to demarcate the end of a serial packet.
        /// </summary>
        /// <param name="flag">Flag that demarcates the end of a serial packet.</param>
        /// <returns>A reference to this ISerialPackets object.</returns>
        public ISerialPacketPrototype EndFlag(string flag)
        {
            Prototype.SetEndFlag(flag);

            SerialPacketRegex = GetSerialPacketRegex();

            return this;
        }

        /// <summary>
        /// Specifies the starting index of the packet payload.
        /// </summary>
        /// <param name="index">The starting index of the payload.</param>
        /// <returns>A reference to this ISerialPackets object.</returns>
        public ISerialPacketPrototype Payload(int index)
        {
            Prototype.SetPayload(index);

            return this;
        }

        /// <summary>
        /// Specifies that serial packets will be of a single, uniform length.
        /// </summary>
        /// <param name="length">The length of all serial packets.</param>
        /// <returns>A reference to this ISerialPackets object.</returns>
        public ISerialPacketPrototype FixedLength(int length)
        {
            FixedLengthString = length < 1 ? "*" : "{" + length.ToString() + "}";

            SerialPacketRegex = GetSerialPacketRegex();

            return this;
        }

        public ISerialPacketPrototype DynamicLength(int index, string formatSpecifier = "")
        {
            Prototype.SetDynamicLength(index, formatSpecifier);

            return this;
        }

        /// <summary>
        /// Specifies the forward error correction (FEC) algorthim and location of
        /// the corresponding error correction code (ECC) in the serial packet.
        /// </summary>
        /// <param name="fec">The forward error correction object to use.</param>
        /// <param name="length"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public ISerialPacketPrototype Fec(IFec fec, int index, int length)
        {
            Prototype.SetFec(fec, index, length);

            return this;
        }

        /// <summary>
        /// Specifies the byte stuffing algorithm to use.
        /// </summary>
        /// <param name="byteStuff">The byte stuffing algorthim to use.</param>
        /// <returns>A reference to this ISerialPackets object.</returns>
        public ISerialPacketPrototype ByteStuff(IByteStuff byteStuff)
        {
            Prototype.SetByteStuff(byteStuff);

            return this;
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

        /// <summary>
        /// Gets the regex pattern for parsing serial packets.
        /// </summary>
        /// <returns>Regex pattern to use for parsing serial packets.</returns>
        private string GetSerialPacketRegexPattern()
        {
            string pattern = string.Empty;
            string startRegexDelimiter = RegexMetaToLiteral(Prototype.StartFlag?.Value);
            string endRegexDelimiter = RegexMetaToLiteral(Prototype.EndFlag?.Value);

            if (string.IsNullOrEmpty(startRegexDelimiter) &&
                string.IsNullOrEmpty(endRegexDelimiter) &&
                FixedLengthString == "*")
            {
                pattern = NoFlagsPattern;
            }
            else if (string.IsNullOrEmpty(startRegexDelimiter) &&
                string.IsNullOrEmpty(endRegexDelimiter))
            {
                pattern = FixedLengthPattern;
            }
            else if (!string.IsNullOrEmpty(startRegexDelimiter) &&
                !string.IsNullOrEmpty(endRegexDelimiter))
            {
                pattern = StartFlagEndFlagDelimitersPattern;
            }
            else if (!string.IsNullOrEmpty(startRegexDelimiter))
            {
                pattern = StartFlagPattern;
            }
            else
            {
                pattern = EndFlagPattern;
            }

            pattern = string.Format(pattern, startRegexDelimiter, endRegexDelimiter, FixedLengthString);

            return pattern;
        }

        /// <summary>
        /// Gets the serial packet regex for parsing serial packets.
        /// </summary>
        /// <returns>Regex for parsing serial packets.</returns>
        private Regex GetSerialPacketRegex()
        {
            return new Regex(GetSerialPacketRegexPattern(), RegexOptions);
        }

        /// <summary>
        /// The default method for parsing serial packets, clearing completed packets from the
        /// serial port input buffer, and raising relavent serial packet events.
        /// </summary>
        /// <param name="inputBuffer">The StringBuilder serial port input buffer to parse packets from.</param>
        public void DefaultParse(StringBuilder inputBuffer)
        {
            ISerialPacket receivedPacket = null;
            List<string> dynLenFrames = null;
            IEnumerable<string> frames = null;
            IEnumerable<string> frameErrors = null;
            //IEnumerable<string> fixedLengthErrors = null;
            //IEnumerable<string> dynamicLengthErrors = null;
            IEnumerable<string> fecErrors = null;
            List<SerialPacketReceivedEventArgs> serialPacketsReceivedEventArgs = null;

            // First, is there anything in the input buffer?
            if (inputBuffer.Length > 0)
            {
                frameErrors = new List<string>();
                serialPacketsReceivedEventArgs = new List<SerialPacketReceivedEventArgs>();

                // Parse packets by starting delimiters, ending delimiters, and fixed length
                frames = SerialPacketRegex
                    .Matches(inputBuffer.ToString())
                    .Cast<Match>()
                    .Select(x => x.Value);

                if (Prototype.DynamicLength != null)
                {
                    dynLenFrames = new List<string>();

                    // Parse each packet based on their dynamic length
                    foreach (string packet in frames)
                    {
                        dynLenFrames.AddRange(ParseDynamicLengthPackets(packet));
                    }

                    //dynamicLengthErrors = dynLenFrames.Except(dynLenFrames).ToList();

                    frames = dynLenFrames;
                }

                frameErrors = GetFrameErrors(inputBuffer.ToString(), frames);

                ClearInputBuffer(inputBuffer, frames);

                if (Prototype.Fec != null)
                {
                    fecErrors = new List<string>();

                    foreach (string packet in frames)
                    {
                        //if (SerialPacketConverter.Has)
                    }
                }

                foreach (string frame in frames)
                {
                    receivedPacket = Prototype.ParseFrame(frame);

                    SerialPacketReceived?.Invoke(this, new SerialPacketReceivedEventArgs(receivedPacket));
                }
            }
        }

        private IEnumerable<string> GetFrameErrors(string inputBuffer, IEnumerable<string> frames)
        {
            string frameError = string.Empty;
            StringBuilder input = new StringBuilder(inputBuffer);
            List<string> frameErrors = new List<string>();

            foreach (string frame in frames)
            {
                frameError = input.ToString().TakeTo(frame);

                if (!string.IsNullOrEmpty(frameError))
                {
                    frameErrors.Add(frameError);
                }

                input.RemoveThrough(0, frame, StringComparison.Ordinal);
            }

            if (!string.IsNullOrEmpty(input.ToString()))
            {
                if (Prototype.StartFlag?.Value != null &&
                    input.Contains(Prototype.StartFlag?.Value))
                {
                    frameError = input.ToString().TakeTo(Prototype.StartFlag.Value);
                }
                else
                {
                    frameError = input.ToString();
                }

                if (!string.IsNullOrEmpty(frameError))
                {
                    frameErrors.Add(frameError);
                }
            }

            return frameErrors;
        }

        private IEnumerable<string> GetFixedLengthErrors(string inputBuffer, IEnumerable<string> frames)
        {
            int index = 0;
            string invalidFixedLengthPacket = string.Empty;
            StringBuilder input = new StringBuilder(inputBuffer);
            List<string> invalidFixedLengthPackets = new List<string>();

            foreach (string frame in frames)
            {
                index = input.ToString().IndexOf(frame);
                invalidFixedLengthPacket = input.ToString().Substring(0, index);

                if (HasValidFlags(invalidFixedLengthPacket))
                {
                    invalidFixedLengthPackets.Add(invalidFixedLengthPacket);
                }

                input.RemoveThrough(0, frame, StringComparison.Ordinal);
            }

            return invalidFixedLengthPackets;
        }

        private StringBuilder ClearInputBuffer(StringBuilder inputBuffer, IEnumerable<string> frames)
        {
            int lastIndex = 0;

            // Did we get any packets?
            if (frames.Count() > 0)
            {
                do
                {
                    inputBuffer.RemoveThrough(0, frames.Last(), StringComparison.Ordinal);
                    lastIndex = inputBuffer.LastIndexOf(frames.Last(), StringComparison.Ordinal);
                }
                while (lastIndex != -1);
            }

            // Did user specify packets having a starting delimiter?
            if (Prototype.StartFlag != null)
            {
                // Does input buffer contain a starting delimiter?
                if (inputBuffer.Contains(Prototype.StartFlag.Value))
                {
                    // Delete everything up to the starting delimiter (it's all garbage data in front of the delimiter)
                    inputBuffer.RemoveTo(0, Prototype.StartFlag.Value, StringComparison.Ordinal);
                    inputBuffer.RemoveTo(0, Prototype.StartFlag.Value, StringComparison.Ordinal);
                }
                else
                {
                    // Else input buffer doesn't have a starting delimiter, delete everything cause it's all garbage
                    inputBuffer.Clear();
                }
            }

            if (Prototype.EndFlag != null && inputBuffer.Contains(Prototype.EndFlag.Value))
            {
                if (Prototype.StartFlag != null && Prototype.StartFlag.Value == Prototype.EndFlag.Value)
                {
                    if (inputBuffer.CountOccurances(Prototype.EndFlag.Value, StringComparison.Ordinal) % 2 == 0)
                    {
                        inputBuffer.RemoveThroughLast(0, Prototype.EndFlag.Value, StringComparison.Ordinal);
                    }
                    else
                    {
                        inputBuffer.RemoveToLast(0, Prototype.EndFlag.Value, StringComparison.Ordinal);
                    }
                }
                else
                {
                    inputBuffer.RemoveThroughLast(0, Prototype.EndFlag.Value, StringComparison.Ordinal);
                }
            }

            return inputBuffer;
        }

        private List<string> ParseDynamicLengthPackets(string value)
        {
            string substring = string.Empty;
            List<string> values = new List<string>();

            if (!string.IsNullOrEmpty(value) && Prototype.DynamicLength.Index < value.Length)
            {
                string dynamicLengthValue =
                    value.Substring(Prototype.DynamicLength.Index, Prototype.DynamicLength.Length);

                int dynamicLength = Prototype.DynamicLengthConverter.ConvertToInt(dynamicLengthValue);

                while (dynamicLength > 0 && value.Length >= dynamicLength)
                {
                    substring = value.Substring(0, dynamicLength);

                    if (HasValidFlags(substring))
                    {
                        values.Add(substring);
                    }

                    value = value.Remove(0, dynamicLength);

                    if (!string.IsNullOrEmpty(value) && Prototype.DynamicLength.Index < value.Length)
                    {
                        dynamicLengthValue =
                            value.Substring(Prototype.DynamicLength.Index, Prototype.DynamicLength.Length);

                        dynamicLength = Prototype.DynamicLengthConverter.ConvertToInt(dynamicLengthValue);
                    }
                }
            }

            return values;
        }

        private bool HasValidFlags(string value)
        {
            bool validStartDelimiter = false;
            bool validEndDelimiter = false;

            if (Prototype.StartFlag == null || value.IndexOf(Prototype.StartFlag.Value) == 0)
            {
                validStartDelimiter = true;
            }

            if (Prototype.EndFlag == null || (value.LastIndexOf(Prototype.EndFlag.Value) ==
                                value.Length - Prototype.EndFlag.Length))
            {
                validEndDelimiter = true;
            }

            return validStartDelimiter && validEndDelimiter;
        }

        public void CustomParse(Action<StringBuilder> parseAction)
        {
            throw new NotImplementedException();
        }

        public string CreateSerialPacket(string text)
        {
            throw new NotImplementedException();
        }
    }
}