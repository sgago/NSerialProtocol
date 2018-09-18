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
    public class EndWithDynamicLengthParseUnitTests : ParseUnitTests
    {
        public static IEnumerable EndWithDynamicLength_DefaultParse_TestCases
        {
            get
            {
                yield return new TestCaseData(1, "?", 0, "").Returns(new string[] { });
                yield return new TestCaseData(1, "?", 0, "?").Returns(new string[] { });
                yield return new TestCaseData(1, "?", 0, "2?").Returns(new string[] { "2?" });
                yield return new TestCaseData(1, "?", 0, "?2").Returns(new string[] { });
                yield return new TestCaseData(1, "?", 0, "2-").Returns(new string[] { });
                yield return new TestCaseData(1, "?", 0, "2?3?2?").Returns(new string[] { "2?", "2?" });

                yield return new TestCaseData(1, "?", 0, "3??").Returns(new string[] { });

                yield return new TestCaseData(2, "?", 0, "07This?05is?10dynamic?")
                    .Returns(new string[] { "07This?", "05is?", "10dynamic?" });
                yield return new TestCaseData(2, "?", 0, "07This?05is?10dynamic?XXX")
                    .Returns(new string[] { "07This?", "05is?", "10dynamic?" });

                // TODO: The leading XXX causes the following packet to be dropped.
                // We attempt to parse the length from XXX07This? which fails.
                // TODO: Is this correct behaviour?
                yield return new TestCaseData(2, "?", 0, "XXX07This?05is?10dynamic?XXX")
                    .Returns(new string[] { "05is?", "10dynamic?" });
                yield return new TestCaseData(2, "?", 0, "XXX07This?XXX05is?10dynamic?XXX")
                    .Returns(new string[] { "10dynamic?" });
                yield return new TestCaseData(2, "?", 0, "XXX07This?XXX05is?XXX10dynamic?XXX")
                    .Returns(new string[] { });
            }
        }

        [TestCaseSource(nameof(EndWithDynamicLength_DefaultParse_TestCases))]
        public string[] EndWithDynamicLength_DefaultParse_Test(int payloadIndex, string endDelimiter, int startIndex, string testString)
        {
            ISerialPacketPrototype serialPackets = new SerialPacketPrototype()
                .EndFlag(endDelimiter)
                .DynamicLength(startIndex)
                .Payload(payloadIndex);
            StringBuilder inputBuffer = new StringBuilder(testString);

            return ParseSerialPackets(serialPackets, inputBuffer).Select(x => x.Value).ToArray();
        }

        public static IEnumerable EndWithDynamicLength_InputBuffer_TestCases
        {
            get
            {
                yield return new TestCaseData(1, "?", 0, "").Returns("");
                yield return new TestCaseData(1, "?", 0, "?").Returns("");
                yield return new TestCaseData(1, "?", 0, "2?").Returns("");

                //yield return new TestCaseData(1, "?", 0, "?2").Returns("?2");
                yield return new TestCaseData(1, "?", 0, "?2").Returns("2");

                yield return new TestCaseData(1, "?", 0, "2-").Returns("2-");
                yield return new TestCaseData(1, "?", 0, "2?3?2?").Returns("");

                yield return new TestCaseData(1, "?", 0, "3??").Returns("");

                yield return new TestCaseData(2, "?", 0, "07This?05is?10dynamic?")
                    .Returns("");
                yield return new TestCaseData(2, "?", 0, "07This?05is?10dynamic?XXX")
                    .Returns("XXX");

                yield return new TestCaseData(2, "?", 0, "XXX07This?05is?10dynamic?XXX")
                    .Returns("XXX");
                yield return new TestCaseData(2, "?", 0, "XXX07This?XXX05is?10dynamic?XXX")
                    .Returns("XXX");
                yield return new TestCaseData(2, "?", 0, "XXX07This?XXX05is?XXX10dynamic?XXX")
                    .Returns("XXX");
            }
        }

        [TestCaseSource(nameof(EndWithDynamicLength_InputBuffer_TestCases))]
        public string EndWithDynamicLength_InputBuffer_Test(int payloadIndex, string endDelimiter, int startIndex, string testString)
        {
            List<string> packets = new List<string>();
            ISerialPacketPrototype serialPackets = new SerialPacketPrototype()
                .EndFlag(endDelimiter)
                .DynamicLength(startIndex)
                .Payload(payloadIndex);
            StringBuilder inputBuffer = new StringBuilder(testString);

            serialPackets.Parse(inputBuffer);

            return inputBuffer.ToString();
        }
    }
}
