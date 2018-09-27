using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialPortFixAccessingWin32Structures
{
    using NativeMethods;
    using SerialPortFix;

    public class Program
    {
        /// <summary>
        /// Name of the port.
        /// </summary>
        private const string PortName = "COM1";

        /// <summary>
        /// Baud rate of the port.
        /// </summary>
        private const int BaudRate = 9600;

        /// <summary>
        /// Bit poisition of the fAbortOnError flag in the DCB flags field.
        /// </summary>
        private const int fAbortOnErrorBitPosition = 14;

        /// <summary>
        /// Bit position of the fClearToSend flag in the ComStat flags field.
        /// </summary>
        private const int fClearToSendBitPosition = 1;

        public static void Main(string[] args)
        {
            // Instantiate a serial port within the using keyword
            using (ISerialPort serialPort = new SerialPortFix(PortName, BaudRate))
            {
                /*** DCB Example ***/

                // Gets the underlying DCB Win32 struct
                Dcb dcb = serialPort.GetDcb();

                // Change the DCB struct as using the fields
                dcb.StopBits = 2;

                // Set the underlying DCB Win32 struct
                serialPort.SetDcb(ref dcb);

                // Or you can access the DCB flags such as fAbortOnError like this
                bool fAbortOnErrorFlag = serialPort.GetDcbFlag(fAbortOnErrorBitPosition);

                // And set DCB flags like this
                serialPort.SetDcbFlag(fAbortOnErrorBitPosition, false);



                /*** ComStat Example ***/

                // Get the ComStat from a referenced DCB struct
                ComStat comStat = serialPort.GetComStat(ref dcb);

                // Use the ComStat struct's fields
                uint bytesToRead = comStat.cbInQue;

                // Or get a ComStat flag like:
                bool fFlag = serialPort.GetComStatFlag(fClearToSendBitPosition);

                // There are no set ComStat fields or flags at this time
            }

            // Serial port is closed when execution leaves this using block
        }
    }
}
