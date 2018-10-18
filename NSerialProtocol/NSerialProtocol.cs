using ProtoBuf;
using NSerialPort;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSerialProtocol
{
    using global::NSerialProtocol.Attributes;
    using global::NSerialProtocol.EventArgs;
    using global::NSerialProtocol.Extensions;
    using global::NSerialProtocol.SerialFrameParsers;
    using NByteStuff;
    using NFec;
    using NFec.Algorithms;
    using NSerialPort;
    using NSerialPort.EventArgs;
    using SerialPortFix;
    using System.IO.Ports;

    public interface ISerialProtocol
    {

    }

    public class DefaultSerialFrame : SerialFrame
    {
        [StartFlag]
        public string StartFlag { get; set; } = "";

        [FrameMember(0)]
        public string Payload { get; set; }

        [EndFlag]
        public string EndFlag { get; set; } = "\0";
    }

    //[ProtoContract]
    //public class DefaultSerialPacket : SerialPacket
    //{
    //    [ProtoMember(0)]
    //    public string Data;
    //}

    public class NSerialProtocol : NSerialPort, ISerialProtocol
    {
        /*
         * NSerialProtocol will handle OUTSIDE OF PROTOBUF
         * - StartFlag OUTSIDE PROTOBUF
         * - EndFlag OUTSIDE PROTOBUF
         * - Length OUTSIDE PROTOBUF
         * - CRC OUTSIDE PROTOBUF
         * - ByteStuff OUTSIDE PROTOBUF
         * 
         * Calculation order
         * 1. Get serialized data as byte[] (convert to List?)
         * 2. Calculate data's CRC and prepend CRC
         * 3. Calculate data's length and prepend length in front of CRC
         * 4. Byte stuff length, checksum, and data
         * 5. Prepend start flag
         * 6. Append end flag
         */

        private readonly Encoding ExtendedAsciiEncoding = Encoding.GetEncoding(437);

        private const int StartFlagParserOrder = 0;
        private const int EndFlagParserOrder = 10;
        private const int FixedLengthParserOrder = 30;

        private INSerialPort SerialPort { get; set; }
        private string InputBuffer { get; set; }

        private SerialFrame PrototypeFrame { get; set; } = new DefaultSerialFrame();

        //private SerialFrameSerializer SerialFrameSerializer = new SerialFrameSerializer(typeof(DefaultSerialFrame));

        private List<Tuple<int, IParser>> Parsers { get; set; } = new List<Tuple<int, IParser>>();

        /// <summary>
        /// Represents the method that will handle the MessageReceived event of a NSerialPort
        /// object.
        /// </summary>
        /// <param name="sender">The sender of the event, which is the NSerialPort object.</param>
        /// <param name="e">A SerialMessageReceivedEventArgs object that contains the event
        /// data.</param>
        //public delegate void SerialPacketReceivedEventHandler(object sender, SerialPacketReceivedEventArgs e);

        public delegate void SerialFrameParsedEventHandler(object sender, SerialFrameParsedEventArgs e);
        public delegate void SerialFrameErrorEventHandler(object sender, SerialFrameErrorEventArgs e);
        public delegate void SerialFrameReceivedEventHandler(object sender, SerialFrameReceivedEventArgs e);

        public event SerialFrameParsedEventHandler SerialFrameParsed;
        public event SerialFrameErrorEventHandler SerialFrameError;
        public event SerialFrameReceivedEventHandler SerialFrameReceived;

        internal NSerialProtocol(INSerialPort serialPort)
        {
            SerialPort = serialPort;

            SerialPort.DataReceived += SerialPort_DataReceived;
            SerialFrameParsed += NSerialProtocol_SerialFrameParsed;
        }

        public NSerialProtocol(string portName = "COM1",
                               int baudRate = 57600,
                               Parity parity = Parity.None,
                               int dataBits = 8,
                               StopBits stopBits = StopBits.One)
            : this(new NSerialPort(portName, baudRate, parity, dataBits, stopBits))
        {

        }

        public NSerialProtocol SetFec(IFec fec)
        {
            throw new NotImplementedException();
        }

        public NSerialProtocol SetLengthField()
        {
            throw new NotImplementedException();
        }

        public NSerialProtocol SetByteStuff(IByteStuff byteStuff)
        {
            throw new NotImplementedException();
        }

        public NSerialProtocol SetStartFlag(byte[] startFlag)
        {
            Parsers.Add(new Tuple<int, IParser>(StartFlagParserOrder, new StartFlagParser(
                ExtendedAsciiEncoding.GetString(startFlag))));

            SetParserSuccessors();

            return this;
        }

        public NSerialProtocol SetStartFlag(string startFlag)
        {
            return SetStartFlag(Encoding.Default.GetBytes(startFlag));
        }

        public NSerialProtocol SetStartFlag(string startFlag, Encoding encoding)
        {
            return SetStartFlag(encoding.GetBytes(startFlag));
        }

        public NSerialProtocol SetEndFlag(byte[] endFlag)
        {
            throw new NotImplementedException();
        }

        public NSerialProtocol SetEndFlag(string endFlag)
        {
            Parsers.Add(new Tuple<int, IParser>(EndFlagParserOrder, new EndFlagParser(endFlag)));

            SetParserSuccessors();

            return this;
        }

        public NSerialProtocol SetMaximumLength(int length)
        {
            throw new NotImplementedException();
        }

        public NSerialProtocol SetFixedLength(int length)
        {
            throw new NotImplementedException();
        }

        public void SetPrototype<T>() where T : SerialFrame
        {

        }

        public void RaiseSerialFrameParsedEvent(object sender, SerialFrameParsedEventArgs e)
        {
            SerialFrameParsed?.Invoke(sender, e);
        }

        private void RaiseSerialFrameErrorEvent(object sender, SerialFrameErrorEventArgs e)
        {
            SerialFrameError?.Invoke(sender, e);
        }

        private void RaiseSerialFrameReceived(object sender, SerialFrameReceivedEventArgs e)
        {
            SerialFrameReceived?.Invoke(sender, e);
        }

        /// <summary>
        /// Reads a number of bytes from the SerialPort input buffer and writes those
        /// bytes into a byte array at the specified offset.
        /// </summary>
        /// <param name="buffer">The byte array to write the input to.</param>
        /// <param name="offset">The offset in buffer at which to write the bytes.</param>
        /// <param name="count">The maximum number of bytes to read.  Fewer bytes are
        /// read if count is greater than the number of bytes in the input buffer.</param>
        /// <returns>The number of bytes read.</returns>
        protected new int Read(byte[] buffer, int offset, int count)
        {
            return Read(buffer, offset, count);
        }

        /// <summary>
        /// Reads a number of characters from the SerialPort input buffer and writes them into
        /// an array of characters at a given offset.
        /// </summary>
        /// <param name="buffer">The character array to write the input to.</param>
        /// <param name="offset">The offset in buffer at which to write the characters</param>
        /// <param name="count">The maximum number of bytes to read.  Fewer bytes are
        /// read if count is greater than the number of bytes in the input buffer.</param>
        /// <returns>The number of bytes read.</returns>
        protected new int Read(char[] buffer, int offset, int count)
        {
            return Read(buffer, offset, count);
        }

        /// <summary>
        /// Synchronously reads one byte from the SerialPort input buffer.
        /// </summary>
        /// <returns>The byte, cast to an Int32 or -1 if the end of the stream has been
        /// read.</returns>
        protected new int ReadByte()
        {
            return ReadByte();
        }

        /// <summary>
        /// Synchronously reads one character from the SerialPort input buffer.
        /// </summary>
        /// <returns>The character that was read.</returns>
        protected new int ReadChar()
        {
            return ReadChar();
        }

        /// <summary>
        /// Reads all immediately available bytes, based on the encoding, in both the stream
        /// and the input buffer of the SerialPOrt object.
        /// </summary>
        /// <returns>The contents of the stream and the input buffer of the SerialPort object.</returns>
        protected new string ReadExisting()
        {
            return ReadExisting();
        }

        /// <summary>
        /// Reads up to the NewLine value in the input buffer.
        /// </summary>
        /// <returns>The contents of the input buffer up to the first occurrence of a
        /// NewLine value.</returns>
        protected new string ReadLine()
        {
            return ReadLine();
        }

        // TODO: Need to hide other Read and ReadAsync methods!!

        /// <summary>
        /// Reads a string up to the specified value in the input buffer.
        /// </summary>
        /// <param name="value">A value that indicates where the read operation stops</param>
        /// <returns>The contents of the input buffer up to the specified value.</returns>
        protected new string ReadTo(string value)
        {
            return ReadTo(value);
        }

        private IList<string> Parse(string data)
        {
            IList<string> results;

            results = Parsers.First().Item2.Parse(data);

            return results;
        }

        private void SetParserSuccessors()
        {
            for (int i = 0; i < Parsers.Count - 1; i++)
            {
                Parsers[i].Item2.SetSuccessor(Parsers[i + 1].Item2);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender">The sender of the event, which is the BaseSerialPort object.</param>
        /// <param name="e">The SerialDataReceivedEventArgs.</param>
        private void SerialPort_DataReceived(object sender, NSerialDataReceivedEventArgs e)
        {
            int index = -1;
            string data = string.Empty;

            // Get the received characters
            InputBuffer += e.Data;

            // Parse the good frames from our InputBuffer
            IList<string> frames = Parse(InputBuffer);

            // Parse the bad frames and garbage data from our InputBuffer
            IList<string> errors = GetErrors(InputBuffer, frames);

            // Int is the index of the value in the frame
            // Bool is true if it's a good frame, else it's a bad frame
            // String is the value as a string
            // Frame + Errors = values for lack of a better word
            List<Tuple<int, bool, string>> values = new List<Tuple<int, bool, string>>();

            foreach (string frame in frames)
            {
                index = InputBuffer.IndexOf(frame);

                values.Add(new Tuple<int, bool, string>(index, true, frame));
            }

            foreach (string error in errors)
            {
                index = InputBuffer.IndexOf(error);

                values.Add(new Tuple<int, bool, string>(index, false, error));
            }

            // Sort the frames and errors based on their index in the received data
            // This will cause frame and error events to be raised in the order
            // they were received.
            values = values.OrderBy(x => x.Item1).ToList();


            foreach (Tuple<int, bool, string> value in values)
            {
                if (value.Item2)
                {
                    RaiseSerialFrameParsedEvent(this, new SerialFrameParsedEventArgs(value.Item3));
                }
                else
                {
                    RaiseSerialFrameErrorEvent(this, new SerialFrameErrorEventArgs(value.Item3));
                }
            }

            if (values.Count > 0)
            {
                ClearInputBuffer(InputBuffer, values);
            }
        }

        private void NSerialProtocol_SerialFrameParsed(object sender, SerialFrameParsedEventArgs e)
        {
            //SerialFrame serialFrame = SerialFrameSerializer.Deserialize(PrototypeFrame.GetType(),
            //    ExtendedAsciiEncoding.GetBytes(e.Frame));

            //RaiseSerialFrameReceived(this, new SerialFrameReceivedEventArgs(serialFrame));
        }

        private IList<string> GetErrors(string inputBuffer, IList<string> frames)
        {
            return inputBuffer.Split(frames.ToArray(), StringSplitOptions.RemoveEmptyEntries);
        }

        private void ClearInputBuffer(string inputBuffer, List<Tuple<int, bool, string>> values)
        {
            Tuple<int, bool, string> lastValue = values.Last();

            // Is the last value a good frame?
            if (lastValue.Item2)
            {
                // It's a good frame, drop the entire buffer
                inputBuffer = string.Empty;
            }
            else
            {
                // The last value is a potentitally incomplete frame
                // Drop everything up to, but not including, the last value received
                inputBuffer = inputBuffer.RemoveToLast(values[values.Count - 1].Item3);
            }
        }

        public void WriteFrame(SerialFrame serialFrame)
        {
            throw new NotImplementedException();
        }

        public SerialFrame ReadFrame()
        {
            throw new NotImplementedException();
        }

        public SerialFrame TranceiveFrame(SerialFrame serialFrame, int timeout = 100, int retries = 0)
        {
            throw new NotImplementedException();
        }

        public void WritePacket(SerialPacket serialPacket)
        {
            throw new NotImplementedException();
        }

        public SerialPacket ReadPacket()
        {
            throw new NotImplementedException();
        }

        public SerialPacket TranceivePacket(SerialPacket serialPacket, int timeout = 100, int retries = 0)
        {
            throw new NotImplementedException();
        }
    }
}
