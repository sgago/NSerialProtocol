using System.IO.Ports;

namespace NSerialPort
{
    /// <summary>
    /// Contains event data about a received line from the serial port.
    /// </summary>
    public class NSerialLineReceivedEventArgs : System.EventArgs
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