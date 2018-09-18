using NSerialProtocol;
using NSerialProtocol.FrameParsers;
using NSerialProtocol.PacketParsers;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameParserPlayground
{
    [ProtoContract]
    [ProtoInclude(1, typeof(SerialPacket))]
    public class MySerialPacket : SerialPacket
    {
        [ProtoMember(2)]
        public int Payload { get; set; } = 123;

        public MySerialPacket()
        {

        }
    }


    class Program
    {

        static void Main(string[] args)
        {
            byte[] data = null;
            string endFlag = "\n";
            MySerialPacket mySerialPacket = new MySerialPacket();

            EndFlagFrameParser frameParser = new EndFlagFrameParser(endFlag);

            EndFlagPacketParser<MySerialPacket> packetParser =
                new EndFlagPacketParser<MySerialPacket>(frameParser, endFlag, Encoding.Default);

            frameParser.OnSerialFrameReceived += Parser_SerialFrameReceived;
            packetParser.OnSerialPacketReceived += PacketParser_OnSerialPacketReceived;

            //frameParser.Parse("Hello\nABC123\n\n");

            data = Serialize(mySerialPacket);

            frameParser.Parse(Encoding.Default.GetString(data) + endFlag);
        }

        public static byte[] Serialize<T>(T obj)
        {
            byte[] data;
            using (var ms = new MemoryStream())
            {
                Serializer.Serialize(ms, obj);

                data = ms.ToArray();
            }

            return data;
        }

        private static void PacketParser_OnSerialPacketReceived(object sender, SerialPacketReceivedEventArgs<MySerialPacket> e)
        {
            MySerialPacket mySerialPacket = e.serialPacket;
        }

        private static void Parser_SerialFrameReceived(object sender, SerialFrameReceivedEventArgs e)
        {
            Console.WriteLine(e.Frame);
        }
    }
}
