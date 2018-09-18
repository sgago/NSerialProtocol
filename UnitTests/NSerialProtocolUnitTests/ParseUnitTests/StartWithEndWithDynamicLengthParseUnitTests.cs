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
    public class StartWithEndWithDynamicLengthParseUnitTests : ParseUnitTests
    {
        public static IEnumerable StartWithEndWithDynamicLength_DefaultParse_TestCases
        {
            get
            {
                yield return new TestCaseData(2, "|", "\r\n", 1, "").Returns(new string[] { });
                yield return new TestCaseData(2, "|", "\r\n", 1, "|").Returns(new string[] { });
                yield return new TestCaseData(2, "|", "\r\n", 1, "|5").Returns(new string[] { });
                yield return new TestCaseData(2, "|", "\r\n", 1, "|5a\r").Returns(new string[] { });
                yield return new TestCaseData(2, "|", "\r\n", 1, "|5a\r\n").Returns(new string[] { "|5a\r\n" });
                yield return new TestCaseData(2, "|", "\r\n", 1, "|5a\r\nGarbage").Returns(new string[] { "|5a\r\n" });
                yield return new TestCaseData(2, "|", "\r\n", 1, "|5a\r\n|5b").Returns(new string[] { "|5a\r\n" });
                yield return new TestCaseData(2, "|", "\r\n", 1, "|5a\r\n|5b\r\n").Returns(new string[] { "|5a\r\n", "|5b\r\n" });
                yield return new TestCaseData(2, "|", "\r\n", 1, "|5a\r\n|6b\r\n").Returns(new string[] { "|5a\r\n" });
                yield return new TestCaseData(2, "|", "\r\n", 1, "|5a\r\n|5b\r\nGarbage").Returns(new string[] { "|5a\r\n", "|5b\r\n" });
                yield return new TestCaseData(2, "|", "\r\n", 1, "|5a\r\nGarbage|5b\r\nGarbage").Returns(new string[] { "|5a\r\n", "|5b\r\n" });
                yield return new TestCaseData(2, "|", "\r\n", 1, "Garbage|5a\r\nGarbage|5b\r\nGarbage").Returns(new string[] { "|5a\r\n", "|5b\r\n" });

                // Try same starting and ending character
                yield return new TestCaseData(2, "|", "|", 1, "").Returns(new string[] { });
                yield return new TestCaseData(2, "|", "|", 1, "|").Returns(new string[] { });
                yield return new TestCaseData(2, "|", "|", 1, "|3|").Returns(new string[] { "|3|" });
                yield return new TestCaseData(2, "|", "|", 1, "|2|").Returns(new string[] { });
                yield return new TestCaseData(2, "|", "|", 1, "||").Returns(new string[] { });
                yield return new TestCaseData(2, "|", "|", 1, "|||").Returns(new string[] { });
                yield return new TestCaseData(2, "|", "|", 1, "|||").Returns(new string[] { });
                yield return new TestCaseData(2, "|", "|", 1, "|3||4a|").Returns(new string[] { "|3|", "|4a|" });
                yield return new TestCaseData(2, "|", "|", 1, "||||").Returns(new string[] { });
                yield return new TestCaseData(2, "|", "|", 1, "||||").Returns(new string[] { });
                yield return new TestCaseData(2, "|", "|", 1, "|3||4a||").Returns(new string[] { "|3|", "|4a|" });
                yield return new TestCaseData(2, "|", "|", 1, "|||||").Returns(new string[] { });
                yield return new TestCaseData(2, "|", "|", 1, "|3abc|").Returns(new string[] { });
                yield return new TestCaseData(2, "|", "|", 1, "|6abc|").Returns(new string[] { "|6abc|" });
                yield return new TestCaseData(2, "|", "|", 1, "|abc|").Returns(new string[] { });
                yield return new TestCaseData(2, "|", "|", 1, "|6abc||6abc|").Returns(new string[] { "|6abc|", "|6abc|" });
                yield return new TestCaseData(2, "|", "|", 1, "|6abc|yyyyyy|6abc|").Returns(new string[] { "|6abc|", "|6abc|" });
                yield return new TestCaseData(2, "|", "|", 1, "|6abc|yyyyyy|6abc|yyyyyy").Returns(new string[] { "|6abc|", "|6abc|" });
                yield return new TestCaseData(2, "|", "|", 1, "yyyyyy|6abc|yyyyyy|6abc|yyyyyy").Returns(new string[] { "|6abc|", "|6abc|" });
            }
        }

        [TestCaseSource(nameof(StartWithEndWithDynamicLength_DefaultParse_TestCases))]
        public string[] StartWithEndWithDynamicLength_DefaultParse_Test(int payloadIndex, string startDelimiter, string endDelimiter,
            int startIndex, string testString)
        {
            ISerialPacketPrototype serialPackets = new SerialPacketPrototype()
                .StartFlag(startDelimiter)
                .EndFlag(endDelimiter)
                .DynamicLength(startIndex)
                .Payload(payloadIndex);
            StringBuilder inputBuffer = new StringBuilder(testString);

            return ParseSerialPackets(serialPackets, inputBuffer).Select(x => x.Value).ToArray();
        }

        public static IEnumerable StartWithEndWithDynamicLength_InputBuffer_TestCases
        {
            get
            {
                yield return new TestCaseData(2, "|", "\r\n", 1, "").Returns("");
                yield return new TestCaseData(2, "|", "\r\n", 1, "|").Returns("|");
                yield return new TestCaseData(2, "|", "\r\n", 1, "|5").Returns("|5");
                yield return new TestCaseData(2, "|", "\r\n", 1, "|5a\r").Returns("|5a\r");
                yield return new TestCaseData(2, "|", "\r\n", 1, "|5a\r\n").Returns("");
                yield return new TestCaseData(2, "|", "\r\n", 1, "|5a\r\nGarbage").Returns("");
                yield return new TestCaseData(2, "|", "\r\n", 1, "|5a\r\n|5b").Returns("|5b");
                yield return new TestCaseData(2, "|", "\r\n", 1, "|5a\r\n|5b\r\n").Returns("");
                yield return new TestCaseData(2, "|", "\r\n", 1, "|5a\r\n|6b\r\n").Returns(""); // Error, returns "|6b\r\n"
                yield return new TestCaseData(2, "|", "\r\n", 1, "|5a\r\n|5b\r\nGarbage").Returns("");
                yield return new TestCaseData(2, "|", "\r\n", 1, "|5a\r\nGarbage|5b\r\nGarbage").Returns("");
                yield return new TestCaseData(2, "|", "\r\n", 1, "Garbage|5a\r\nGarbage|5b\r\nGarbage").Returns("");
                yield return new TestCaseData(2, "|", "\r\n", 1, "Garbage|5a\r\nGarbage|5b\r\nGarbage|").Returns("|");

                // Try same starting and ending character
                yield return new TestCaseData(2, "|", "|", 1, "").Returns("");
                yield return new TestCaseData(2, "|", "|", 1, "|").Returns("|");
                yield return new TestCaseData(2, "|", "|", 1, "|3|").Returns("");
                yield return new TestCaseData(2, "|", "|", 1, "|2|").Returns("");
                yield return new TestCaseData(2, "|", "|", 1, "||").Returns("");
                yield return new TestCaseData(2, "|", "|", 1, "|||").Returns("|");
                yield return new TestCaseData(2, "|", "|", 1, "|||").Returns("|");
                yield return new TestCaseData(2, "|", "|", 1, "|3||4a|").Returns("");
                yield return new TestCaseData(2, "|", "|", 1, "||||").Returns("");
                yield return new TestCaseData(2, "|", "|", 1, "||||").Returns("");
                yield return new TestCaseData(2, "|", "|", 1, "|3||4a||").Returns("|");
                yield return new TestCaseData(2, "|", "|", 1, "|||||").Returns("|");
                yield return new TestCaseData(2, "|", "|", 1, "|3abc|").Returns("");
                yield return new TestCaseData(2, "|", "|", 1, "|6abc|").Returns("");
                yield return new TestCaseData(2, "|", "|", 1, "|abc|").Returns("");
                yield return new TestCaseData(2, "|", "|", 1, "|6abc||6abc|").Returns("");
                yield return new TestCaseData(2, "|", "|", 1, "|6abc|yyyyyy|6abc|").Returns("");
                yield return new TestCaseData(2, "|", "|", 1, "|6abc|yyyyyy|6abc|yyyyyy").Returns("");
                yield return new TestCaseData(2, "|", "|", 1, "yyyyyy|6abc|yyyyyy|6abc|yyyyyy").Returns("");
            }
        }

        [TestCaseSource(nameof(StartWithEndWithDynamicLength_InputBuffer_TestCases))]
        public string StartWithEndWithDynamicLength_InputBuffer_Test(int payloadIndex, string startDelimiter, string endDelimiter,
            int startIndex, string testString)
        {
            List<string> packets = new List<string>();
            ISerialPacketPrototype serialPackets = new SerialPacketPrototype()
                .StartFlag(startDelimiter)
                .EndFlag(endDelimiter)
                .DynamicLength(startIndex);
            StringBuilder inputBuffer = new StringBuilder(testString);

            serialPackets.Parse(inputBuffer);

            return inputBuffer.ToString();
        }
    }
}
