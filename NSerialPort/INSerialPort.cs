using SerialPortFix;
using static NSerialPort.NSerialPort;

namespace NSerialPort
{
    public interface INSerialPort : ISerialPort
    {
        /// <summary>
        /// Indicates that data has been received through a port represented by the SerialPort
        /// object.
        /// </summary>
        new event NSerialDataReceivedEventHandler DataReceived;

        /// <summary>
        /// Indicates that an error has ocurred with a port represented by the SerialPort object.
        /// </summary>
        event NSerialLineReceivedEventHandler LineReceived;
    }
}
