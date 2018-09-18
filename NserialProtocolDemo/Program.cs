using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSerialProtocolExample
{
    using NSerialProtocol;
    using System.IO;

    [ProtoContract]
    [ProtoInclude(1, typeof(SerialPacket))]
    public class MySerialPacket : SerialPacket
    {
        [ProtoMember(2)]
        public int Payload { get; set; } = 123;

        [ProtoMember(3)]
        public string Payload2 { get; set; } = "abc123";

        public MySerialPacket()
        {

        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            NSerialProtocol protocol = new NSerialProtocol();

            MySerialPacket mySerialPacket = new MySerialPacket();

            protocol.WritePacket(mySerialPacket);

            MySerialPacket newSerialPacket = (MySerialPacket)protocol.ReadPacket();
        }
    }
}
