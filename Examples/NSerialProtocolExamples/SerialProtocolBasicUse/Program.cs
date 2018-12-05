using NSerialProtocol;
using NSerialProtocol.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialProtocolBasicUse
{
    class MySerialFrame : SerialFrame
    {
        [Payload(0)]
        public string Payload { get; set; } = "";

        [EndFlag]
        public char EndFlag { get; set; } = '\n';
    }

    class Program
    {
        static void Main(string[] args)
        {
            ISerialProtocol protocol = new SerialProtocol("COM1", 57600)
                .SetFramePrototype<MySerialFrame>();

            /*
             * This writes the byte array
             * [11, T, h, e, P, a, y, l, o, a, d, \n]
             */
            protocol.WriteFrame("ThePayload");
        }
    }
}
