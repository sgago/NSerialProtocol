using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialFrameExample
{
    using NSerialProtocol;
    using NSerialProtocol.Attributes;

    class MySerialFrame : SerialFrame
    {
        [Payload(1)]
        public string Payload { get; set; } = "";

        [EndFlag]
        public char EndFlag { get; } = char.MinValue;
    }

    class Program
    {
        static void Main(string[] args)
        {
            SerialProtocol protocol = new SerialProtocol();

            MySerialFrame frame = new MySerialFrame();

            protocol.WriteFrame("somedata");
        }
    }
}
