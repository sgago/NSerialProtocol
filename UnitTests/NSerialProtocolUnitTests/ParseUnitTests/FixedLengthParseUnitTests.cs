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
    public class FixedLengthParseUnitTests : ParseUnitTests
    {
        /// <summary>
        /// Gets test cases for the FixedLength function verifying completed packets are properly parsed
        /// using the DefaultParse method.
        /// </summary>
        public static IEnumerable FixedLength_DefaultParse_TestCases
        {
            get
            {
                // Negative length test cases
                yield return new TestCaseData(0, -1, "").Returns(new string[] { });
                yield return new TestCaseData(0, -1, "abc").Returns(new string[] { "abc" });

                // Zero length test cases
                yield return new TestCaseData(0, 0, "").Returns(new string[] { });
                yield return new TestCaseData(0, 0, "abc").Returns(new string[] { "abc" });

                // Try several permutations of FixedLength = 3
                yield return new TestCaseData(0, 3, "").Returns(new string[] { });
                yield return new TestCaseData(0, 3, "ab").Returns(new string[] { });
                yield return new TestCaseData(0, 3, "abc").Returns(new string[] { "abc" });
                yield return new TestCaseData(0, 3, "abcab").Returns(new string[] { "abc" });
                yield return new TestCaseData(0, 3, "abcabc").Returns(new string[] { "abc", "abc" });
                yield return new TestCaseData(0, 3, "abcabcab").Returns(new string[] { "abc", "abc" });

                // Try data with a carriage return
                yield return new TestCaseData(0, 3, "abca\rcab").Returns(new string[] { "abc", "a\rc" });

                // Try data with the soft hypen
                yield return new TestCaseData(0, 3, "abca\u00ADcab").Returns(new string[] { "abc", "a\u00ADc" });
            }
        }

        [TestCaseSource(nameof(FixedLength_DefaultParse_TestCases))]
        public string[] FixedLength_DefaultParse_Test(int payloadIndex, int length, string testString)
        {
            ISerialPacketPrototype serialPackets = new SerialPacketPrototype()
                .FixedLength(length)
                .Payload(payloadIndex);
            StringBuilder inputBuffer = new StringBuilder(testString);

            return ParseSerialPackets(serialPackets, inputBuffer).Select(x => x.Value).ToArray();
        }

        /// <summary>
        /// Gets test cases for the FixedLength function verifying that completed packets are properly removed from the
        /// serial port's input buffer using the DefaultParse method.
        /// </summary>
        public static IEnumerable FixedLength_InputBuffer_TestCases
        {
            get
            {
                // Negative length test cases
                yield return new TestCaseData(0, -1, "").Returns("");
                yield return new TestCaseData(0, -1, "abc").Returns("");

                // Zero length test cases
                yield return new TestCaseData(0, 0, "").Returns("");
                yield return new TestCaseData(0, 0, "abc").Returns("");

                // Try several permutations of FixedLength = 3
                yield return new TestCaseData(0, 3, "").Returns("");
                yield return new TestCaseData(0, 3, "ab").Returns("ab");
                yield return new TestCaseData(0, 3, "abc").Returns("");
                yield return new TestCaseData(0, 3, "abcab").Returns("ab");
                yield return new TestCaseData(0, 3, "abcabc").Returns("");
                yield return new TestCaseData(0, 3, "abcabcab").Returns("ab");

                // Try data with a carriage return
                yield return new TestCaseData(0, 3, "abca\rcab").Returns("ab");

                // Try data with the soft hypen
                yield return new TestCaseData(0, 3, "abca\u00ADcab").Returns("ab");
            }
        }

        [TestCaseSource(nameof(FixedLength_InputBuffer_TestCases))]
        public string FixedLength_InputBuffer_Test(int payloadIndex, int length, string testString)
        {
            List<string> packets = new List<string>();
            ISerialPacketPrototype serialPackets = new SerialPacketPrototype()
                .FixedLength(length)
                .Payload(payloadIndex);
            StringBuilder inputBuffer = new StringBuilder(testString);

            serialPackets.Parse(inputBuffer);

            return inputBuffer.ToString();
        }
    }
}
