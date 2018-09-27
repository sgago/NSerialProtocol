namespace NSerialPort
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.IO.Ports;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using global::NSerialPort.EventArgs;
    using NativeMethods;
    using SerialPortFix;

    /// <summary>
    /// Represents a NSerialPort resource.
    /// </summary>
    public class NSerialPort : INSerialPort, INotifyPropertyChanged, IDisposable
    {
        /// <summary>
        /// Gets or sets the serial port object that is being wrapped.
        /// </summary>
        private ISerialPort SerialPort { get; set; }

        /// <summary>
        /// Gets or sets the input buffer to capture characters from
        /// the serial port.
        /// </summary>
        private string InputBuffer { get; set; } = "";

        /// <summary>
        /// Gets an AutoResetEvent which is used to block until a line is received on
        /// the serial port.
        /// </summary>
        /// <see>TranceiveLine method.</see>
        private AutoResetEvent LineReceivedAutoResetEvent { get; } = new AutoResetEvent(false);

        /// <summary>
        /// Represents a method that will handle the DataReceived event when
        /// a line is read from the serial port.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A SerialDAtaReceivedEventArgs that contains the event data.</param>
        public delegate void NSerialDataReceivedEventHandler(object sender, NSerialDataReceivedEventArgs e);

        /// <summary>
        /// Represents a method that will handle the LineReceived event when
        /// a line is read from the serial port.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A SerialLineReceivedEventArgs that contains the event data.</param>
        public delegate void NSerialLineReceivedEventHandler(object sender, NSerialLineReceivedEventArgs e);

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Indicates that data has been received through a port represented by the SerialPort
        /// object.
        /// </summary>
        public event NSerialDataReceivedEventHandler DataReceived;

        /// <summary>
        /// Indicates that an error has ocurred with a port represented by a SerialPort object.
        /// </summary>
        public event SerialErrorReceivedEventHandler ErrorReceived;

        /// <summary>
        /// Indicates that a non-data signal event has occurred on the port represented
        /// by the SerialPort object.
        /// </summary>
        public event SerialPinChangedEventHandler PinChanged;

        /// <summary>
        /// Indicates that a line has been received on the serial port.
        /// </summary>
        public event NSerialLineReceivedEventHandler LineReceived;

        /// <summary>
        /// 
        /// </summary>
        event NSerialLineReceivedEventHandler INSerialPort.LineReceived
        {
            add
            {
                LineReceived += value;
            }

            remove
            {
                LineReceived -= value;
            }
        }

        /// <summary>
        /// Gets the underlying Stream object for a SerialPort object.
        /// </summary>
        public Stream BaseStream
        {
            get
            {
                return SerialPort.BaseStream;
            }
        }

        /// <summary>
        /// Gets or sets the serial baud rate.
        /// </summary>
        public int BaudRate
        {
            get
            {
                return SerialPort.BaudRate;
            }

            set
            {
                SerialPort.BaudRate = value;
                RaisePropertyChangedEvent(nameof(BaudRate));
            }
        }


        /// <summary>
        /// Gets or sets the break signal state.
        /// </summary>
        public bool BreakState
        {
            get
            {
                return SerialPort.BreakState;
            }

            set
            {
                SerialPort.BreakState = value;
                RaisePropertyChangedEvent(nameof(BreakState));
            }
        }

        /// <summary>
        /// Gets the number of bytes of data in the receive buffer.
        /// </summary>
        public int BytesToRead
        {
            get
            {
                return SerialPort.BytesToRead;
            }
        }

        /// <summary>
        /// Gets the number of bytes of data in the send buffer.
        /// </summary>
        public int BytesToWrite
        {
            get
            {
                return SerialPort.BytesToWrite;
            }
        }

        /// <summary>
        /// Gets the state of the Carrier Detect line for the port.
        /// </summary>
        public bool CDHolding
        {
            get
            {
                return SerialPort.CDHolding;
            }
        }

        /// <summary>
        /// Gets the state of the Clear-to-Send line.
        /// </summary>
        public bool CtsHolding
        {
            get
            {
                return SerialPort.CtsHolding;
            }
        }

        /// <summary>
        /// Gets or sets the standard length of data bits per byte.
        /// </summary>
        public int DataBits
        {
            get
            {
                return SerialPort.DataBits;
            }

            set
            {
                SerialPort.DataBits = value;
                RaisePropertyChangedEvent(nameof(DataBits));
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether null bytes are ignored when transmitted
        /// between the port and the receive buffer.
        /// </summary>
        public bool DiscardNull
        {
            get
            {
                return SerialPort.DiscardNull;
            }

            set
            {
                SerialPort.DiscardNull = value;
                RaisePropertyChangedEvent(nameof(DiscardNull));
            }
        }

        /// <summary>
        /// Gets the state of the Data Set Ready (DSR) signal.
        /// </summary>
        public bool DsrHolding
        {
            get
            {
                return SerialPort.DsrHolding;
            }
        }

        /// <summary>
        /// Gets or sets a value that enables the Data Terminal Ready (DTR) signal during
        /// serial communication.
        /// </summary>
        public bool DtrEnable
        {
            get
            {
                return SerialPort.DtrEnable;
            }

            set
            {
                SerialPort.DtrEnable = value;
                RaisePropertyChangedEvent(nameof(DtrEnable));
            }
        }

        /// <summary>
        /// Gets or sets the byte encoding for pre- and post-transmission conversion of text.
        /// </summary>
        public Encoding Encoding
        {
            get
            {
                return SerialPort.Encoding;
            }

            set
            {
                SerialPort.Encoding = value;
                RaisePropertyChangedEvent(nameof(Encoding));
            }
        }

        /// <summary>
        /// Gets or sets the handshaking protocol for serial port transmission of data using
        /// a value from Handshake.
        /// </summary>
        public Handshake Handshake
        {
            get
            {
                return SerialPort.Handshake;
            }

            set
            {
                SerialPort.Handshake = value;
                RaisePropertyChangedEvent(nameof(Handshake));
            }
        }

        /// <summary>
        /// Gets a value indicating the open or closed status of the SerialPort object.
        /// </summary>
        public bool IsOpen
        {
            get
            {
                return SerialPort.IsOpen;
            }
        }

        /// <summary>
        /// Gets or sets the value used to interpret the end of a call to the ReadLine and WriteLine
        /// methods.
        /// </summary>
        public string NewLine
        {
            get
            {
                return SerialPort.NewLine;
            }

            set
            {
                SerialPort.NewLine = value;
                RaisePropertyChangedEvent(nameof(NewLine));
            }
        }

        /// <summary>
        /// Gets or sets the parity-checking protocol.
        /// </summary>
        public Parity Parity
        {
            get
            {
                return SerialPort.Parity;
            }

            set
            {
                SerialPort.Parity = value;
                RaisePropertyChangedEvent(nameof(Parity));
            }
        }

        /// <summary>
        /// Gets or sets the byte that replaces invalid bytes in a data stream when a parity error
        /// occurs.
        /// </summary>
        public byte ParityReplace
        {
            get
            {
                return SerialPort.ParityReplace;
            }

            set
            {
                SerialPort.ParityReplace = value;
                RaisePropertyChangedEvent(nameof(ParityReplace));
            }
        }

        /// <summary>
        /// Gets or sets the port for communications, including but not limited to all available
        /// COM ports.
        /// </summary>
        public string PortName
        {
            get
            {
                return SerialPort.PortName;
            }

            set
            {
                SerialPort.PortName = value;
                RaisePropertyChangedEvent(nameof(PortName));
            }
        }

        /// <summary>
        /// Gets or sets the size of the SerialPort input buffer.
        /// </summary>
        public int ReadBufferSize
        {
            get
            {
                return SerialPort.ReadBufferSize;
            }

            set
            {
                SerialPort.ReadBufferSize = value;
                RaisePropertyChangedEvent(nameof(ReadBufferSize));
            }
        }

        /// <summary>
        /// Gets or sets the number of milliseconds before a time-out occurs when a read operation
        /// does not finish.
        /// </summary>
        public int ReadTimeout
        {
            get
            {
                return SerialPort.ReadTimeout;
            }

            set
            {
                SerialPort.ReadTimeout = value;
                RaisePropertyChangedEvent(nameof(ReadTimeout));
            }
        }

        /// <summary>
        /// Gets or sets the number of bytes in the internal input buffer before a
        /// DataReceived event occurs.
        /// </summary>
        public int ReceivedBytesThreshold
        {
            get
            {
                return SerialPort.ReceivedBytesThreshold;
            }

            set
            {
                SerialPort.ReceivedBytesThreshold = value;
                RaisePropertyChangedEvent(nameof(ReceivedBytesThreshold));
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Request to Send (RTS) signal is enabled
        /// during serial communication.
        /// </summary>
        public bool RtsEnable
        {
            get
            {
                return SerialPort.RtsEnable;
            }

            set
            {
                SerialPort.RtsEnable = value;
                RaisePropertyChangedEvent(nameof(RtsEnable));
            }
        }

        /// <summary>
        /// Gets or sets the standard number of stopbits per byte.
        /// </summary>
        public StopBits StopBits
        {
            get
            {
                return SerialPort.StopBits;
            }

            set
            {
                SerialPort.StopBits = value;
                RaisePropertyChangedEvent(nameof(StopBits));
            }
        }

        /// <summary>
        /// Gets or sets the size of the serial port output buffer.
        /// </summary>
        public int WriteBufferSize
        {
            get
            {
                return SerialPort.WriteBufferSize;
            }

            set
            {
                SerialPort.WriteBufferSize = value;
                RaisePropertyChangedEvent(nameof(WriteBufferSize));
            }
        }

        /// <summary>
        /// Gets or sets the number of milliseconds before a time-out occurs when a write
        /// operation does not finish.
        /// </summary>
        public int WriteTimeout
        {
            get
            {
                return SerialPort.WriteTimeout;
            }

            set
            {
                SerialPort.WriteTimeout = value;
                RaisePropertyChangedEvent(nameof(WriteTimeout));
            }
        }

        /// <summary>
        /// Initializes a new instance of the SerialPort class using the specified
        /// port name, baud rate, parity bit, data bits, and stop bits.
        /// </summary>
        /// <param name="serialPort">The BaseSerialPort object dependency.</param>
        /// <param name="portName">The port to use e.g., "COM1", "COM10", or "COM100".</param>
        /// <param name="baudRate">The baud rate.</param>
        /// <param name="parity">One of the Parity values.</param>
        /// <param name="dataBits">The data bits value.</param>
        /// <param name="stopBits">One of the StopBits values.</param>
        internal NSerialPort(ISerialPort serialPort,
                             string portName = "COM1",
                             int baudRate = 57600,
                             Parity parity = Parity.None,
                             int dataBits = 8,
                             StopBits stopBits = StopBits.One)
        {
            // Set dependencies
            SerialPort = serialPort;

            // Set port settings and raise PropertyChanged events for startup
            PortName = portName;
            BaudRate = baudRate;
            Parity = parity;
            DataBits = dataBits;
            StopBits = stopBits;

            // Subscribe to SerialPort events so we can raise them from NSerialPort

            SerialPort.DataReceived += SerialPort_DataReceived;
            SerialPort.ErrorReceived += RaiseErrorReceivedEvent;
            SerialPort.PinChanged += RaisePinChangedEvent;
        }

        /// <summary>
        /// Initializes a new instance of the SerialPort class using the specified
        /// port name, baud rate, parity bit, data bits, and stop bits.
        /// </summary>
        /// <param name="portName">The port to use e.g., "COM1", "COM10", or "COM100".</param>
        /// <param name="baudRate">The baud rate.</param>
        /// <param name="parity">One of the Parity values.</param>
        /// <param name="dataBits">The data bits value.</param>
        /// <param name="stopBits">One of the StopBits values.</param>
        public NSerialPort(string portName = "COM1",
                           int baudRate = 57600,
                           Parity parity = Parity.None,
                           int dataBits = 8,
                           StopBits stopBits = StopBits.One)
        {
            // Yes, this violates SOLID.  However, this simplifies the instantiation
            // of NSerialPort objects to ISerialPort nSerialPort = new NSerialPort();
        }

        /// <summary>
        /// Finalizer to dispose of unmanaged resources used by the NSerialPort instance
        /// when the garbage collector is run.
        /// </summary>
        ~NSerialPort()
        {
            Dispose();
        }

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="name">Name of the property that changed.</param>
        protected void RaisePropertyChangedEvent(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        /// <summary>
        /// Raises the DataReceived event indicating that serial data has been received
        /// through a port represented by the NSerialPort object.
        /// </summary>
        /// <param name="sender">The sender of the event, which is the NSerialPort object.</param>
        /// <param name="e">The serial data received event data.</param>
        protected void RaiseDataReceivedEvent(object sender, NSerialDataReceivedEventArgs e)
        {
            DataReceived?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the ErrorReceived event indicating that an error occured with a port
        /// represented by the NSerialPort object.
        /// </summary>
        /// <param name="sender">The sender of the event, which is the NSerialPort object.</param>
        /// <param name="e">The serial error event data.</param>
        protected void RaiseErrorReceivedEvent(object sender, SerialErrorReceivedEventArgs e)
        {
            ErrorReceived?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the PinChanged event indicating that a non-data signal event has occurred
        /// on the port represented by the NSerialPort object.
        /// </summary>
        /// <param name="sender">The sender of the event, which is the NSerialPort object.</param>
        /// <param name="e">The serial pin changed event data.</param>
        protected void RaisePinChangedEvent(object sender, SerialPinChangedEventArgs e)
        {
            PinChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the PinChanged event indicating that a non-data signal event has occurred
        /// on the port represented by the NSerialPort object.
        /// </summary>
        /// <param name="sender">The sender of the event, which is the NSerialPort object.</param>
        /// <param name="e">The serial pin changed event data.</param>
        protected void RaiseLineReceivedEvent(object sender, NSerialLineReceivedEventArgs e)
        {
            LineReceived?.Invoke(this, e);
        }

        /// <summary>
        /// Gets a Comstat struct for the serial communications device.
        /// </summary>
        /// <returns>The ComStat struct for the serial communications device.</returns>
        public ComStat GetComStat(ref Dcb dcb)
        {
            return SerialPort.GetComStat(ref dcb);
        }

        /// <summary>
        /// Gets the value of a specified ComStat flag.
        /// </summary>
        /// <param name="flag">The flag value to find.</param>
        /// <returns>Value of the specified flag.</returns>
        public bool GetComStatFlag(int flag)
        {
            return SerialPort.GetComStatFlag(flag);
        }

        /// <summary>
        /// Gets the DCB control settings for the serial communications device.
        /// </summary>
        /// <returns>The DCB struct representing the control settings for the serial
        /// communications device.</returns>
        public Dcb GetDcb()
        {
            return SerialPort.GetDcb();
        }

        /// <summary>
        /// Sets the control settings for the serial communications device.
        /// </summary>
        /// <param name="dcb">A reference to the DCB control settings for the device.</param>
        public void SetDcb(ref Dcb dcb)
        {
            SerialPort.SetDcb(ref dcb);
        }

        /// <summary>
        /// Gets the value of a specified DCB flag.
        /// </summary>
        /// <param name="flag">The flag value to return.</param>
        /// <returns>Value of the specified flag.</returns>
        public bool GetDcbFlag(int flag)
        {
            return SerialPort.GetDcbFlag(flag);
        }

        /// <summary>
        /// Sets a specified DCB flag for the serial communications device.
        /// </summary>
        /// <param name="flag">DCB flag to set.</param>
        /// <param name="value">Value to set DCB flag to.</param>
        public void SetDcbFlag(int flag, bool value)
        {
            SerialPort.SetDcbFlag(flag, value);
        }

        /// <summary>
        /// Closes the port connection, sets the IsOpen property to false, and disposes
        /// of the internal Stream object.
        /// </summary>
        public void Close()
        {
            SerialPort?.Close();
            RaisePropertyChangedEvent(nameof(IsOpen));
        }

        /// <summary>
        /// Discards data from the serial driver's receive buffer.
        /// </summary>
        public void DiscardInBuffer()
        {
            SerialPort.DiscardInBuffer();
        }

        /// <summary>
        /// Discards data from the serial driver's transmit buffer.
        /// </summary>
        public void DiscardOutBuffer()
        {
            SerialPort.DiscardOutBuffer();
        }

        /// <summary>
        /// Releases all resources used by this serial port resource.
        /// </summary>
        public void Dispose()
        {
            // Release resources held by this AutoResetEvent (it uses a SafeHandle)
            LineReceivedAutoResetEvent.Dispose();

            SerialPort.Dispose();
        }

        /// <summary>
        /// Opens a new serial port connection.
        /// </summary>
        public void Open()
        {
            SerialPort.Open();
            RaisePropertyChangedEvent(nameof(IsOpen));
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
        public int Read(byte[] buffer, int offset, int count)
        {
            return SerialPort.Read(buffer, offset, count);
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
        public int Read(char[] buffer, int offset, int count)
        {
            return SerialPort.Read(buffer, offset, count);
        }

        /// <summary>
        /// Synchronously reads one byte from the SerialPort input buffer.
        /// </summary>
        /// <returns>The byte, cast to an Int32 or -1 if the end of the stream has been
        /// read.</returns>
        public int ReadByte()
        {
            return SerialPort.ReadByte();
        }

        /// <summary>
        /// Synchronously reads one character from the SerialPort input buffer.
        /// </summary>
        /// <returns>The character that was read.</returns>
        public int ReadChar()
        {
            return SerialPort.ReadChar();
        }

        /// <summary>
        /// Reads all immediately available bytes, based on the encoding, in both the stream
        /// and the input buffer of the SerialPOrt object.
        /// </summary>
        /// <returns>The contents of the stream and the input buffer of the SerialPort object.</returns>
        public string ReadExisting()
        {
            return SerialPort.ReadExisting();
        }

        /// <summary>
        /// Reads up to the NewLine value in the input buffer.
        /// </summary>
        /// <returns>The contents of the input buffer up to the first occurrence of a
        /// NewLine value.</returns>
        public string ReadLine()
        {
            return SerialPort.ReadLine();
        }

        /// <summary>
        /// Reads a string up to the specified value in the input buffer.
        /// </summary>
        /// <param name="value">A value that indicates where the read operation stops</param>
        /// <returns>The contents of the input buffer up to the specified value.</returns>
        public string ReadTo(string value)
        {
            return SerialPort.ReadTo(value);
        }

        /// <summary>
        /// Writes a specified number of bytes to the serial port using data from a buffer.
        /// </summary>
        /// <param name="buffer">The byte array that contains the data to write to the port.</param>
        /// <param name="offset">The zero-based byte offset in the buffer parameter at which
        /// to begin copying bytes to the port.</param>
        /// <param name="count">The number of bytes to write.</param>
        public void Write(byte[] buffer, int offset, int count)
        {
            SerialPort.Write(buffer, offset, count);
        }

        /// <summary>
        /// Writes a specified number of characters to the serial port using data from a buffer.
        /// </summary>
        /// <param name="buffer">The byte array that contains the data to write to the port.</param>
        /// <param name="offset">The zero-based byte offset in the buffer parameter at which
        /// to begin copying bytes to the port.</param>
        /// <param name="count">The number of characters to write.</param>
        public void Write(char[] buffer, int offset, int count)
        {
            SerialPort.Write(buffer, offset, count);
        }

        /// <summary>
        /// Writes the specified string to the serial port.
        /// </summary>
        /// <param name="text">The string to write to the output buffer.</param>
        public void Write(string text)
        {
            SerialPort.Write(text);
        }

        /// <summary>
        /// Writes the specified string and the NewLine value to the output buffer.
        /// </summary>
        /// <param name="text">The string to write to the output buffer.</param>
        public void WriteLine(string text)
        {
            SerialPort.WriteLine(text);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">The sender of the event, which is the BaseSerialPort object.</param>
        /// <param name="e">The SerialDataReceivedEventArgs.</param>
        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // Read all new data from the serial port
            string newData = ReadExisting();

            // Append it to the existing data
            InputBuffer += newData;

            // Let everyone know we got new data
            RaiseDataReceivedEvent(this, new NSerialDataReceivedEventArgs(e.EventType, newData));

            // TODO: This regex can probably be rewritten to get rid of the if (line.Contains...)
            // Split the InputBuffer on the NewLine string
            string[] lines = Regex.Split(InputBuffer, @"(?<=[" + NewLine + "])");

            foreach (string line in lines)
            {
                if (line.Contains(NewLine))
                {
                    // We got a line!
                    // Clear the received line from the input buffer
                    InputBuffer = InputBuffer.Remove(0, line.Length);

                    RaiseLineReceivedEvent(this, new NSerialLineReceivedEventArgs(e.EventType, line));
                }
            }
        }

        /// <summary>
        /// Writes the specified string and the NewLine value to the output buffer, and
        /// returns a line received, if any.
        /// </summary>
        /// <param name="text">The string to write to the output buffer.</param>
        /// <param name="timeout">Amount of time to wait in milliseconds before retransmitting the message.</param>
        /// <param name="retries">Number of retries to attempt.</param>
        /// <returns>The received line on the serial port; otherwise, null.</returns>
        public string TranceiveLine(string text, int timeout = 100, int retries = 0)
        {
            bool gotPacket = false;
            string result = null;

            // TODO: Can this be made more efficient on the CPU?
            // Define a temporary LineReceived event handler to capture a serial message
            void lineReceivedHandler(object sender, NSerialLineReceivedEventArgs lineReceivedEventArgs)
            {
                result = lineReceivedEventArgs.Line; // Grab received message from event args
                LineReceivedAutoResetEvent.Set();  // Unblock thread
            }

            // Subscribe to message received event we just created to get messages
            LineReceived += lineReceivedHandler;

            do
            {
                WriteLine(text);

                // Block until we get a message or timeout
                gotPacket = LineReceivedAutoResetEvent.WaitOne(timeout);
            }
            while (!gotPacket && --retries > 0);  // Transmit until we get a message back or run out of retries

            // Unsubscribe (save RAM, removes possible event problems
            // for delegates with similar signature, etc.)
            LineReceived -= lineReceivedHandler;

            return result;
        }
    }
}
