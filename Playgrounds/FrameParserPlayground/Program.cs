using NByteStuff.Algorithms;
using NFec;
using NFec.Algorithms;
using NSerialProtocol;
using NSerialProtocol.Attributes;
using NSerialProtocol.FrameParsers;
using NSerialProtocol.PacketParsers;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace FrameParserPlayground
{
    //[ProtoContract]
    //[ProtoInclude(1, typeof(SerialPacket))]
    //public class MySerialPacket : SerialPacket
    //{
    //    [ProtoMember(2)]
    //    public int Payload { get; set; } = 123;

    //    public MySerialPacket()
    //    {

    //    }
    //}

    //[SerialFrame]
    //[FrameStartFlag(new byte[] { 97 })]
    //[FrameLength(1, typeof(byte))]
    //[FrameErrorCorrectionCode(2, typeof(byte))]
    //[FrameEndFlag(new byte[] { 98 })]
    //[FrameByteStuff(typeof(Babs))]
    //public class MySerialFrame : SerialFrame
    //{
    //    [FramePayload(3)]
    //    public SerialPacket SerialPacket { get; set; }
    //}

    class Data
    {
        public int X = 123;

        public string Y = "Hello";
    }

    class Program
    {

        static void Main(string[] args)
        {
            //byte[] data = null;
            //string endFlag = "\n";

            //MySerialPacket mySerialPacket = new MySerialPacket();

            //EndFlagFrameParser frameParser = new EndFlagFrameParser(endFlag);

            //EndFlagPacketParser<MySerialPacket> packetParser =
            //new EndFlagPacketParser<MySerialPacket>(frameParser, endFlag, Encoding.Default);

            //frameParser.OnSerialFrameReceived += Parser_SerialFrameReceived;
            //packetParser.OnSerialPacketReceived += PacketParser_OnSerialPacketReceived;

            //frameParser.Parse("Hello\nABC123\n\n");

            //data = Serialize(mySerialPacket);

            //frameParser.Parse(Encoding.Default.GetString(data) + endFlag);

            //MySerialFrame serialFrame = new MySerialFrame();
            //serialFrame.FrameLength = new byte[] { 123 };

            //IFec fec = new Checksum8();

            //MySerialFrame myReflectedFrame = Activator.CreateInstance<MySerialFrame>();

            //IFec fec = Activator.CreateInstance<Checksum8>();

            //object o = null;
            //byte[] length = serialFrame.GetPropertyValue<byte[]>(typeof(FrameLengthAttribute));

            //serialFrame.SerializeFrame();

            //Data data = new Data();
            //byte[] output = null;

            //// ***** SERIALIZE ******* //
            //// Grab size of the object
            ////var size = Marshal.SizeOf(data.X);
            //var size = Marshal.SizeOf(typeof(Data));

            //// Both managed and unmanaged buffers required.
            //var bytes = new byte[size];
            //var ptr = Marshal.AllocHGlobal(size);
            //// Copy object byte-to-byte to unmanaged memory.
            //Marshal.StructureToPtr(data, ptr, false);
            //// Copy data from unmanaged memory to managed buffer.
            //Marshal.Copy(ptr, bytes, 0, size);
            //// Release unmanaged memory.
            //Marshal.FreeHGlobal(ptr);

            Data data = new Data();
            byte[] bytes;
            BinaryWriter writer;
            BinaryReader reader;

            using (MemoryStream stream = new MemoryStream())
            {
                writer = new BinaryWriter(stream);

                reader = new BinaryReader(stream);

                writer.Write(data.X);
                writer.Write(((int?)7).Value);
                writer.Write(Encoding.ASCII.GetBytes(data.Y));

                bytes = stream.ToArray();

                
            }
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

        //private static void PacketParser_OnSerialPacketReceived(object sender, SerialPacketReceivedEventArgs<MySerialPacket> e)
        //{
        //    MySerialPacket mySerialPacket = e.serialPacket;
        //}

        //private static void Parser_SerialFrameReceived(object sender, SerialFrameReceivedEventArgs e)
        //{
        //    Console.WriteLine(e.Frame);
        //}
    }
}
