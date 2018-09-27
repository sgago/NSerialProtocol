using System.IO.Ports;

namespace NSerialPort.EventArgs
{
    public class NSerialDataReceivedEventArgs : System.EventArgs
    {
        /// <summary>
        /// Gets the Event type.
        /// </summary>
        public readonly SerialData EventType;

        /// <summary>
        /// Gets the data that was read from the serial port.
        /// </summary>
        public readonly string Data;

        /// <summary>
        /// Instantiates a new instance of the NSerialDataReceivedEventArgs class.
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="data"></param>
        public NSerialDataReceivedEventArgs(SerialData eventType, string data)
        {
            EventType = eventType;
            Data = data;
        }
    }
}
