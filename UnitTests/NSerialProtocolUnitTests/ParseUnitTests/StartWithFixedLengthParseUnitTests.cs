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
    public class StartWithFixedLengthParseUnitTests : ParseUnitTests
    {
        /// <summary>
        /// Gets test cases for the StartWith and FixedLength functions verifying packets are properly parsed
        /// using the DefaultParse method.
        /// </summary>
        public static IEnumerable StartWithFixedLength_DefaultParse_TestCases
        {
            get
            {
                yield return new TestCaseData(0, "", 1, "").Returns(new string[] { });
                yield return new TestCaseData(0, "", 2, "aabb").Returns(new string[] { "aa", "bb" });

                yield return new TestCaseData(1, "+", 2, "").Returns(new string[] { });
                yield return new TestCaseData(1, "+", 2, "ab").Returns(new string[] { });
                yield return new TestCaseData(1, "+", 2, "aaa").Returns(new string[] { });
                yield return new TestCaseData(1, "+", 2, "+").Returns(new string[] { });
                yield return new TestCaseData(1, "+", 2, "+a").Returns(new string[] { });
                yield return new TestCaseData(1, "+", 2, "+aa").Returns(new string[] { "+aa" });
                yield return new TestCaseData(1, "+", 2, "+aabbbbbb").Returns(new string[] { "+aa" });
                yield return new TestCaseData(1, "+", 2, "+aabbbbbb+a").Returns(new string[] { "+aa" });
                yield return new TestCaseData(1, "+", 2, "bbbbbb+aa").Returns(new string[] { "+aa" });
                yield return new TestCaseData(1, "+", 2, "+aa+aa").Returns(new string[] { "+aa", "+aa" });
                yield return new TestCaseData(1, "+", 2, "bbbbbb+aa+aa").Returns(new string[] { "+aa", "+aa" });
                yield return new TestCaseData(1, "+", 2, "bbbbbb+aabbbbbb+aa").Returns(new string[] { "+aa", "+aa" });
                yield return new TestCaseData(1, "+", 2, "bbbbbb+aabbbbbb+aabbbbbb").Returns(new string[] { "+aa", "+aa" });
            }
        }

        [TestCaseSource(nameof(StartWithFixedLength_DefaultParse_TestCases))]
        public string[] StartWithFixedLength_DefaultParse_Test(int payloadIndex, string startDelimiter, int length, string testString)
        {
            ISerialPacketPrototype serialPackets = new SerialPacketPrototype()
                .StartFlag(startDelimiter)
                .FixedLength(length)
                .Payload(payloadIndex);
            StringBuilder inputBuffer = new StringBuilder(testString);

            return ParseSerialPackets(serialPackets, inputBuffer).Select(x => x.Value).ToArray();
        }

        public static IEnumerable StartWithFixedLength_InputBuffer_TestCases
        {
            get
            {
                yield return new TestCaseData(0, "", 1, "").Returns("");
                yield return new TestCaseData(0, "", 2, "aabb").Returns("");

                yield return new TestCaseData(1, "+", 2, "").Returns("");
                yield return new TestCaseData(1, "+", 2, "ab").Returns("");
                yield return new TestCaseData(1, "+", 2, "aaa").Returns("");
                yield return new TestCaseData(1, "+", 2, "+").Returns("+");
                yield return new TestCaseData(1, "+", 2, "+a").Returns("+a");
                yield return new TestCaseData(1, "+", 2, "+aa").Returns("");
                yield return new TestCaseData(1, "+", 2, "+aabbbbbb").Returns("");
                yield return new TestCaseData(1, "+", 2, "+aabbbbbb+a").Returns("+a");
                yield return new TestCaseData(1, "+", 2, "bbbbbb+aa").Returns("");
                yield return new TestCaseData(1, "+", 2, "+aa+aa").Returns("");
                yield return new TestCaseData(1, "+", 2, "bbbbbb+aa+aa").Returns("");
                yield return new TestCaseData(1, "+", 2, "bbbbbb+aabbbbbb+aa").Returns("");
                yield return new TestCaseData(1, "+", 2, "bbbbbb+aabbbbbb+aabbbbbb").Returns("");
            }
        }

        [TestCaseSource(nameof(StartWithFixedLength_InputBuffer_TestCases))]
        public string StartWithFixedLength_InputBuffer_Test(int payloadIndex, string startDelimiter, int length, string testString)
        {
            List<string> packets = new List<string>();
            ISerialPacketPrototype serialPackets = new SerialPacketPrototype()
                .StartFlag(startDelimiter)
                .FixedLength(length)
                .Payload(payloadIndex);
            StringBuilder inputBuffer = new StringBuilder(testString);

            serialPackets.Parse(inputBuffer);

            return inputBuffer.ToString();
        }
    }
}
