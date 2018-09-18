namespace NSerialProtocolUnitTests.SerialPacketParseUnitTests
{
    using NSerialProtocol;
    using NUnit.Framework;
    using System.Collections;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Contains unit tests for SerialPacketPrototype class.
    /// </summary>
    [TestFixture]
    public class DynamicLengthParseUnitTests : ParseUnitTests
    {
        /// <summary>
        /// Gets test cases for the DynamicLength function verifying packets are properly parsed
        /// using the DefaultParse method.
        /// </summary>
        public static IEnumerable DynamicLength_DefaultParse_TestCases
        {
            get
            {
                yield return new TestCaseData(0, 0, "").Returns(new string[] { });
                yield return new TestCaseData(1, 0, "2").Returns(new string[] { });
                yield return new TestCaseData(1, 0, "2a").Returns(new string[] { "2a" });
                yield return new TestCaseData(0, 1, "a2").Returns(new string[] { "a2" });
                yield return new TestCaseData(1, 0, "2a2a").Returns(new string[] { "2a", "2a" });
                yield return new TestCaseData(1, 0, "5This3is8dynamic7length")
                    .Returns(new string[] { "5This", "3is", "8dynamic", "7length" });
            }
        }

        [TestCaseSource(nameof(DynamicLength_DefaultParse_TestCases))]
        public string[] DynamicLength_DefaultParse_Test(int payloadIndex, int dynamicLengthIndex, string testString)
        {
            ISerialPacketPrototype serialPackets = new SerialPacketPrototype()
                .DynamicLength(dynamicLengthIndex)
                .Payload(payloadIndex);
            StringBuilder inputBuffer = new StringBuilder(testString);

            return ParseSerialPackets(serialPackets, inputBuffer).Select(x => x.Value).ToArray();
        }

        public static IEnumerable DynamicLength_InputBuffer_TestCases
        {
            get
            {
                yield return new TestCaseData(0, 0, 1, "").Returns("");
                yield return new TestCaseData(1, 0, 1, "2").Returns("2");
                yield return new TestCaseData(1, 0, 1, "2a").Returns("");
                yield return new TestCaseData(0, 1, 1, "a2").Returns("");
                yield return new TestCaseData(1, 0, 1, "2a2a").Returns("");
                yield return new TestCaseData(1, 0, 1, "5This3is8dynamic7length").Returns("");
            }
        }

        [TestCaseSource(nameof(DynamicLength_InputBuffer_TestCases))]
        public string DynamicLength_InputBuffer_Test(int payloadIndex, int dynamicLengthIndex, string testString)
        {
            ISerialPacketPrototype serialPackets = new SerialPacketPrototype()
                .DynamicLength(dynamicLengthIndex, "")
                .Payload(payloadIndex);
            StringBuilder inputBuffer = new StringBuilder(testString);

            serialPackets.Parse(inputBuffer);

            return inputBuffer.ToString();
        }
    }
}
