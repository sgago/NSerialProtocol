namespace SerialPortFix
{
    using Microsoft.Win32.SafeHandles;
    using NativeMethods;
    using System;
    using System.IO.Ports;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Represents a base serial port resource which clears the fAbortOnError flag.
    /// </summary>
    /// <remarks>
    /// The purpose of this class is two-fold.
    /// First, this class inherits an interface.  This is useful for decoupling and
    /// creating substitutes for unit testing.
    /// Second, members that are difficult to unit test have been located here as they
    /// require a virtual or physical serial port.  A separate
    /// unit test project will be created for those members.
    /// </remarks>
    /// <example>
    /// <code>
    ///     using (ISerialPort serialPort = new SerialPortFix("COM10", 57600))
    ///     {
    ///         // TODO: Write your code using the serial port here
    ///     }
    /// </code>
    /// </example>
    [System.ComponentModel.DesignerCategory("Code")]
    public sealed class SerialPortFix : SerialPort, ISerialPortFix
    {
        /// <summary>
        /// Number of attempts to try and get the Comm state of the serial port.
        /// </summary>
        private const int CommStateRetries = 10;

        /// <summary>
        /// Gets or sets a SafeFileHandle to the serial port.
        /// </summary>
        private SafeFileHandle SerialPortHandle { get; set; }

        /// <summary>
        /// Gets or sets the port for communication, including but not limited to
        /// all available COM ports.  Valid names include "COM1", "COM2", "COM10", etc.
        /// </summary>
        public new string PortName
        {
            get
            {
                return base.PortName;
            }

            set
            {
                SerialPortHandle = GetSerialPortHandle(value);

                // FIXME: Magic number 14 is the fAbortOnError flag
                // FIXME: Should be using a NativeMethods enum or similar
                SetDcbFlag(14, false);

                base.PortName = value;
            }
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
        public SerialPortFix(string portName = "COM1",
                             int baudRate = 57600,
                             Parity parity = Parity.None,
                             int dataBits = 8,
                             StopBits stopBits = StopBits.One)
        {
            // Calls base, no code here
        }

        /// <summary>
        /// Releases all resources used by this serial port resource.
        /// </summary>
        public new void Dispose()
        {
            base.Dispose();
            SerialPortHandle?.Close();
            SerialPortHandle = null;
        }

        /// <summary>
        /// Looks up the last Win32 error and throws an exception.
        /// </summary>
        // TODO: Threw an UnauthorizedAccessException exception!!!!
        private void ThrowWinIoException()
        {
            Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
        }

        /// <summary>
        /// Gets a SafeFileHandle from a serial port name.
        /// </summary>
        /// <param name="portName">Port name to get SafeFileHandle from.</param>
        /// <returns>A SafeFileHandle to the specified port.</returns>
        private SafeFileHandle GetSerialPortHandle(string portName)
        {
            SafeFileHandle safeFileHandle;
            FileType fileType = FileType.Remote;
            const int dwFlagsAndAttributes = 0x40000000;
            const int dwAccess = unchecked((int)0xC0000000);

            // Create a safe file handle (yay Win32) to the serial port
            safeFileHandle = NativeMethods.CreateFile(
                @"\\.\" + portName,
                dwAccess,
                0,
                IntPtr.Zero,
                3,
                dwFlagsAndAttributes,
                IntPtr.Zero
             );

            // Check if our SafeFileHandle is valid
            if (safeFileHandle.IsInvalid)
            {
                // Uhoh, SafeFileHandle is invalid
                safeFileHandle.Close();
                ThrowWinIoException();
            }

            // Get the FileType of our SafeFileHandle to the SerialPort
            fileType = (FileType)NativeMethods.GetFileType(safeFileHandle);

            // Verify the SafeFileHandle's file type
            if ((fileType != FileType.Unknown) && (fileType != FileType.Char))
            {
                // Uhoh, bad port name
                safeFileHandle.Close();
                throw new ArgumentException("Invalid port name.", "portName");
            }

            return safeFileHandle;
        }

        /// <summary>
        /// Gets the ComStat structure for the communications device.
        /// </summary>
        /// <param name="dcb">A reference DCB struct containing control settings for
        /// the serial communications device.</param>
        /// <returns>A ComStat struct containing information about the communications
        /// device.</returns>
        public ComStat GetComStat(ref Dcb dcb)
        {
            bool gotCommState = false;
            int commErrors = 0;
            ComStat comStat = new ComStat();

            // Call ClearCommError until we fill the ComStat struct
            for (int i = 0; (i < CommStateRetries) && (!gotCommState); i++)
            {
                // Call ClearCommError which fills the ComStat struct for us
                if (!NativeMethods.ClearCommError(SerialPortHandle, ref commErrors, ref comStat))
                {
                    // Uhoh, ClearCommError failed
                    SerialPortHandle.Close();
                    ThrowWinIoException();
                }

                // Call GetCommState to fills the DCB struct
                gotCommState = NativeMethods.GetCommState(SerialPortHandle, ref dcb);
            }

            if (!gotCommState)
            {
                // Uhoh, we didn't get the CommState
                SerialPortHandle.Close();
                ThrowWinIoException();
            }

            return comStat;
        }

        /// <summary>
        /// Gets the value of a specified ComStat flag.
        /// </summary>
        /// <param name="flag">The ComStat flag value to retrieve.</param>
        /// <returns>Value of the specified ComStat flag.</returns>
        public bool GetComStatFlag(int flag)
        {
            Dcb dcb = GetDcb();
            ComStat comStat = GetComStat(ref dcb);

            return Convert.ToBoolean((comStat.Flags >> flag) & 1);
        }

        /// <summary>
        /// Gets the DCB control settings for the serial communications device.
        /// </summary>
        /// <returns>The DCB struct representing the control settings for the serial
        /// communications device.</returns>
        public Dcb GetDcb()
        {
            Dcb dcb = new Dcb();

            GetComStat(ref dcb);

            return dcb;
        }

        /// <summary>
        /// Sets the DCB structure for the communications device.
        /// </summary>
        /// <param name="dcb">A refernce DCB struct containing control settings for
        /// the serial communications device.</param>
        public void SetDcb(ref Dcb dcb)
        {
            bool setCommState = false;
            int commErrors = 0;
            ComStat comStat = new ComStat();

            for (int i = 0; (i < CommStateRetries) && (!setCommState); i++)
            {
                if (!NativeMethods.ClearCommError(SerialPortHandle, ref commErrors, ref comStat))
                {
                    // Uhoh, ClearCommError had a problem
                    SerialPortHandle.Close();
                    ThrowWinIoException();
                }

                setCommState = NativeMethods.SetCommState(SerialPortHandle, ref dcb);
            }

            if (!setCommState)
            {
                // Uhoh, we didn't set the CommState
                SerialPortHandle.Close();
                ThrowWinIoException();
            }
        }

        /// <summary>
        /// Gets the value of a specified DCB flag.
        /// </summary>
        /// <param name="flag">The DCB flag value to retrieve.</param>
        /// <returns>Value of the specified DCB flag.</returns>
        public bool GetDcbFlag(int flag)
        {
            return Convert.ToBoolean((GetDcb().Flags >> flag) & 1);
        }

        /// <summary>
        /// Sets a DCB flag to a specified value.
        /// </summary>
        /// <param name="flag">The DCB flag to set.</param>
        /// <param name="value">Value to set the DCB flag to.</param>
        public void SetDcbFlag(int flag, bool value)
        {
            Dcb dcb = GetDcb();

            // Bit magic for setting a flag
            dcb.Flags &= ~((uint)(value ? 1 : 0) << flag);

            SetDcb(ref dcb);
        }
    }
}