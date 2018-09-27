namespace NativeMethods
{
    using Microsoft.Win32.SafeHandles;
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    /// <summary>
    /// File type of the specified file.
    /// </summary>
    public enum FileType
    {
        /// <summary>
        /// Either the type of the specified file is unknown or
        /// the function failed.
        /// </summary>
        Unknown = 0x0000,

        /// <summary>
        /// The specified file is a disk file.
        /// </summary>
        Disk = 0x0001,

        /// <summary>
        /// The specified file is a character file, typically an LPT device
        /// or console.
        /// </summary>
        Char = 0x0002,

        /// <summary>
        /// The specified file is a socket, named pipe, or anonymous pipe.
        /// </summary>
        Pipe = 0x0003,

        /// <summary>
        /// Unused.
        /// </summary>
        Remote = 0x8000,
    }

    /// <summary>
    /// Flags used in the ComStat structure.
    /// </summary>
    [Flags]
    public enum ComStatFlags
    {
        /// <summary>
        /// If this member is true, transmission is waiting for the
        /// clear-to-send (CTS) signal to be sent.
        /// </summary>
        fCtsHold,

        /// <summary>
        /// If this member is true, transmission is waiting for the
        /// data-set-ready (DTR) signal to be sent.
        /// </summary>
        fDsrHold,

        /// <summary>
        /// If this memeber is true, transmission is waiting for the
        /// receive-line-signal-detect (RLSD) signal to be sent.
        /// </summary>
        fRlsdHold,

        /// <summary>
        /// If this member is true, transmission is waiting because
        /// the XOFF character was received.
        /// </summary>
        fXoffHold,

        /// <summary>
        /// If this member is true, transmission is waiting because
        /// the XOFF character was transmitted.
        /// </summary>
        fXoffSent,

        /// <summary>
        /// IF this member is true, the end-of-file (EOF) character has
        /// been received.
        /// </summary>
        fEof,

        /// <summary>
        /// If this member is true, there is a character queued for transmission
        /// that has come to the communications device by way of the
        /// TransmiteCommChar function.
        /// </summary>
        fTxim,

        // Bits numbered 8 through 32 are fReserved, do not use.
    }

    /// <summary>
    /// Contains information about a communications device.  This structure
    /// is filled by the ClearCommError function.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ComStat
    {
        /// <summary>
        /// Comstat structure flags.
        /// </summary>
        public uint Flags;

        /// <summary>
        /// The number of bytes received by the serial provider but not yet
        /// read by a ReadFile operation.
        /// </summary>
        public uint cbInQue;

        /// <summary>
        /// The number of bytes of user data remaining to be transmitted for all
        /// write operations.  This value will be zero for a nonoverlapped write.
        /// </summary>
        public uint cbOutQue;
    }

    /// <summary>
    /// Flags used within the DCB structure.
    /// </summary>
    [Flags]
    public enum DcbFlags
    {
        /// <summary>
        /// If this member is true, binary mode is enabled.  Windows does not support
        /// nonbinary mode transfers, so this member must be true.
        /// </summary>
        fBinary = 1,

        /// <summary>
        /// If this member is true, parity checking is performed and errors are
        /// reported.
        /// </summary>
        fParity,

        /// <summary>
        /// If this member is true, the clear-to-send (CTS) signal is monitored
        /// for output flow control.  If this member is true and CTS is turned off,
        /// the output is suspended until CTS is sent again.
        /// </summary>
        fOutxCtsFlow,

        /// <summary>
        /// If this member is true, the data-set-ready (DSR) signal is monitored
        /// for output flow control.  If this member is true and DSR is turned off,
        /// the output is suspended until DSR is sent again.
        /// </summary>
        fOutxDsrFlow,

        // TODO: Verify this is actually the low bit!!!
        /// <summary>
        /// The data-terminal-ready (DTR) flow control low bit.
        /// </summary>
        fDtrControlLowBit,

        // TODO: Verify this is actually the high bit!!!
        /// <summary>
        /// The data-terminal-ready (DTR) flow control high bit.
        /// </summary>
        fDtrControlHighBit,

        /// <summary>
        /// If this member is true, the communications driver is sensitive to the
        /// state of the data-set-ready (DSR) signal.  The driver ignores any bytes
        /// received, unless the DSR modem input line is high.
        /// </summary>
        fDsrSensitivity,

        /// <summary>
        /// If this member is true, transmission continues after the input buffer
        /// has come within XoffLim bytes of being full and the driver has transmitted
        /// the XoffChar character to stop receiving bytes.  If this member is false,
        /// transmission does not continue until the input buffer is within the
        /// XonLim bytes of being empty and the driver has transmitted the XonChar
        /// character to resume reception.
        /// </summary>
        fTXContinueOnXoff,

        /// <summary>
        /// Indicates whether XON/XOFF flow control is used during transmission.
        /// If this member is true, transmission stops when the XoffChar character
        /// is received and starts again when the XonChar character is received.
        /// </summary>
        fOutx,

        /// <summary>
        /// Indicates whether XON/XOFF flow control is used during reception.  If
        /// this member is true, the XoffChar character is sent when the input buffer
        /// comes within XoffLim bytes of being full, and the XonChar character is
        /// sent when the input buffer comes within XonLim bytes of being empty.
        /// </summary>
        fInX,

        /// <summary>
        /// Indicates whether bytes received with parity errors are replaced with
        /// the character specified by the ErrorChar member. If this member is true
        /// and the fParity member is true, replacement occurs.
        /// </summary>
        fErrorChar,

        /// <summary>
        /// If this member is true, null bytes are discarded when received.
        /// </summary>
        fNull,

        // TODO: Verify this is the low bit!!!
        /// <summary>
        /// The request-to-send (RTS) flow control low bit.
        /// </summary>
        fRtsControlLowBit,

        // TODO: Verify this is the high bit!!!
        /// <summary>
        /// The request-to-send (RTS) flow control high bit.
        /// </summary>
        fRtsControlHighBit,

        /// <summary>
        /// If this member is true, the driver terminates all read and write
        /// operations with an error status if an error occurs. The driver will not
        /// accept any further communications operations until the application has
        /// acknowledged the error by calling the ClearCommError function.
        /// </summary>
        fAbortOnError,

        // Bit number 15 is fDummy2; do not use
    }

    /// <summary>
    /// A device-control block (DCB) which defines control settings for a serial
    /// communications device.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Dcb
    {
        /// <summary>
        /// The length of the structure in bytes.  The caller must set this member
        /// to sizeof(DCB).
        /// </summary>
        public uint DCBlength;

        /// <summary>
        /// The baud rate at which the communications device operates.
        /// </summary>
        public uint BaudRate;

        /// <summary>
        /// DCB flags for control settings.
        /// </summary>
        public uint Flags;

        /// <summary>
        /// Reserved; must be zero.
        /// </summary>
        public ushort wReserved;

        /// <summary>
        /// The minimum number of bytes in use allowed in the input buffer before flow
        /// control is activated to allow transmission by the sender.  This assumes
        /// that either XON/XOFF, RTS, or DTR inputs flow control is specified in the
        /// fInX, fRtsControl, or fDtrControl members.
        /// </summary>
        public ushort XonLim;

        /// <summary>
        /// The minimum number of free bytes allowed in the input buffer before flow
        /// control is activated to inhibit the sender.
        /// </summary>
        public ushort XoffLim;

        /// <summary>
        /// The number of bits in the bytes transmitted and received.
        /// </summary>
        public byte ByteSize;

        /// <summary>
        /// The parity scheme to be used.
        /// </summary>
        public byte Parity;

        /// <summary>
        /// The number of stop bits to be used.
        /// </summary>
        public byte StopBits;

        /// <summary>
        /// The value of the XON character for both transmission and reception.
        /// </summary>
        public byte XonChar;

        /// <summary>
        /// The value of the XOFF character for both transmission and reception.
        /// </summary>
        public byte XoffChar;

        /// <summary>
        /// The value of the character used to replace bytes received with a parity
        /// error.
        /// </summary>
        public byte ErrorChar;

        /// <summary>
        /// The value of the character used to signal the end of data.
        /// </summary>
        public byte EofChar;

        /// <summary>
        /// The value of the character used to signal an event.
        /// </summary>
        public byte EvtChar;

        /// <summary>
        /// Reserved; do not use.
        /// </summary>
        public ushort wReserved1;
    }

    /// <summary>
    /// Contains native, Kernel32 DLL (Win32) calls for more control over the serial ports.
    /// </summary>
    internal static class NativeMethods
    {
        /// <summary>
        /// Formats a message string.
        /// </summary>
        ///
        /// <param name="dwFlags">
        /// The formatting options, and how to interpret the lpSource parameter.
        /// </param>
        ///
        /// <param name="lpSource">
        /// The location of the message definition. The type of
        /// this parameter depends upon the settings in the dwFlags parameter.
        /// </param>
        ///
        /// <param name="dwMessageId">
        /// The message identifier for the requested message.
        /// This parameter is ignored if dwFlags includes
        /// FORMAT_MESSAGE_FROM_STRING.
        /// </param>
        ///
        /// <param name="dwLanguageId">
        /// The language identifier for the requested message.
        /// </param>
        ///
        /// <param name="lpBuffer">
        /// A pointer to a buffer that receives the
        /// null-terminated string that specifies the formatted message.
        /// </param>
        ///
        /// <param name="nSize">
        /// If the FORMAT_MESSAGE_ALLOCATE_BUFFER flag is not set,
        /// this parameter specifies the size of the output buffer, in TCHARs.
        /// </param>
        ///
        ///
        /// <param name="arguements">
        /// An array of values that are used as insert values
        /// in the formatted message. A %1 in the format string indicates the first
        /// value in the Arguments array; a %2 indicates the second argument; and
        /// so on.
        /// </param>
        ///
        /// <returns>
        /// If the function succeeds, the return value is the number of
        /// TCHARs stored in the output buffer, excluding the terminating null
        /// character.
        /// </returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int FormatMessage(int dwFlags,
                                               HandleRef lpSource,
                                               int dwMessageId,
                                               int dwLanguageId,
                                               StringBuilder lpBuffer,
                                               int nSize,
                                               IntPtr arguements);

        /// <summary>
        /// Retrieves the current control settings for a specified communications device.
        /// </summary>
        ///
        /// <param name="hFile">A handle to the communications device.
        /// The CreateFile function returns this handle.</param>
        ///
        /// <param name="lpDcb">A pointer to a DCB structure that receives the control settings
        /// information.</param>
        ///
        /// <returns>If the function succeeds, the return value is zero.
        /// If the function fails, the return value is zero.  To get extended error information, call GetLastError.
        /// </returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool GetCommState(SafeFileHandle hFile, ref Dcb lpDcb);

        /// <summary>
        /// Configures a communications device according to the specifications in a device-control-block (DCB).
        /// </summary>
        ///
        /// <param name="hFile">A hanlde to the communications device.  The CreateFile function returns this handle.
        /// </param>
        ///
        /// <param name="lpDcb">A pointer to the DCB structure that contains the configuration information
        /// for the specified communications device.</param>
        ///
        /// <returns>If the function succeeds, the return value is nonzero.
        /// If the function fails, the return value is zero.  To get extended error information, call GetLastError.
        /// </returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetCommState(SafeFileHandle hFile, ref Dcb lpDcb);

        /// <summary>
        /// Retrieves information about a communications errror and reports the current
        /// status of a communications device.
        /// </summary>
        /// <param name="hFile">A handle to the communications device.</param>
        /// <param name="lpErrors">A pointer to a variable that receives a mask
        /// indicating the type of error.</param>
        /// <param name="lpStat">A pointer to a ComStat structure in which the device's
        /// status information is returned.</param>
        /// <returns>If the function succeeds, the return value is nonzero.</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool ClearCommError(SafeFileHandle hFile,
                                                 ref int lpErrors,
                                                 ref ComStat lpStat);

        /// <summary>
        /// Creates of opens a file or I/O device.
        /// </summary>
        /// <param name="lpFileName">The name of the file or device to be created or opened.</param>
        /// <param name="dwDesiredAccess">The requested access to the file or device, which can be
        /// summarized as read, write, both, or neither zero.</param>
        /// <param name="dwShareMode">The requested sharing mode of the file or device, which can
        /// be read, write, both, delete, all of these, or none.</param>
        /// <param name="securityAttrs">A pointer to a SECURITY_ATTRIBUTES structure that contains
        /// two separate but related data members.</param>
        /// <param name="dwCreationDisposition">An action to take on a file or device that exists
        /// or does not exist.</param>
        /// <param name="dwFlagsAndAttributes">The file or device attributes flags.</param>
        /// <param name="hTemplateFile">A valid handle to a template file with the GENERIC_READ
        /// access right.</param>
        /// <returns>If the function succeeds, the return value is an open handle to the specified
        /// file, device, named pipe, or mail slot.</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern SafeFileHandle CreateFile(string lpFileName,
                                                       int dwDesiredAccess,
                                                       int dwShareMode,
                                                       IntPtr securityAttrs,
                                                       int dwCreationDisposition,
                                                       int dwFlagsAndAttributes,
                                                       IntPtr hTemplateFile);

        /// <summary>
        /// Retrieves the file type of the specified file.
        /// </summary>
        /// <param name="hFile">A handle to the file.</param>
        /// <returns>The coded file type.</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int GetFileType(SafeFileHandle hFile);
    }
}
