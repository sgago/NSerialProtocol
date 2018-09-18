namespace NSerialProtocolUnitTests.SerialPacketParseUnitTests
{
    using NSerialProtocol;
    using NUnit.Framework;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Contains unit tests for SerialPacketPrototype class.
    /// </summary>
    [TestFixture]
    public class StartWithEndWithFixedLengthParseUnitTests : ParseUnitTests
    {
        public static IEnumerable StartWithEndWithFixedLength_DefaultParse_TestCases
        {
            get
            {
                yield return new TestCaseData(0, "", "", 2, "").Returns(new string[] { });
                yield return new TestCaseData(0, "", "", 2, "xxyy").Returns(new string[] { "xx", "yy" });

                yield return new TestCaseData(1, "|", "\r\n", 1, "").Returns(new string[] { });
                yield return new TestCaseData(1, "|", "\r\n", 1, "|").Returns(new string[] { });
                yield return new TestCaseData(1, "|", "\r\n", 1, "|\r\n").Returns(new string[] { });
                yield return new TestCaseData(1, "|", "\r\n", 1, "|a\r\n").Returns(new string[] { "|a\r\n" });

                yield return new TestCaseData(1, "|", "\r\n", 1, "||\r\n").Returns(new string[] { "||\r\n" });
                yield return new TestCaseData(1, "|", "\r\n", 1, "|\r\r\n").Returns(new string[] { "|\r\r\n" });

                yield return new TestCaseData(1, "|", "\r\n", 1, "|a\n\r").Returns(new string[] { });
                yield return new TestCaseData(1, "|", "\r\n", 1, "Garbage|a\r\n").Returns(new string[] { "|a\r\n" });
                yield return new TestCaseData(1, "|", "\r\n", 1, "|a\r\nGarbage").Returns(new string[] { "|a\r\n" });
                yield return new TestCaseData(1, "|", "\r\n", 1, "Garbage|a\r\nGarbage").Returns(new string[] { "|a\r\n" });
                yield return new TestCaseData(1, "|", "\r\n", 1, "Garbage|a\r\n|b").Returns(new string[] { "|a\r\n" });

                // Try same starting and ending character
                yield return new TestCaseData(1, "|", "|", 2, "").Returns(new string[] { });
                yield return new TestCaseData(1, "|", "|", 2, "|").Returns(new string[] { });
                yield return new TestCaseData(1, "|", "|", 0, "||").Returns(new string[] { "||" });
                yield return new TestCaseData(1, "|", "|", 2, "||").Returns(new string[] { });
                yield return new TestCaseData(1, "|", "|", 3, "||").Returns(new string[] { });
                yield return new TestCaseData(1, "|", "|", 2, "|||").Returns(new string[] { });
                yield return new TestCaseData(1, "|", "|", 3, "|||").Returns(new string[] { });
                yield return new TestCaseData(1, "|", "|", 0, "||||").Returns(new string[] { "||", "||" });
                yield return new TestCaseData(1, "|", "|", 2, "||||").Returns(new string[] { });
                yield return new TestCaseData(1, "|", "|", 3, "||||").Returns(new string[] { });
                yield return new TestCaseData(1, "|", "|", 0, "|||||").Returns(new string[] { "||", "||" });
                yield return new TestCaseData(1, "|", "|", 1, "|||||").Returns(new string[] { });
                yield return new TestCaseData(1, "|", "|", 2, "|abc|").Returns(new string[] { });
                yield return new TestCaseData(1, "|", "|", 3, "|abc|").Returns(new string[] { "|abc|" });
                yield return new TestCaseData(1, "|", "|", 4, "|abc|").Returns(new string[] { });
                yield return new TestCaseData(1, "|", "|", 3, "|abc||abc|").Returns(new string[] { "|abc|", "|abc|" });
                yield return new TestCaseData(1, "|", "|", 3, "|abc|yyyyyy|abc|").Returns(new string[] { "|abc|", "|abc|" });
                yield return new TestCaseData(1, "|", "|", 3, "|abc|yyyyyy|abc|yyyyyy").Returns(new string[] { "|abc|", "|abc|" });
                yield return new TestCaseData(1, "|", "|", 3, "yyyyyy|abc|yyyyyy|abc|yyyyyy").Returns(new string[] { "|abc|", "|abc|" });
            }
        }

        [TestCaseSource(nameof(StartWithEndWithFixedLength_DefaultParse_TestCases))]
        public string[] StartWithEndWithFixedLength_DefaultParse_Test(int payloadIndex, string startDelimiter, string endDelimiter,
            int length, string testString)
        {
            ISerialPacketPrototype serialPackets = new SerialPacketPrototype()
                .StartFlag(startDelimiter)
                .EndFlag(endDelimiter)
                .FixedLength(length)
                .Payload(payloadIndex);
            StringBuilder inputBuffer = new StringBuilder(testString);

            return ParseSerialPackets(serialPackets, inputBuffer).Select(x => x.Value).ToArray();
        }

        public static IEnumerable StartWithEndWithFixedLength_InputBuffer_TestCases
        {
            get
            {
                yield return new TestCaseData(0, "", "", 2, "").Returns("");
                yield return new TestCaseData(0, "", "", 2, "xxyy").Returns("");

                yield return new TestCaseData(1, "|", "\r\n", 1, "").Returns("");
                yield return new TestCaseData(1, "|", "\r\n", 1, "|").Returns("|");
                yield return new TestCaseData(1, "|", "\r\n", 1, "|\r\n").Returns("");
                yield return new TestCaseData(1, "|", "\r\n", 1, "|a\r\n").Returns("");

                yield return new TestCaseData(1, "|", "\r\n", 1, "||\r\n").Returns("");
                yield return new TestCaseData(1, "|", "\r\n", 1, "|\r\r\n").Returns("");

                yield return new TestCaseData(1, "|", "\r\n", 1, "|a\n\r").Returns("|a\n\r");
                yield return new TestCaseData(1, "|", "\r\n", 1, "Garbage|a\r\n").Returns("");
                yield return new TestCaseData(1, "|", "\r\n", 1, "|a\r\nGarbage").Returns("");
                yield return new TestCaseData(1, "|", "\r\n", 1, "Garbage|a\r\nGarbage").Returns("");
                yield return new TestCaseData(1, "|", "\r\n", 1, "Garbage|a\r\n|b").Returns("|b");

                // Try same starting and ending character
                yield return new TestCaseData(1, "|", "|", 2, "").Returns("");
                yield return new TestCaseData(1, "|", "|", 2, "|").Returns("|");
                yield return new TestCaseData(1, "|", "|", 0, "||").Returns("");
                yield return new TestCaseData(1, "|", "|", 2, "||").Returns("");
                yield return new TestCaseData(1, "|", "|", 3, "||").Returns("");
                yield return new TestCaseData(1, "|", "|", 2, "|||").Returns("|");
                yield return new TestCaseData(1, "|", "|", 3, "|||").Returns("|");
                yield return new TestCaseData(1, "|", "|", 0, "||||").Returns("");
                yield return new TestCaseData(1, "|", "|", 2, "||||").Returns("");
                yield return new TestCaseData(1, "|", "|", 3, "||||").Returns("");
                yield return new TestCaseData(1, "|", "|", 0, "|||||").Returns("|");
                yield return new TestCaseData(1, "|", "|", 1, "|||||").Returns("|");
                yield return new TestCaseData(1, "|", "|", 2, "|abc|").Returns("");
                yield return new TestCaseData(1, "|", "|", 3, "|abc|").Returns("");
                yield return new TestCaseData(1, "|", "|", 4, "|abc|").Returns("");
                yield return new TestCaseData(1, "|", "|", 3, "|abc||abc|").Returns("");
                yield return new TestCaseData(1, "|", "|", 3, "|abc|yyyyyy|abc|").Returns("");
                yield return new TestCaseData(1, "|", "|", 3, "|abc|yyyyyy|abc|yyyyyy").Returns("");
                yield return new TestCaseData(1, "|", "|", 3, "yyyyyy|abc|yyyyyy|abc|yyyyyy").Returns("");

            }
        }

        [TestCaseSource(nameof(StartWithEndWithFixedLength_InputBuffer_TestCases))]
        public string StartWithEndWithFixedLength_InputBuffer_Test(int payloadIndex, string startDelimiter,
            string endDelimiter, int length, string testString)
        {
            List<string> packets = new List<string>();
            ISerialPacketPrototype serialPackets = new SerialPacketPrototype()
                .StartFlag(startDelimiter)
                .EndFlag(endDelimiter)
                .FixedLength(length)
                .Payload(payloadIndex);
            StringBuilder inputBuffer = new StringBuilder(testString);

            serialPackets.Parse(inputBuffer);

            return inputBuffer.ToString();
        }
    }
}
