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

    public interface ISerialProtocol
    {

    }

    [Fec()]
    [ByteStuff()]
    public class DefaultSerialFrame : SerialFrame
    {
        [StartFlag]
        public char StartFlag { get; set; } = '|';

        [FrameMember(1)]
        public string Payload { get; set; }

        [EndFlag]
        public char EndFlag { get; set; } = '\n';
    }

    //[ProtoContract]
    //public class DefaultSerialPacket : SerialPacket
    //{
    //    [ProtoMember(0)]
    //    public string Data;
    //}

    public class NSerialProtocol : ISerialProtocol
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
        private const int ExtendedAsciiCodepage = 437;
        private readonly Encoding ExtendedAsciiEncoding = Encoding.GetEncoding(ExtendedAsciiCodepage);

        private const int FlagParserOrder = 0;
        private const int FixedLengthParserOrder = 30;

        private ISerialPort SerialPort { get; set; }
        private string InputBuffer { get; set; }

        private ISerialFrame PrototypeFrame { get; set; } = new DefaultSerialFrame();

        private IFrameSerializer SerialFrameSerializer { get; set; }

        private List<Tuple<int, IFrameParser>> Parsers { get; set; } = new List<Tuple<int, IFrameParser>>();

        internal EventRouter FrameReceivedEventRouter;

        /// <summary>
        /// Represents the method that will handle the MessageReceived event of a NSerialPort
        /// object.
        /// </summary>
        /// <param name="sender">The sender of the event, which is the NSerialPort object.</param>
        /// <param name="e">A SerialMessageReceivedEventArgs object that contains the event
        /// data.</param>
        public delegate void SerialFrameParsedEventHandler(object sender, SerialFrameParsedEventArgs e);
        public delegate void SerialFrameReceivedEventHandler(object sender, SerialFrameReceivedEventArgs e);
        public delegate void SerialFrameErrorEventHandler(object sender, SerialFrameErrorEventArgs e);
        public delegate void SerialPacketReceivedEventHandler(object sender, SerialPacketReceivedEventArgs e);

        public event SerialFrameParsedEventHandler SerialFrameParsed;
        public event SerialFrameReceivedEventHandler SerialFrameReceived;
        public event SerialFrameErrorEventHandler SerialFrameError;
        public event SerialPacketReceivedEventHandler SerialPacketReceived;
        
        internal NSerialProtocol(ISerialPort serialPort, IFrameSerializer serializer)
        {
            SerialPort = serialPort;
            SerialFrameSerializer = serializer;

            SerialPort.DataReceived += SerialPort_DataReceived;
            SerialFrameParsed += NSerialProtocol_SerialFrameParsed;

            FrameReceivedEventRouter = new EventRouter(this);
        }

        public NSerialProtocol(string portName = "COM1",
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

        //public NSerialProtocol SetFlags(byte[] endFlag, byte[] startFlag = null)
        //{
        //    throw new NotImplementedException();
        //}

        public NSerialProtocol SetFlags(string endFlag, string startFlag = "")
        {
            Parsers.Add(new Tuple<int, IFrameParser>(FlagParserOrder, new FlagParser(endFlag, startFlag)));

            SetParserSuccessors();

            return this;
        }

        //public NSerialProtocol SetFec(IFec fec)
        //{
        //    throw new NotImplementedException();
        //}

        //public NSerialProtocol SetLengthField()
        //{
        //    throw new NotImplementedException();
        //}

        //public NSerialProtocol SetByteStuff(IByteStuff byteStuff)
        //{
        //    throw new NotImplementedException();
        //}

        //public NSerialProtocol SetMaximumLength(int length)
        //{
        //    throw new NotImplementedException();
        //}

        public void SetFramePrototype(Type type)
        {
            TypeAccessor typeAccessor = TypeAccessor.Create(type);
            ObjectAccessor objectAccessor = ObjectAccessor.Create(typeAccessor.CreateNew());

            throw new NotImplementedException();
        }

        public void SetFramePrototype<T>() where T : ISerialFrame
        {
            SetFramePrototype(typeof(T));
        }

        private void RaiseSerialFrameParsedEvent(object sender, SerialFrameParsedEventArgs e)
        {
            SerialFrameParsed?.Invoke(sender, e);
        }

        private void RaiseSerialFrameReceivedEvent(object sender, SerialFrameReceivedEventArgs e)
        {
            SerialFrameReceived?.Invoke(sender, e);
        }

        private void RaiseSerialFrameErrorEvent(object sender, SerialFrameErrorEventArgs e)
        {
            SerialFrameError?.Invoke(sender, e);
        }

        private void RaiseSerialPacketReceivedEvent(object sender, SerialPacketReceivedEventArgs e)
        {
            SerialPacketReceived?.Invoke(sender, e);
        }

        private IList<string> Parse(string data)
        {
            IList<string> results;

            results = Parsers.First().Item2.Parse(data);

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

        private void NSerialProtocol_SerialFrameParsed(object sender, SerialFrameParsedEventArgs e)
        {
            object receivedFrame =
                SerialFrameSerializer.Deserialize(PrototypeFrame.GetType(), e.Frame);

            RaiseSerialFrameReceivedEvent(this, new SerialFrameReceivedEventArgs(receivedFrame));
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

        public void WriteFrame(ISerialFrame serialFrame)
        {
            throw new NotImplementedException();
        }

        public void WriteFrame(string payload)
        {
            throw new NotImplementedException();
        }

        // FIXME: Use ReadFrame in the TranceiveFrame method
        public SerialFrame ReadFrame()
        {
            throw new NotImplementedException();
        }

        public T ReadFrame<T>()
        {
            throw new NotImplementedException();
        }

        public SerialFrame TranceiveFrame(SerialFrame serialFrame, int timeout = Timeout.Infinite, int retries = 0)
        {
            throw new NotImplementedException();
        }

        public SerialFrame TranceiveFrame(string payload, int timeout = Timeout.Infinite, int retries = 0)
        {
            throw new NotImplementedException();
        }

        public void WritePacket(ISerialPacket serialPacket)
        {
            throw new NotImplementedException();
        }

        public ISerialPacket ReadPacket()
        {
            throw new NotImplementedException();
        }

        public ISerialPacket TranceivePacket(SerialPacket serialPacket, int timeout = Timeout.Infinite, int retries = 0)
        {
            throw new NotImplementedException();
        }
    }
}
