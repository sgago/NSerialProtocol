using NSerialProtocol.FrameParsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NSerialProtocol
{
    public enum FrameErrorType
    {
        BufferOverrun,
        InvalidLength
    }

    public abstract class SerialFrameParser
    {
        protected RegexOptions RegexOptions { get; } =
            RegexOptions.CultureInvariant | RegexOptions.Multiline;

        public delegate void SerialFrameReceivedEventHandler(object sender, SerialFrameReceivedEventArgs e);


        public delegate void SerialFrameErrorEventHandler(object sender, SerialFrameErrorEventArgs e);

        /// <summary>
        /// Indicates that a serial frame has been received through a port represented by the
        /// SerialPort object.
        /// </summary>
        public abstract event SerialFrameReceivedEventHandler SerialFrameReceived;

        /// <summary>
        /// Indicates that a framing error has occured with data in the SerialPacketPrototype object.
        /// </summary>
        public abstract event SerialFrameErrorEventHandler SerialFrameError;

        public abstract void Parse(string data);
    }
}
