using NativeMethods;
using SerialPortFix;
using System;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Threading.Tasks;
using static NSerialPort.NSerialPort;

namespace NSerialPort
{
    /// <summary>
    /// Represents a NSerialPort resource.
    /// </summary>
    public interface INSerialPort : IDisposable
    {
        /// <summary>
        /// Gets the underlying Stream object for a SerialPort object.
        /// </summary>
        Stream BaseStream { get; }

        /// <summary>
        /// Gets or sets the serial baud rate.
        /// </summary>
        int BaudRate { get; set; }

        /// <summary>
        /// Gets or sets the break signal state.
        /// </summary>
        bool BreakState { get; set; }

        /// <summary>
        /// Gets the number of bytes of data in the receive buffer.
        /// </summary>
        int BytesToRead { get; }

        /// <summary>
        /// Gets the number of bytes of data in the send buffer.
        /// </summary>
        int BytesToWrite { get; }

        /// <summary>
        /// Gets the state of the Carrier Detect line for the port.
        /// </summary>
        bool CDHolding { get; }

        /// <summary>
        /// Gets the state of the Clear-to-Send line.
        /// </summary>
        bool CtsHolding { get; }

        /// <summary>
        /// Gets or sets the standard length of data bits per byte.
        /// </summary>
        int DataBits { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether null bytes are ignored when transmitted
        /// between the port and the receive buffer.
        /// </summary>
        bool DiscardNull { get; set; }

        /// <summary>
        /// Gets the state of the Data Set Ready (DSR) signal.
        /// </summary>
        bool DsrHolding { get; }

        /// <summary>
        /// Gets or sets a value that enables the Data Terminal Ready (DTR) signal during
        /// serial communication.
        /// </summary>
        bool DtrEnable { get; set; }

        /// <summary>
        /// Gets or sets the byte encoding for pre- and post-transmission conversion of text.
        /// </summary>
        Encoding Encoding { get; set; }

        /// <summary>
        /// Gets or sets the handshaking protocol for serial port transmission of data using
        /// a value from Handshake.
        /// </summary>
        Handshake Handshake { get; set; }

        /// <summary>
        /// Gets a value indicating the open or closed status of the SerialPort object.
        /// </summary>
        bool IsOpen { get; }

        /// <summary>
        /// Gets or sets the value used to interpret the end of a call to the ReadLine and WriteLine
        /// methods.
        /// </summary>
        string NewLine { get; set; }

        /// <summary>
        /// Gets or sets the parity-checking protocol.
        /// </summary>
        Parity Parity { get; set; }

        /// <summary>
        /// Gets or sets the byte that replaces invalid bytes in a data stream when a parity error
        /// occurs.
        /// </summary>
        byte ParityReplace { get; set; }

        /// <summary>
        /// Gets or sets the port for communications, including but not limited to all available
        /// COM ports.
        /// </summary>
        string PortName { get; set; }

        /// <summary>
        /// Gets or sets the size of the SerialPort input buffer.
        /// </summary>
        int ReadBufferSize { get; set; }

        /// <summary>
        /// Gets or sets the number of milliseconds before a time-out occurs when a read operation
        /// does not finish.
        /// </summary>
        int ReadTimeout { get; set; }

        /// <summary>
        /// Gets or sets the number of bytes in the internal input buffer before a
        /// DataReceived event occurs.
        /// </summary>
        int ReceivedBytesThreshold { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Request to Send (RTS) signal is enabled
        /// during serial communication.
        /// </summary>
        bool RtsEnable { get; set; }

        /// <summary>
        /// Gets or sets the standard number of stopbits per byte.
        /// </summary>
        StopBits StopBits { get; set; }

        /// <summary>
        /// Gets or sets the size of the serial port output buffer.
        /// </summary>
        int WriteBufferSize { get; set; }

        /// <summary>
        /// Gets or sets the number of milliseconds before a time-out occurs when a write
        /// operation does not finish.
        /// </summary>
        int WriteTimeout { get; set; }

        /// <summary>
        /// Indicates that an error has ocurred with a port represented by the SerialPort object.
        /// </summary>
        event SerialErrorReceivedEventHandler ErrorReceived;

        /// <summary>
        /// Indicates that a non-data signal event has occurred on the port represented
        /// by the SerialPort object.
        /// </summary>
        event SerialPinChangedEventHandler PinChanged;

        /// <summary>
        /// Indicates that data has been received through a port represented by the SerialPort
        /// object.
        /// </summary>
        event NSerialDataReceivedEventHandler DataReceived;

        /// <summary>
        /// Indicates that an error has ocurred with a port represented by the SerialPort object.
        /// </summary>
        event NSerialLineReceivedEventHandler LineReceived;

        /// <summary>
        /// Closes the port connection, sets the IsOpen property to false, and disposes
        /// of the internal Stream object.
        /// </summary>
        void Close();

        /// <summary>
        /// Discards data from the serial driver's receive buffer.
        /// </summary>
        void DiscardInBuffer();

        /// <summary>
        /// Discards data from the serial driver's transmit buffer.
        /// </summary>
        void DiscardOutBuffer();

        /// <summary>
        /// Opens a new serial port connection.
        /// </summary>
        void Open();

        /// <summary>
        /// Reads a number of bytes from the SerialPort input buffer and writes those
        /// bytes into a byte array at the specified offset.
        /// </summary>
        /// <param name="buffer">The byte array to write the input to.</param>
        /// <param name="offset">The offset in buffer at which to write the bytes.</param>
        /// <param name="count">The maximum number of bytes to read.  Fewer bytes are
        /// read if count is greater than the number of bytes in the input buffer.</param>
        /// <returns>The number of bytes read.</returns>
        int Read(byte[] buffer, int offset, int count);

        /// <summary>
        /// Reads a number of characters from the SerialPort input buffer and writes them into
        /// an array of characters at a given offset.
        /// </summary>
        /// <param name="buffer">The character array to write the input to.</param>
        /// <param name="offset">The offset in buffer at which to write the characters</param>
        /// <param name="count">The maximum number of bytes to read.  Fewer bytes are
        /// read if count is greater than the number of bytes in the input buffer.</param>
        /// <returns>The number of bytes read.</returns>
        int Read(char[] buffer, int offset, int count);

        /// <summary>
        /// Synchronously reads one byte from the SerialPort input buffer.
        /// </summary>
        /// <returns>The byte, cast to an Int32 or -1 if the end of the stream has been
        /// read.</returns>
        int ReadByte();

        /// <summary>
        /// Synchronously reads one character from the SerialPort input buffer.
        /// </summary>
        /// <returns>The character that was read.</returns>
        int ReadChar();

        /// <summary>
        /// Reads all immediately available bytes, based on the encoding, in both the stream
        /// and the input buffer of the SerialPort object.
        /// </summary>
        /// <returns>The contents of the stream and the input buffer of the SerialPort object.</returns>
        string ReadExisting();

        /// <summary>
        /// Reads up to the NewLine value in the input buffer.
        /// </summary>
        /// <returns>The contents of the input buffer up to the first occurrence of a
        /// NewLine value.</returns>
        string ReadLine();

        /// <summary>
        /// Reads a string up to the specified value in the input buffer.
        /// </summary>
        /// <param name="value">A value that indicates where the read operation stops</param>
        /// <returns>The contents of the input buffer up to the specified value.</returns>
        string ReadTo(string value);

        /// <summary>
        /// Writes a specified number of bytes to the serial port using data from a buffer.
        /// </summary>
        /// <param name="buffer">The byte array that contains the data to write to the port.</param>
        /// <param name="offset">The zero-based byte offset in the buffer parameter at which
        /// to begin copying bytes to the port.</param>
        /// <param name="count">The number of bytes to write.</param>
        void Write(byte[] buffer, int offset, int count);

        /// <summary>
        /// Writes a specified number of characters to the serial port using data from a buffer.
        /// </summary>
        /// <param name="buffer">The byte array that contains the data to write to the port.</param>
        /// <param name="offset">The zero-based byte offset in the buffer parameter at which
        /// to begin copying bytes to the port.</param>
        /// <param name="count">The number of characters to write.</param>
        void Write(char[] buffer, int offset, int count);

        /// <summary>
        /// Writes the specified string to the serial port.
        /// </summary>
        /// <param name="text">The string to write to the output buffer.</param>
        void Write(string text);

        /// <summary>
        /// Writes the specified string and the NewLine value to the output buffer.
        /// </summary>
        /// <param name="text">The string to write to the output buffer.</param>
        void WriteLine(string text);


        string TranceiveLine(string text, int timeout = 100, int retries = 0);


        Task<string> TranceiveLineAsync(string text, int timeout = 100, int retries = 0);

        /// <summary>
        /// Gets the ComStat structure for the communications device.
        /// </summary>
        /// <param name="dcb">A reference DCB struct containing control settings for
        /// the serial communications device.</param>
        /// <returns>A ComStat struct containing information about the communications
        /// device.</returns>
        ComStat GetComStat(ref Dcb dcb);

        /// <summary>
        /// Gets the value of a specified ComStat flag.
        /// </summary>
        /// <param name="flag">The ComStat flag value to retrieve.</param>
        /// <returns>Value of the specified ComStat flag.</returns>
        bool GetComStatFlag(int flag);

        /// <summary>
        /// Gets the DCB control settings for the serial communications device.
        /// </summary>
        /// <returns>The DCB struct representing the control settings for the serial
        /// communications device.</returns>
        Dcb GetDcb();

        /// <summary>
        /// Gets the value of a specified DCB flag.
        /// </summary>
        /// <param name="flag">The DCB flag value to retrieve.</param>
        /// <returns>Value of the specified DCB flag.</returns>
        bool GetDcbFlag(int flag);

        /// <summary>
        /// Sets the DCB structure for the communications device.
        /// </summary>
        /// <param name="dcb">A refernce DCB struct containing control settings for
        /// the serial communications device.</param>
        void SetDcb(ref Dcb dcb);

        /// <summary>
        /// Sets a DCB flag to a specified value.
        /// </summary>
        /// <param name="flag">The DCB flag to set.</param>
        /// <param name="value">Value to set the DCB flag to.</param>
        void SetDcbFlag(int flag, bool value);
    }
}
