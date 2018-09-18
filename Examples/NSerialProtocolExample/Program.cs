using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSerialProtocolExample
{
    using NFec.Algorithms;
    using NSerialProtocol;

    class Program
    {
        static void Main(string[] args)
        {
            NSerialProtocol protocol = new NSerialProtocol();

            /*
            
                

             */

            protocol.SerialPackets
                .StartFlag("|")
                .DynamicLength(1)
                .Payload(2)
                .Fec(new Checksum8Ascii(), -2, 1)
                .EndFlag("\n");


            protocol.TranceivePacket("hello");
        }
    }
}
