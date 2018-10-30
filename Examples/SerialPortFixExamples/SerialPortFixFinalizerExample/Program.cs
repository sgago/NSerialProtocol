namespace SerialPortFixExample
{
    using SerialPortFix;

    /// <summary>
    /// Serial ports require the use of unmanaged resources.
    /// Therefore, we have to dispose of those resources manually.
    /// This example shows how with a Finalaizer method.
    /// 
    /// Use this method if the serial port is used more frequently
    /// to save on system resoruces.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Hold onto our SerialPort reference with this property.
        /// </summary>
        static ISerialPortFix SerialPort { get; set; }

        public static void Main(string[] args)
        {
            string results = "";

            // Instantiate a new serial port
            SerialPort = new SerialPortFix("COM1", 9600);

            // Example with the using keyword to automatically close the serial port for us
            SerialPort.Open();

            // TODO: Use the serial port here like you normally would.  Here are some examples:
            SerialPort.WriteLine("Send data...");
            results = SerialPort.ReadLine();
            System.Console.WriteLine(results);

            // Serial port is disposed when execution leaves this using block
        }

        /// <summary>
        /// This finalizer is called when the garbage collector
        /// runs.  This will dispose of the SerialPort's unmanaged resources
        /// for us.
        /// </summary>
        ~Program()
        {
            // Close the serial port if it is not a null reference.
            SerialPort?.Close();
        }
    }
}
