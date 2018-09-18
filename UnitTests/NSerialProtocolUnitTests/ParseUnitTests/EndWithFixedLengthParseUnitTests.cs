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
    public class EndWithFixedLengthParseUnitTests : ParseUnitTests
    {
        /// <summary>
        /// Gets test cases for the EndWith and FixedLength functions verifying packets are properly parsed
        /// using the DefaultParse method.
        /// </summary>
        public static IEnumerable EndWithFixedLength_DefaultParse_TestCases
        {
            get
            {
                yield return new TestCaseData(0, "", 1, "").Returns(new string[] { });
                yield return new TestCaseData(0, "", 2, "xxyy").Returns(new string[] { "xx", "yy" });

                yield return new TestCaseData(0, "-", 2, "").Returns(new string[] { });
                yield return new TestCaseData(0, "-", 2, "xx").Returns(new string[] { });
                yield return new TestCaseData(0, "-", 2, "xxx").Returns(new string[] { });
                yield return new TestCaseData(0, "-", 2, "-").Returns(new string[] { });
                yield return new TestCaseData(0, "-", 2, "x-").Returns(new string[] { });
                yield return new TestCaseData(0, "-", 2, "xx-").Returns(new string[] { "xx-" });
                yield return new TestCaseData(0, "-", 2, "x--").Returns(new string[] { });
                yield return new TestCaseData(0, "-", 2, "xx-xx").Returns(new string[] { "xx-" });
                yield return new TestCaseData(0, "-", 2, "yyyyyyxx-").Returns(new string[] { "xx-" });
                yield return new TestCaseData(0, "-", 2, "yyyyyyxx-xx").Returns(new string[] { "xx-" });
                yield return new TestCaseData(0, "-", 2, "yyyyyyxx-yyyyyy").Returns(new string[] { "xx-" });
                yield return new TestCaseData(0, "-", 2, "yyyyyyxx-yyyyyyxx").Returns(new string[] { "xx-" });
                yield return new TestCaseData(0, "-", 2, "xx-xx-").Returns(new string[] { "xx-", "xx-" });
                yield return new TestCaseData(0, "-", 2, "xx-xx-xx").Returns(new string[] { "xx-", "xx-" });
                yield return new TestCaseData(0, "-", 2, "xx-xx-yyyyyy").Returns(new string[] { "xx-", "xx-" });
                yield return new TestCaseData(0, "-", 2, "xx-xx-yyyyyyxx").Returns(new string[] { "xx-", "xx-" });
                yield return new TestCaseData(0, "-", 2, "yyyyyyxx-xx-yyyyyy").Returns(new string[] { "xx-", "xx-" });
                yield return new TestCaseData(0, "-", 2, "yyyyyyxx-xx-yyyyyyxx").Returns(new string[] { "xx-", "xx-" });
                yield return new TestCaseData(0, "-", 2, "yyyyyyxx-yyyyyyxx-yyyyyy").Returns(new string[] { "xx-", "xx-" });
                yield return new TestCaseData(0, "-", 2, "yyyyyyxx-yyyyyyxx-yyyyyyxx").Returns(new string[] { "xx-", "xx-" });
            }
        }

        [TestCaseSource(nameof(EndWithFixedLength_DefaultParse_TestCases))]
        public string[] EndWithFixedLength_DefaultParse_Test(int payloadIndex, string endDelimiter, int length, string testString)
        {
            ISerialPacketPrototype serialPackets = new SerialPacketPrototype()
                .EndFlag(endDelimiter)
                .FixedLength(length)
                .Payload(payloadIndex);
            StringBuilder inputBuffer = new StringBuilder(testString);

            return ParseSerialPackets(serialPackets, inputBuffer).Select(x => x.Value).ToArray();
        }

        // TODO: Right now we hold onto the garbage characters.  Should we toss the "yyyyyy" or keep it?
        public static IEnumerable EndWithFixedLength_InputBuffer_TestCases
        {
            get
            {
                yield return new TestCaseData(0, "", 1, "").Returns("");
                yield return new TestCaseData(0, "", 2, "xxyy").Returns("");

                yield return new TestCaseData(0, "-", 2, "").Returns("");
                yield return new TestCaseData(0, "-", 2, "xx").Returns("xx");
                yield return new TestCaseData(0, "-", 2, "xxx").Returns("xxx");

                //yield return new TestCaseData(0, "-", 2, "-").Returns("-");
                //yield return new TestCaseData(0, "-", 2, "x-").Returns("x-");
                //yield return new TestCaseData(0, "-", 2, "x--").Returns("x--");
                yield return new TestCaseData(0, "-", 2, "-").Returns("");
                yield return new TestCaseData(0, "-", 2, "x-").Returns("");
                yield return new TestCaseData(0, "-", 2, "x--").Returns("");

                yield return new TestCaseData(0, "-", 2, "xx-").Returns("");
                yield return new TestCaseData(0, "-", 2, "xx-xx").Returns("xx");
                yield return new TestCaseData(0, "-", 2, "yyyyyyxx-").Returns("");
                yield return new TestCaseData(0, "-", 2, "yyyyyyxx-xx").Returns("xx");
                yield return new TestCaseData(0, "-", 2, "yyyyyyxx-yyyyyy").Returns("yyyyyy");
                yield return new TestCaseData(0, "-", 2, "yyyyyyxx-yyyyyyxx").Returns("yyyyyyxx");
                yield return new TestCaseData(0, "-", 2, "xx-xx-").Returns("");
                yield return new TestCaseData(0, "-", 2, "xx-xx-xx").Returns("xx");
                yield return new TestCaseData(0, "-", 2, "xx-xx-yyyyyy").Returns("yyyyyy");
                yield return new TestCaseData(0, "-", 2, "xx-xx-yyyyyyxx").Returns("yyyyyyxx");
                yield return new TestCaseData(0, "-", 2, "yyyyyyxx-xx-yyyyyy").Returns("yyyyyy");
                yield return new TestCaseData(0, "-", 2, "yyyyyyxx-xx-yyyyyyxx").Returns("yyyyyyxx");
                yield return new TestCaseData(0, "-", 2, "yyyyyyxx-yyyyyyxx-yyyyyy").Returns("yyyyyy");
                yield return new TestCaseData(0, "-", 2, "yyyyyyxx-yyyyyyxx-yyyyyyxx").Returns("yyyyyyxx");
            }
        }

        [TestCaseSource(nameof(EndWithFixedLength_InputBuffer_TestCases))]
        public string EndWithFixedLength_InputBuffer_Test(int payloadIndex, string endDelimiter, int length, string testString)
        {
            List<string> packets = new List<string>();
            ISerialPacketPrototype serialPackets = new SerialPacketPrototype()
                .EndFlag(endDelimiter)
                .FixedLength(length)
                .Payload(payloadIndex);
            StringBuilder inputBuffer = new StringBuilder(testString);

            serialPackets.Parse(inputBuffer);

            return inputBuffer.ToString();
        }
    }
}
