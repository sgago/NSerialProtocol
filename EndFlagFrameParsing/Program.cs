using NSerialProtocol;
using NSerialProtocol.FrameParsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndFlagFrameParsing
{
    class Program
    {
        class MySerialFrame : SerialFrame
        {
            [SerialFrameMember(1)]
            public string Payload { get; set; } = "";

            [SerialFrameMember(2)]
            public string EndFlag { get; set; } = "\n";
        }

        static SerialFrameSerializer serializer = new SerialFrameSerializer();
        static SerialFrameParser frameParser = new EndFlagFrameParser("\n");


        static void Main(string[] args)
        {
            frameParser.SerialFrameReceived += FrameParser_SerialFrameReceived;

            MySerialFrame mySerialFrame = new MySerialFrame();

            mySerialFrame.Payload = "test";

            byte[] frame = serializer.Serialize(mySerialFrame);

            frameParser.Parse(Encoding.Default.GetString(frame));

        }

        private static void FrameParser_SerialFrameReceived(object sender, SerialFrameReceivedEventArgs e)
        {
            MySerialFrame receivedFrame =
                (MySerialFrame)serializer.Deserialize(typeof(MySerialFrame), Encoding.Default.GetBytes(e.Frame));
        }
    }
}
