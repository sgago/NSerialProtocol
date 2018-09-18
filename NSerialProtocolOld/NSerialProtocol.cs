namespace NSerialProtocol
{
    using EventArgs;
    using NSerialPort;
    using SerialPortFix;
    using System;
    using System.ComponentModel;
    using System.IO.Ports;
    using System.Text;
    using System.Threading;

    public interface ISerialProtocol : ISerialPort, INotifyPropertyChanged
    {
        ISerialPacketPrototype SerialPackets { get; }

        /// <summary>
        /// Gets or sets the input buffer for this serial port resource used
        /// to construct serial packets.
        /// </summary>
        StringBuilder InputBuffer { get; set; }

        event NSerialProtocol.SerialPacketReceivedEventHandler PacketReceived;

        string TranceiveLine(string text, int timeout = 100, int retries = 0);
        ISerialPacket TranceivePacket(string text, int timeout = 100, int retries = 0);
    }


    public class NSerialProtocol : NSerialPort, ISerialProtocol
    {
        private ISerialPacketPrototype serialPackets;

        /// <summary>
        /// StringBuilder for concatenating serial data and parsing messages.
        /// </summary>
        private StringBuilder inputBuffer = new StringBuilder();

        public ISerialPacketPrototype SerialPackets
        {
            get
            {
                return serialPackets;
            }

            private set
            {
                serialPackets = value;
            }
        }

        /// <summary>
        /// Gets or sets the input buffer for this serial port resource used
        /// to construct serial packets.
        /// </summary>
        public StringBuilder InputBuffer
        {
            get
            {
                return inputBuffer;
            }

            set
            {
                inputBuffer = value;
            }
        }

        /// <summary>
        /// Represents the method that will handle the MessageReceived event of a NSerialPort
        /// object.
        /// </summary>
        /// <param name="sender">The sender of the event, which is the NSerialPort object.</param>
        /// <param name="e">A SerialMessageReceivedEventArgs object that contains the event
        /// data.</param>
        public delegate void SerialPacketReceivedEventHandler(object sender, SerialPacketReceivedEventArgs e);

        /// <summary>
        /// Indicates that a complete serial message has been received through a port represented by the
        /// SerialPort object.
        /// </summary>
        public event SerialPacketReceivedEventHandler PacketReceived;


        internal NSerialProtocol(ISerialPort serialPort,
                                 ISerialPacketPrototype serialPackets,
                                 string portName = "COM1",
                                 int baudRate = 57600,
                                 Parity parity = Parity.None,
                                 int dataBits = 8,
                                 StopBits stopBits = StopBits.None)
            : base(serialPort, portName, baudRate, parity, dataBits, stopBits)
        {
            SerialPackets = serialPackets;
        }


        public NSerialProtocol(string portName = "COM1",
                               int baudRate = 57600,
                               Parity parity = Parity.None,
                               int dataBits = 8,
                               StopBits stopBits = StopBits.None)
            : this(new SerialPortFix(), new SerialPacketPrototype(),
                   portName, baudRate, parity, dataBits, stopBits)
        {

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


        public void WritePacket(string text)
        {
            Write(SerialPackets.CreateSerialPacket(text));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender">The sender of the event, which is the BaseSerialPort object.</param>
        /// <param name="e">The SerialDataReceivedEventArgs.</param>
        private void SerialPortModel_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string data = string.Empty;

            // Read characters from SerialPort IOStream to our input buffer
            InputBuffer.Append(ReadExisting());

            // Parse any packets
            SerialPackets.Parse(InputBuffer);
        }

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
        /// string result = NSerialPort.TranceiveLine("Foo command", 500, 1);
        ///
        /// // Aysnchrounous, GUI-friendly way
        /// string result = await Task.Run(() => NSerialPort.TranceiveLine("Foo command", 200, 3));
        /// </code>
        /// </example>
        public string TranceiveLine(string text, int timeout = 100, int retries = 0)
        {
            // TODO: Move non-packet based Tranceives to NSerialPort????

            throw new NotImplementedException();
        }

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
        public ISerialPacket TranceivePacket(string text, int timeout = 100, int retries = 0)
        {
            bool gotPacket = false;
            ISerialPacket result = null;

            // TODO: is this AutoResetEvent used safely?  (No, it should be in (static?) variable above
            // and properly disposed of.)
            AutoResetEvent packetReceived = new AutoResetEvent(false); // For thread blocking

            // Define a temporary MessageReceived event handler to capture a serial message
            SerialPacketReceivedEventHandler packetReceivedHandler = (sender, packetReceivedEventArgs) =>
            {
                result = packetReceivedEventArgs.SerialPacket; // Grab received message from event args
                packetReceived.Set();  // Unblock thread
            };

            // Subscribe to message received event we just created to get messages
            PacketReceived += packetReceivedHandler;

            do
            {
                WritePacket(text);
                gotPacket = packetReceived.WaitOne(timeout);   // Block until we get a message or timeout
            }
            while (!gotPacket && --retries > 0);  // Transmit until we get a message back or run out of retries

            // Unsubscribe (save RAM, removes possible event problems for delegates with similar signature, etc.)
            PacketReceived -= packetReceivedHandler;

            

            return result;
        }
    }
}
