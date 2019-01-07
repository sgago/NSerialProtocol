using NSerialPort;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NSerialProtocolUnitTests
{
    using NSerialPort.EventArgs;
    using NSerialProtocol;
    using NSerialProtocol.Attributes;
    using NSerialProtocol.Extensions;
    using ProtoBuf;
    using System.Threading.Tasks;
    using System.Windows;
    using static NSerialPort.NSerialPort;

    [TestFixture]
    public class NSerialProtocolUnitTests
    {
        public static readonly IEnumerable<Tuple<string, string, string[]>> ParseStartFlagTestCases
            = new List<Tuple<string, string, string[]>>
        {
            new Tuple<string, string, string[]>("|", null, new string[] {  }),
            new Tuple<string, string, string[]>("|", "", new string[] {  }),
            new Tuple<string, string, string[]>("|", "garbage", new string[] {  }),
            new Tuple<string, string, string[]>("|", "|testdata", new string[] { "|testdata" }),
            new Tuple<string, string, string[]>("|", "garbage|testdata", new string[] { "|testdata" }),

            new Tuple<string, string, string[]>("|", "|testdata|testdata",
                new string[] { "|testdata", "|testdata" }),
        };

        private static IEnumerable<TestCaseData> GetParseStartFlagTestCaseData()
        {
            foreach (Tuple<string, string, string[]> data in ParseStartFlagTestCases)
            {
                yield return new TestCaseData(data.Item1, data.Item2)
                    .Returns(data.Item3);
            }
        }

        [Test]
        [TestCaseSource(nameof(GetParseStartFlagTestCaseData))]
        public string[] NSerialProtocol_ParseStartFlag_Test(string startFlag, string data)
        {
            List<string> framesReceived = new List<string>();
            
            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            IFrameSerializer serializer = Substitute.For<IFrameSerializer>();
            SerialProtocol protocol = new SerialProtocol(serialPortSub, serializer);

            NSerialDataReceivedEventArgs args = new NSerialDataReceivedEventArgs(
                System.IO.Ports.SerialData.Eof, data);

            protocol.SetFlags("", startFlag);

            protocol.OnFrameParsed += (sender, e) =>
            {
                framesReceived.Add(e.Frame);
            };

            serialPortSub.DataReceived +=
                Raise.Event<NSerialDataReceivedEventHandler>(args);

            return framesReceived.ToArray();
        }




        public static readonly IEnumerable<Tuple<string, string, string[]>> ParseEndFlagTestCases
            = new List<Tuple<string, string, string[]>>
        {
                    new Tuple<string, string, string[]>("\n", null, new string[] {  }),
                    new Tuple<string, string, string[]>("\n", "", new string[] {  }),
                    new Tuple<string, string, string[]>("\n", "garbage", new string[] {  }),
                    new Tuple<string, string, string[]>("\n", "testdata\n", new string[] { "testdata\n" }),
                    new Tuple<string, string, string[]>("\n", "testdata\ngarbage", new string[] { "testdata\n" }),

                    new Tuple<string, string, string[]>("\n", "testdata\ntestdata\n",
                        new string[] { "testdata\n", "testdata\n" }),
        };

        private static IEnumerable<TestCaseData> GetParseEndFlagTestCaseData()
        {
            foreach (Tuple<string, string, string[]> data in ParseEndFlagTestCases)
            {
                yield return new TestCaseData(data.Item1, data.Item2)
                    .Returns(data.Item3);
            }
        }

        [Test]
        [TestCaseSource(nameof(GetParseEndFlagTestCaseData))]
        public string[] NSerialProtocol_ParseEndFlag_Test(string endFlag, string data)
        {
            List<string> framesReceived = new List<string>();

            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            IFrameSerializer serializerSub = Substitute.For<IFrameSerializer>();
            SerialProtocol protocol = new SerialProtocol(serialPortSub, serializerSub);

            NSerialDataReceivedEventArgs args = new NSerialDataReceivedEventArgs(
                System.IO.Ports.SerialData.Eof, data);

            protocol.SetFlags(endFlag);

            protocol.OnFrameParsed += (sender, e) =>
            {
                framesReceived.Add(e.Frame);
            };

            serialPortSub.DataReceived +=
                Raise.Event<NSerialDataReceivedEventHandler>(args);

            return framesReceived.ToArray();
        }

        [Test]
        public void Nsp_Test()
        {
            string data = "|\u0004data\n";
            List<string> framesReceived = new List<string>();

            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            IFrameSerializer serializer = new FrameSerializer();
            SerialProtocol protocol = new SerialProtocol(serialPortSub, serializer);

            NSerialDataReceivedEventArgs args = new NSerialDataReceivedEventArgs(
                System.IO.Ports.SerialData.Eof, data);

            protocol.SetFlags("\n", "|");

            protocol.OnFrameReceived<DefaultSerialFrame>()
                .Do((sf) =>
                    {
                        DefaultSerialFrame receivedFrame = sf as DefaultSerialFrame;

                        MessageBox.Show(receivedFrame.Payload);
                    }
                )
                .If
                (
                    // If Predicate
                    (sf) =>
                    {
                        return (sf as DefaultSerialFrame).EndFlag == '\n';
                    },
                    // Then Do Action
                    (sf) =>
                    {
                        MessageBox.Show("If is working!");
                    }
                );

            serialPortSub.DataReceived +=
                Raise.Event<NSerialDataReceivedEventHandler>(args);
        }

        private class TestFrame : SerialFrame
        {
            [SerialPacket(1)]
            public string Payload { get; set; }

            [EndFlag]
            public char EndFlag { get; set; }
        }

        [Test]
        public void NSerialProtocol_ReadPacket_Test()
        {
            const char endFlag = '\n';
            const string data = "data";

            List<string> framesReceived = new List<string>();

            ISerialPort serialPortSub = Substitute.For<ISerialPort>();
            IFrameSerializer serializerSub = Substitute.For<IFrameSerializer>();
            SerialProtocol protocol = new SerialProtocol(serialPortSub, serializerSub);

            serializerSub.Deserialize(typeof(TestFrame), "").ReturnsForAnyArgs(new TestFrame
            {
                Payload = data,
                EndFlag = endFlag
            });

            NSerialDataReceivedEventArgs args = new NSerialDataReceivedEventArgs(
                System.IO.Ports.SerialData.Eof, data);

            protocol.SetFramePrototype(typeof(TestFrame));

            Task<object> readPacketTask = Task.Run(() => protocol.ReadPacket());

            serialPortSub.DataReceived +=
                Raise.Event<NSerialDataReceivedEventHandler>(args);

            string actual = readPacketTask.Result as string;

            Assert.That(actual, Is.EqualTo(data));
        }

        //[Test]
        //public void SerialFrame_Deserialized_Test()
        //{
        //    ISerialFrame actual = null;
        //    string startFlag = "|";
        //    string endFlag = "\0";
        //    string data = "garbage|\u0008testdata\0moregarbage";

        //    ISerialPort serialPortSub = Substitute.For<ISerialPort>();
        //    ISerialFrameSerializer serializerSub = Substitute.For<ISerialFrameSerializer>();
        //    SerialProtocol protocol = new SerialProtocol(serialPortSub, serializerSub);

        //    NSerialDataReceivedEventArgs args = new NSerialDataReceivedEventArgs(
        //        System.IO.Ports.SerialData.Eof, data);

        //    protocol
        //        .SetFlags(endFlag, startFlag);

        //    protocol.SerialFrameReceived += (sender, e) =>
        //    {
        //        actual = e.SerialFrame;
        //    };

        //    serialPortSub.DataReceived +=
        //        Raise.Event<NSerialDataReceivedEventHandler>(args);

        //    //return actual;
        //}

        //[ProtoContract]
        //[ProtoInclude(1, typeof(SerialPacket))]
        //public class MySerialPacket : SerialPacket
        //{
        //    [ProtoMember(2)]
        //    public string StringData;

        //    [ProtoMember(3)]
        //    public int IntData;

        //    public MySerialPacket()
        //    {

        //    }
        //}

        
        //public class MySerialFrame : SerialFrame
        //{
        //    [StartFlag]
        //    public char StartFlag { get; set; } = '|';

        //    //[FrameMember(1)]
        //    [SerialPacketMember(1, LengthPrefixType = typeof(int))]
        //    public MySerialPacket SerialPacket { get; set; } = new MySerialPacket();

        //    [EndFlag]
        //    public char EndFlag { get; set; } = '\n';
        //}

        //[Test]
        //public void SerialPacket_Serialize_Test()
        //{
        //    MySerialFrame mySerialFrame = new MySerialFrame();

        //    mySerialFrame.SerialPacket.StringData = "testdata";
        //    mySerialFrame.SerialPacket.IntData = 1234567890;

        //    SerialFrameSerializer serialFrameSerializer = new SerialFrameSerializer();

        //    byte[] bytes = serialFrameSerializer.Serialize(mySerialFrame);

        //    MySerialFrame result = (MySerialFrame)serialFrameSerializer.Deserialize(typeof(MySerialFrame), bytes);

        //    MySerialFrame result2 = serialFrameSerializer.Deserialize<MySerialFrame>(bytes);
        //}


        //public class CastSerialFrame : SerialFrame
        //{
        //    [StartFlag]
        //    public char StartFlag { get; set; } = '|';

        //    [FrameMember(1)]
        //    public string Data { get; set; }

        //    [EndFlag]
        //    public char EndFlag { get; set; } = '\n';
        //}

        //[Test]
        //public void SerialPacket_GetFrame_Test()
        //{
        //    ISerialFrame actual = null;
        //    string startFlag = "|";
        //    string endFlag = "\n";
        //    string data = "garbage|\u0008testdata\nmoregarbage";

        //    ISerialPort serialPortSub = Substitute.For<ISerialPort>();
        //    ISerialFrameSerializer serializerSub = Substitute.For<ISerialFrameSerializer>();
        //    SerialProtocol protocol = new SerialProtocol(serialPortSub, serializerSub);

        //    NSerialDataReceivedEventArgs args = new NSerialDataReceivedEventArgs(
        //        System.IO.Ports.SerialData.Eof, data);

        //    protocol.SetFlags(endFlag, startFlag);

        //    protocol.SerialFrameReceived += (sender, e) =>
        //    {
        //        actual = e.SerialFrame;
        //    };

        //    serialPortSub.DataReceived +=
        //        Raise.Event<NSerialDataReceivedEventHandler>(args);

        //    DefaultSerialFrame castedFrame = actual as DefaultSerialFrame;

        //    //return actual;
        //}

    }
}
