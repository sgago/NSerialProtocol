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

        private const int StartFlagParserOrder = 0;
        private const int EndFlagParserOrder = 10;
        private const int FixedLengthParserOrder = 30;


        private INSerialPort SerialPort { get; set; }
        private byte[] StartFlag { get; set; }
        private byte[] EndFlag { get; set; }
        private IFec Fec { get; set; }
        private IByteStuff ByteStuff { get; set; }
        private string InputBuffer { get; set; }

        private List<Tuple<int, IParser>> Parsers { get; set; } = new List<Tuple<int, IParser>>();


        /// <summary>
        /// Represents the method that will handle the MessageReceived event of a NSerialPort
        /// object.
        /// </summary>
        /// <param name="sender">The sender of the event, which is the NSerialPort object.</param>
        /// <param name="e">A SerialMessageReceivedEventArgs object that contains the event
        /// data.</param>
        //public delegate void SerialPacketReceivedEventHandler(object sender, SerialPacketReceivedEventArgs e);

        public delegate void SerialFrameReceivedEventHandler(object sender, SerialFrameReceivedEventArgs e);
        public delegate void SerialFrameErrorEventHandler(object sender, SerialFrameErrorEventArgs e);

        public event SerialFrameReceivedEventHandler SerialFrameReceived;
        public event SerialFrameErrorEventHandler SerialFrameError;

        internal NSerialProtocol(INSerialPort serialPort)
        {
            SerialPort = serialPort;

            SerialPort.DataReceived += SerialPort_DataReceived;
        }


        public NSerialProtocol(string portName = "COM1",
                               int baudRate = 57600,
                               Parity parity = Parity.None,
                               int dataBits = 8,
                               StopBits stopBits = StopBits.One)
            : this(new NSerialPort(portName, baudRate, parity, dataBits, stopBits))
        {

        }

        public void RaiseSerialFrameReceivedEvent(object sender, SerialFrameReceivedEventArgs e)
        {
            SerialFrameReceived?.Invoke(sender, e);
        }

        private void RaiseSerialFrameErrorEvent(object sender, SerialFrameErrorEventArgs e)
        {
            SerialFrameError?.Invoke(sender, e);
        }

        public NSerialProtocol SetFec(IFec fec)
        {
            throw new NotImplementedException();

            //return this;
        }

        public NSerialProtocol SetLengthField()
        {
            throw new NotImplementedException();

            //return this;
        }

        public NSerialProtocol SetByteStuff(IByteStuff byteStuff)
        {
            throw new NotImplementedException();

            //return this;
        }

        public NSerialProtocol SetStartFlag(byte[] startFlag)
        {
            StartFlag = startFlag;

            //return this;

            throw new NotImplementedException();
        }

        public NSerialProtocol SetStartFlag(string startFlag)
        {
            StartFlag = Encoding.Default.GetBytes(startFlag);

            Parsers.Add(new Tuple<int, IParser>(StartFlagParserOrder, new StartFlagParser(startFlag)));

            return this;
        }

        public NSerialProtocol SetEndFlag(byte[] endFlag)
        {
            EndFlag = endFlag;

            //return this;

            throw new NotImplementedException();
        }

        public NSerialProtocol SetEndFlag(string endFlag)
        {
            EndFlag = Encoding.Default.GetBytes(endFlag);

            Parsers.Add(new Tuple<int, IParser>(EndFlagParserOrder, new EndFlagParser(endFlag)));

            return this;
        }

        public NSerialProtocol SetMaximumLength(int length)
        {
            throw new NotImplementedException();

            //return this;
        }

        public NSerialProtocol SetFixedLength(int length)
        {
            throw new NotImplementedException();

            //return this;
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

        /// <summary>
        /// Reads a string up to the specified value in the input buffer.
        /// </summary>
        /// <param name="value">A value that indicates where the read operation stops</param>
        /// <returns>The contents of the input buffer up to the specified value.</returns>
        protected new string ReadTo(string value)
        {
            return ReadTo(value);
        }

        public static byte[] Serialize<T>(T obj)
        {
            byte[] data;
            using (var ms = new MemoryStream())
            {
                Serializer.Serialize(ms, obj);

                data = ms.ToArray();
            }

            return data;
        }

        public static byte[] SerializeWithLengthPrefix<T>(T obj, PrefixStyle prefixStyle = PrefixStyle.Base128)
        {
            byte[] data;
            using (var ms = new MemoryStream())
            {
                Serializer.SerializeWithLengthPrefix(ms, obj, prefixStyle);

                data = ms.ToArray();
            }

            return data;
        }

        public static T Deserialize<T>(byte[] data)
        {
            T serialPacket;

            using (var ms = new MemoryStream(data))
            {
                serialPacket = Serializer.Deserialize<T>(ms);
            }

            return serialPacket;
        }

        public void WritePacket(SerialPacket serialPacket)
        {
            byte[] payloadBytes = Serialize(serialPacket);
            SerialPacket dsp = Deserialize<SerialPacket>(payloadBytes);

            byte[] payloadEcc = Fec?.Compute(payloadBytes) ?? null;

            byte[] payloadLength = BitConverter.GetBytes(payloadBytes.Length);

            byte[] stuffedPayloadBytes = ByteStuff?.Stuff(payloadBytes) ?? payloadBytes;

            byte[] rawPacket = new byte[
                StartFlag?.Length ?? 0 +
                payloadLength?.Length ?? 0 +
                payloadEcc?.Length ?? 0 + 
                stuffedPayloadBytes.Length +
                EndFlag?.Length ?? 0
            ];

            int startFlagIndex = 0;
            int payloadLengthIndex = startFlagIndex + StartFlag.Count();
            int payloadEccIndex = payloadLengthIndex + payloadLength.Count();
            int stuffedPayloadIndex = payloadEccIndex + payloadEcc.Length;
            int endFlagIndex = stuffedPayloadIndex + EndFlag.Length;

            Array.Copy(StartFlag, 0, rawPacket, startFlagIndex, StartFlag.Length);
            Array.Copy(payloadLength, 0, rawPacket, payloadLengthIndex, payloadLength.Length);
            Array.Copy(payloadEcc, 0, rawPacket, payloadEccIndex, payloadEcc.Length);
            Array.Copy(stuffedPayloadBytes, 0, rawPacket, stuffedPayloadIndex, stuffedPayloadBytes.Length);
            Array.Copy(EndFlag, 0, rawPacket, endFlagIndex, EndFlag.Length);

            //Write(rawPacket, 0, rawPacket.Length);
        }

        public SerialPacket ReadPacket()
        {



            throw new NotImplementedException();
        }

        private IList<string> Parse(string data)
        {
            IList<string> results;

            for (int i = 0; i < Parsers.Count - 1; i++)
            {
                Parsers[i].Item2.SetSuccessor(Parsers[i + 1].Item2);
            }

            results = Parsers[0].Item2.Parse(data);

            return results;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender">The sender of the event, which is the BaseSerialPort object.</param>
        /// <param name="e">The SerialDataReceivedEventArgs.</param>
        private void SerialPort_DataReceived(object sender, NSerialDataReceivedEventArgs e)
        {
            string data = string.Empty;

            // Get the received characters
            InputBuffer += e.Data;

            IList<string> frames = Parse(InputBuffer);

            IList<string> errors = GetErrors(InputBuffer, frames);

            // Int is the index of the value in the frame
            // Bool is true if it's a good frame, else it's a bad frame
            // String is the value as a string
            List<Tuple<int, bool, string>> values = new List<Tuple<int, bool, string>>();

            int index = -1;

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
            values = values.OrderBy(x => x.Item1).ToList();


            foreach (Tuple<int, bool, string> value in values)
            {
                if (value.Item2)
                {
                    RaiseSerialFrameReceivedEvent(this, new SerialFrameReceivedEventArgs(value.Item3));
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

        //TODO: Move non-packet tranceive methods to NSerialPort???
        /// <summary>
        ///
        /// </summary>
        /// <param name="text">Text to transmit.</param>
        /// <param name="timeout">
        /// Number of milliseconds to wait for a reply or <c>Timeout.Infinite</c> (-1) to wait indefinitely.
        /// </param>
        /// <param name="retries">Number of retry attempts.</param>
        /// <returns>Received message or empty string if no message was received.</returns>
        /// <example>
        /// <code>
        /// // Basic use
        /// ISerialPacket result = NSerialPort.TranceivePacket("Foo command", 500, 1);
        ///
        /// // Aysnchrounous, GUI-friendly way
        /// ISerialPacket result = await Task.Run(() => NSerialPort.TranceivePacket("Foo command", 200, 3));
        /// </code>
        /// </example>
        //public ISerialPacket TranceivePacket(string text, int timeout = 100, int retries = 0)
        //{
        //    bool gotPacket = false;
        //    ISerialPacket result = null;

        //    // TODO: is this AutoResetEvent used safely?  (No, it should be in (static?) variable above
        //    // and properly disposed of.)
        //    AutoResetEvent packetReceived = new AutoResetEvent(false); // For thread blocking

        //    // Define a temporary MessageReceived event handler to capture a serial message
        //    SerialPacketReceivedEventHandler packetReceivedHandler = (sender, packetReceivedEventArgs) =>
        //    {
        //        result = packetReceivedEventArgs.SerialPacket; // Grab received message from event args
        //        packetReceived.Set();  // Unblock thread
        //    };

        //    // Subscribe to message received event we just created to get messages
        //    PacketReceived += packetReceivedHandler;

        //    do
        //    {
        //        WritePacket(text);
        //        gotPacket = packetReceived.WaitOne(timeout);   // Block until we get a message or timeout
        //    }
        //    while (!gotPacket && --retries > 0);  // Transmit until we get a message back or run out of retries

        //    // Unsubscribe (save RAM, removes possible event problems for delegates with similar signature, etc.)
        //    PacketReceived -= packetReceivedHandler;

        //    return result;
        //}
    }
}
