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
    public class StartWithDynamicLengthParseUnitTests : ParseUnitTests
    {
        public static IEnumerable StartWithDynamicLength_DefaultParse_TestCases
        {
            get
            {
                yield return new TestCaseData(2, "|", 1, "").Returns(new string[] { });
                yield return new TestCaseData(2, "|", 1, "|").Returns(new string[] { });
                yield return new TestCaseData(2, "|", 1, "|2").Returns(new string[] { "|2" });
                yield return new TestCaseData(2, "|", 1, "+2").Returns(new string[] { });
                yield return new TestCaseData(2, "|", 1, "2|").Returns(new string[] { });
                yield return new TestCaseData(2, "|", 1, "|2|3|2").Returns(new string[] { "|2", "|2" });

                // TODO: Wait, is "|3|" a legal packet?
                yield return new TestCaseData(2, "|", 1, "|3|").Returns(new string[] { });

                yield return new TestCaseData(3, "|", 1, "|07This|05is|10dynamic")
                    .Returns(new string[] { "|07This", "|05is", "|10dynamic" });
                yield return new TestCaseData(3, "|", 1, "|07This|05is|10dynamicXXX")
                    .Returns(new string[] { "|07This", "|05is", "|10dynamic" });
                yield return new TestCaseData(3, "|", 1, "XXX|07This|05is|10dynamicXXX")
                    .Returns(new string[] { "|07This", "|05is", "|10dynamic" });
                yield return new TestCaseData(3, "|", 1, "XXX|07This|05isXXX|10dynamicXXX")
                    .Returns(new string[] { "|07This", "|05is", "|10dynamic" });
                yield return new TestCaseData(3, "|", 1, "XXX|07ThisXXX|05isXXX|10dynamicXXX")
                    .Returns(new string[] { "|07This", "|05is", "|10dynamic" });
            }
        }

        [TestCaseSource(nameof(StartWithDynamicLength_DefaultParse_TestCases))]
        public string[] StartWithDynamicLength_DefaultParse_Test(int payloadIndex, string startDelimiter, int startIndex,
            int numberLength, string testString)
        {
            ISerialPacketPrototype serialPackets = new SerialPacketPrototype()
                .StartFlag(startDelimiter)
                .DynamicLength(startIndex)
                .Payload(payloadIndex);
            StringBuilder inputBuffer = new StringBuilder(testString);

            return ParseSerialPackets(serialPackets, inputBuffer).Select(x => x.Value).ToArray();
        }

        public static IEnumerable StartWithDynamicLength_InputBuffer_TestCases
        {
            get
            {
                yield return new TestCaseData(2, "|", 1, "").Returns("");
                yield return new TestCaseData(2, "|", 1, "|").Returns("|");
                yield return new TestCaseData(2, "|", 1, "|2").Returns("");
                yield return new TestCaseData(2, "|", 1, "+2").Returns("");
                yield return new TestCaseData(2, "|", 1, "2|").Returns("|");
                yield return new TestCaseData(2, "|", 1, "|2|3|2").Returns("");

                // TODO: InputBuffer contains "|3|" is this what we want?  Is this a legal packet?
                // This is due to delimiters being evaluated before the dynamic length is checked
                // Matches returns "|3" and "|".  "|3" does not have a length of three.
                // Then we remove up to the first instance of the start delimiter "|".
                // This results in us leaving "|3|" alone.
                // TODO: Is this what we want???
                yield return new TestCaseData(2, "|", 1, "|3|").Returns("|3|");

                yield return new TestCaseData(3, "|", 1, "|07This|05is|10dynamic").Returns("");
                yield return new TestCaseData(3, "|", 1, "|07This|05is|10dynamicXXX").Returns("");
                yield return new TestCaseData(3, "|", 1, "XXX|07This|05is|10dynamicXXX").Returns("");
                yield return new TestCaseData(3, "|", 1, "XXX|07This|05isXXX|10dynamicXXX").Returns("");
                yield return new TestCaseData(3, "|", 1, "XXX|07ThisXXX|05isXXX|10dynamicXXX").Returns("");
            }
        }

        [TestCaseSource(nameof(StartWithDynamicLength_InputBuffer_TestCases))]
        public string StartWithDynamicLength_InputBuffer_Test(int payloadIndex, string startDelimiter, int startIndex,
            string testString)
        {
            List<string> packets = new List<string>();
            ISerialPacketPrototype serialPackets = new SerialPacketPrototype()
                .StartFlag(startDelimiter)
                .DynamicLength(startIndex)
                .Payload(payloadIndex);
            StringBuilder inputBuffer = new StringBuilder(testString);

            serialPackets.Parse(inputBuffer);

            return inputBuffer.ToString();
        }
    }
}
