using global::NSerialProtocol.Attributes;
using global::NSerialProtocol.EventArgs;
using global::NSerialProtocol.Extensions;
using global::NSerialProtocol.FrameParsers;
using NByteStuff;
using NFec;
using NSerialPort.EventArgs;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;

namespace NSerialProtocol
{
    using FastMember;
    using NByteStuff.Algorithms;
    using NFec.Algorithms;
    using NSerialPort;
    using System.Linq.Expressions;

    [Fec()]
    [ByteStuff()]
    public class DefaultSerialFrame : SerialFrame
    {
        [StartFlag]
        public char StartFlag { get; set; } = '|';

        [SerialPacket(1)]
        public string Payload { get; set; }

        [EndFlag]
        public char EndFlag { get; set; } = '\n';
    }

    public class SerialProtocol : ISerialProtocol
    {
        /*
        * SerialProtocol will handle OUTSIDE OF PROTOBUF
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
        private const int FlagParserOrder = 0;
        private const int FixedLengthParserOrder = 30;
        private const int ExtendedAsciiCodepage = 437;

        private readonly Encoding ExtendedAsciiEncoding
            = Encoding.GetEncoding(ExtendedAsciiCodepage);

        private ISerialPort SerialPort { get; set; }
        private string InputBuffer { get; set; }

        private readonly AutoResetEvent FrameReceivedAutoResetEvent
            = new AutoResetEvent(false);

        private object PrototypeFrame { get; set; }
            = new DefaultSerialFrame();

        private IFrameSerializer FrameSerializer { get; set; }

        private List<Tuple<int, IFrameParser>> Parsers { get; set; }
            = new List<Tuple<int, IFrameParser>>();

        public EventRouter FrameReceivedEventRouter { get; set; }
        

        private TypeAccessor PrototypeFrameTypeAccessor { get; set; }
        private ObjectAccessor PrototypeFrameObjectAccessor { get; set; }
        private string PrototypeFramePayloadMemberName { get; set; }


        /// <summary>
        /// Represents the method that will handle the MessageReceived event of a NSerialPort
        /// object.
        /// </summary>
        /// <param name="sender">The sender of the event, which is the NSerialPort object.</param>
        /// <param name="e">A SerialMessageReceivedEventArgs object that contains the event
        /// data.</param>
        public delegate void FrameParsedEventHandler(object sender, SerialFrameParsedEventArgs e);
        public delegate void FrameReceivedEventHandler(object sender, SerialFrameReceivedEventArgs e);
        public delegate void FrameErrorEventHandler(object sender, SerialFrameErrorEventArgs e);

        public delegate void PacketReceivedEventHandler(object sender, SerialPacketReceivedEventArgs e);

        public event FrameParsedEventHandler OnFrameParsed;
        public event FrameReceivedEventHandler OnFrameReceived;
        public event FrameErrorEventHandler OnFrameError;
        public event PacketReceivedEventHandler OnPacketReceived;
        
        internal SerialProtocol(ISerialPort serialPort, IFrameSerializer serializer)
        {
            SerialPort = serialPort;
            FrameSerializer = serializer;

            SerialPort.DataReceived += SerialPort_DataReceived;
            OnFrameParsed += SerialProtocol_SerialFrameParsed;
            OnFrameReceived += SerialProtocol_OnFrameReceived;

            FrameReceivedEventRouter = new FrameReceivedEventRouter(this);

            SetFramePrototype<DefaultSerialFrame>();
        }

        public SerialProtocol(string portName = "COM1",
                               int baudRate = 57600,
                               Parity parity = Parity.None,
                               int dataBits = 8,
                               StopBits stopBits = StopBits.One)
            : this(new NSerialPort(portName, baudRate, parity, dataBits, stopBits),
                   new FrameSerializer())
        {
            // Yes, this violates SOLID.  However, this simplifies the instantiation
            // for users.
        }

        // TODO: Provide overloads for the SetFlags method
        public ISerialProtocol SetFlags(string endFlag, string startFlag = "")
        {
            Parsers.Add(new Tuple<int, IFrameParser>(FlagParserOrder, new FlagParser(endFlag, startFlag)));

            SetParserSuccessors();

            return this;
        }

        //public SerialProtocol SetFec(IFec fec)
        //{
        //    throw new NotImplementedException();
        //}

        //public SerialProtocol SetLengthField()
        //{
        //    throw new NotImplementedException();
        //}

        //public SerialProtocol SetByteStuff(IByteStuff byteStuff)
        //{
        //    throw new NotImplementedException();
        //}

        //public SerialProtocol SetMaximumLength(int length)
        //{
        //    throw new NotImplementedException();
        //}

        public ISerialProtocol SetFramePrototype(Type type)
        {
            PrototypeFrameTypeAccessor = TypeAccessor.Create(type);

            PrototypeFrame = PrototypeFrameTypeAccessor.CreateNew();

            PrototypeFrameObjectAccessor = ObjectAccessor.Create(PrototypeFrame);

            PrototypeFramePayloadMemberName = PrototypeFrameTypeAccessor
                .GetMembers()
                .Where(x => x.IsDefined(typeof(PayloadAttribute)))
                .FirstOrDefault()
                .Name;

            return this;
        }

        public ISerialProtocol SetFramePrototype<TFrame>() where TFrame : ISerialFrame
        {
            SetFramePrototype(typeof(TFrame));

            return this;
        }

        private void RaiseSerialFrameParsedEvent(object sender, SerialFrameParsedEventArgs e)
        {
            OnFrameParsed?.Invoke(sender, e);
        }

        private void RaiseSerialFrameReceivedEvent(object sender, SerialFrameReceivedEventArgs e)
        {
            OnFrameReceived?.Invoke(sender, e);
        }

        private void RaiseSerialFrameErrorEvent(object sender, SerialFrameErrorEventArgs e)
        {
            OnFrameError?.Invoke(sender, e);
        }

        private void RaiseSerialPacketReceivedEvent(object sender, SerialPacketReceivedEventArgs e)
        {
            OnPacketReceived?.Invoke(sender, e);
        }

        private IList<string> Parse(string data)
        {
            IList<string> results;

            // TODO: Not sure I like this if statement
            // Is there a better way?
            if (Parsers.Count > 0)
            {
                results = Parsers.First().Item2.Parse(data);
            }
            else
            {
                results = new List<string> { data };
            }

            return results;
        }

        /// <summary>
        /// Sets the next parser in the parser chain.
        /// </summary>
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

        private void SerialProtocol_SerialFrameParsed(object sender, SerialFrameParsedEventArgs e)
        {
            object receivedFrame =
                FrameSerializer.Deserialize(PrototypeFrame.GetType(), e.Frame);

            RaiseSerialFrameReceivedEvent(this, new SerialFrameReceivedEventArgs(receivedFrame));
        }

        private void SerialProtocol_OnFrameReceived(object sender, SerialFrameReceivedEventArgs e)
        {
            object serialPacket;

            if (e.SerialFrame != null)
            {
                serialPacket = ObjectAccessor.Create(e.SerialFrame)[PrototypeFramePayloadMemberName];

                RaiseSerialPacketReceivedEvent(this, new SerialPacketReceivedEventArgs(serialPacket));
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

        /// <summary>
        /// Serializes a frame and writes the bytes to the serial port.
        /// </summary>
        /// <param name="serialFrame">The serial frame to serialize and write to the port.</param>
        public void WriteFrame(ISerialFrame serialFrame)
        {
            byte[] serializedFrame = FrameSerializer.Serialize(serialFrame);

            SerialPort.Write(serializedFrame, 0, serializedFrame.Count());
        }

        public void WriteFrame<TPayload>(TPayload payload)
        {
            PrototypeFrameObjectAccessor[PrototypeFramePayloadMemberName] = payload;

            WriteFrame(PrototypeFrame as ISerialFrame);
        }

        // FIXME: Use ReadFrame in the TranceiveFrame method
        public object ReadFrame(int timeout = Timeout.Infinite)
        {
            object result = null;

            // TODO: Can this be made more efficient on the CPU?
            // Define a temporary LineReceived event handler to capture a serial message
            void frameReceivedHandler(object sender, SerialFrameReceivedEventArgs frameReceivedEventArgs)
            {
                result = frameReceivedEventArgs.SerialFrame; // Grab received message from event args
                FrameReceivedAutoResetEvent.Set();  // Unblock thread
            }

            // Subscribe to message received event we just created to get messages
            OnFrameReceived += frameReceivedHandler;

            // Block until we get a message or timeout
            FrameReceivedAutoResetEvent.WaitOne(timeout);

            // Unsubscribe (save RAM, removes possible event problems
            // for delegates with similar signature, etc.)
            OnFrameReceived -= frameReceivedHandler;

            return result;
        }

        public object ReadFrame(TimeSpan timeout)
        {
            return ReadFrame(timeout.Milliseconds);
        }

        public TFrame ReadFrame<TFrame>(int timeout = Timeout.Infinite) where TFrame : ISerialFrame
        {
            return (TFrame)ReadFrame(timeout);
        }

        public TFrame ReadFrame<TFrame>(TimeSpan timeout) where TFrame : ISerialFrame
        {
            return (TFrame)ReadFrame(timeout);
        }

        public object TranceiveFrame(ISerialFrame serialFrame, int timeout = Timeout.Infinite, int retries = 0)
        {
            object receivedFrame = false;

            do
            {
                WriteFrame(serialFrame);

                // Block until we get a message or timeout
                receivedFrame = ReadFrame(timeout);
            }
            while (receivedFrame == null && --retries > 0);  // Transmit until we get a message back or run out of retries

            return receivedFrame;
        }

        public object TranceiveFrame(ISerialFrame serialFrame, TimeSpan timeout, int retries = 0)
        {
            return TranceiveFrame(serialFrame, timeout.Milliseconds, retries);
        }

        public TFrame TranceiveFrame<TFrame>(ISerialFrame serialFrame, int timeout = Timeout.Infinite, int retries = 0)
            where TFrame : ISerialFrame
        {
            return (TFrame)TranceiveFrame(serialFrame, timeout, retries);
        }

        public TFrame TranceiveFrame<TFrame>(ISerialFrame serialFrame, TimeSpan timeout, int retries = 0)
            where TFrame : ISerialFrame
        {
            return (TFrame)TranceiveFrame(serialFrame, timeout, retries);
        }

        public void WritePacket(ISerialPacket serialPacket)
        {
            throw new NotImplementedException();
        }

        public object ReadPacket(int timeout = Timeout.Infinite)
        {
            object packet = null;
            object frame = ReadFrame(timeout);

            if (frame != null)
            {
                packet = ObjectAccessor.Create(frame)[PrototypeFramePayloadMemberName];
            }

            return packet;
        }

        public object ReadPacket(TimeSpan timeout)
        {
            return ReadPacket(timeout.Milliseconds);
        }

        public TPacket ReadPacket<TPacket>(int timeout = Timeout.Infinite) where TPacket : ISerialPacket
        {
            return (TPacket)ReadPacket(timeout);
        }

        public TPacket ReadPacket<TPacket>(TimeSpan timeout) where TPacket : ISerialPacket
        {
            return (TPacket)ReadPacket(timeout.Milliseconds);
        }

        public object TranceivePacket(ISerialPacket serialPacket, int timeout = Timeout.Infinite, int retries = 0)
        {
            object receivedPacket = false;

            do
            {
                WritePacket(serialPacket);

                // Block until we get a message or timeout
                receivedPacket = ReadPacket(timeout);
            }
            while (receivedPacket == null && --retries > 0);  // Transmit until we get a message back or run out of retries

            return receivedPacket;
        }

        public TPacket TranceivePacket<TPacket>(ISerialPacket serialPacket, int timeout = Timeout.Infinite, int retries = 0)
            where TPacket : ISerialPacket
        {
            return (TPacket)TranceivePacket(serialPacket, timeout, retries);
        }
    }
}
