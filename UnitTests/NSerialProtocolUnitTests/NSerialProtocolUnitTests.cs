using NSerialPort;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace NSerialProtocolUnitTests
{
    using NSerialPort.EventArgs;
    using NSerialProtocol;
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
            
            INSerialPort serialPortSub = Substitute.For<INSerialPort>();
            NSerialProtocol protocol = new NSerialProtocol(serialPortSub);

            NSerialDataReceivedEventArgs args = new NSerialDataReceivedEventArgs(
                System.IO.Ports.SerialData.Eof, data);

            protocol.SetStartFlag(startFlag);

            protocol.SerialFrameReceived += (sender, e) =>
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

            INSerialPort serialPortSub = Substitute.For<INSerialPort>();
            NSerialProtocol protocol = new NSerialProtocol(serialPortSub);

            NSerialDataReceivedEventArgs args = new NSerialDataReceivedEventArgs(
                System.IO.Ports.SerialData.Eof, data);

            protocol.SetEndFlag(endFlag);

            protocol.SerialFrameReceived += (sender, e) =>
            {
                framesReceived.Add(e.Frame);
            };

            serialPortSub.DataReceived +=
                Raise.Event<NSerialDataReceivedEventHandler>(args);

            return framesReceived.ToArray();
        }
    }
}
