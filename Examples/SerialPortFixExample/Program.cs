namespace SerialPortFixExample
{
    using SerialPortFix;

    /*
     * 
     * Serial ports require the use of unmanaged resources.
     * Therefore, we have to dispose of those resources manually.
     * This example shows two methods of disposing of the serial port
     * resource when the program finishes:
     *   Method 1 - The using keyword which disposes of the serial port
     *   automatically.
     *  
     *  Method 2 - A Finalizer 
     * 
     */
    public class Program
    {
        // Method 2 - Using an object variable (part 1)
        private ISerialPort SerialPort { get; set; }

        public static void Main(string[] args)
        {
            string results = "";

            // Method 1 - With the using keyword to automatically close the serial port
            using (ISerialPort port = new SerialPortFix("COM1", 9600))
            {
                port.Open();

                // TODO: Use the serial port here.

                port.WriteLine("Send data...");
                results = port.ReadLine();
                System.Console.WriteLine(results);
            }
        }

        // Method 2 - Call Close() on the serial port automatically with a finalizer
        ~Program()
        {
            SerialPort.Close();
        }
    }
}
