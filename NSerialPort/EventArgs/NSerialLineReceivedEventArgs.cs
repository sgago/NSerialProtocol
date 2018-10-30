using System.IO.Ports;

namespace NSerialPort.EventArgs
{
    using System;

    /// <summary>
    /// Provides data for the NSerialPort LineReceived event.
    /// </summary>
    public class NSerialLineReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the event type.
        /// </summary>
        public readonly SerialData EventType;

        /// <summary>
        /// The line that that was received.
        /// </summary>
        public readonly string Line;

        /// <summary>
        /// Instantiates a new instance of the SerialLineReceivedEventArgs class.
        /// </summary>
        /// <param name="line">The line that was received from the serial port.</param>
        /// <param name="eventType">The type of event.</param>
        public NSerialLineReceivedEventArgs(SerialData eventType, string line)
        {
            EventType = eventType;
            Line = line;
        }
    }
}