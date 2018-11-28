using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSerialPortTranceiveLineExample
{
    using NSerialPort;
    using System.Threading;

    class Program
    {
        /// <summary>
        /// Name of the COM port to use.
        /// </summary>
        private const string PortName = "COM1";

        /// <summary>
        /// Baud rate of the COM ports.
        /// </summary>
        private const int BaudRate = 57600;

        static void Main(string[] args)
        {
           // string result = "";

            // Create an NSerialPort instance within the using keyword for automatic
            // disposal of unmanaged resources
            //using (INSerialPort serialPort = new NSerialPort(PortName, BaudRate))
            //{
            //    serialPort.Open();

            //    // 
            //    result = serialPort.TranceiveLine("my message here");

            //    Console.Write("Result = " + result);
            //}
            //SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

        }

        //public async void Method()
        //{
        //    string result = "";

        //    using (INSerialPort serialPort = new NSerialPort(PortName, BaudRate))
        //    {
        //        serialPort.Open();

        //        // 
        //        result = await serialPort.TranceiveLineAsync("my message here", -1);

        //        Console.Write("Result = " + result);
        //    }
        //}
    }
}
