namespace SerialPortFixExample
{
    using SerialPortFix;

    /// <summary>
    /// Serial ports require the use of unmanaged resources.
    /// Therefore, we have to dispose of those resources manually.
    /// This example shows how the using keyword cna dispose of the serial port
    /// automatically.
    /// 
    /// Use this method if the serial port is called infrequently.
    /// </summary>
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

        public static void Main(string[] args)
        {
            string results = "";

            // Instiate a serial port within the using keyword
            using (ISerialPort serialPort = new SerialPortFix(PortName, BaudRate))
            {
                serialPort.Open();

                // TODO: Use the serial port here like you normally would.  Here are some examples:
                serialPort.WriteLine("Send data...");
                results = serialPort.ReadLine();
                System.Console.WriteLine(results);
            }

            // Serial port is closed when execution leaves this using block
        }
    }
}
