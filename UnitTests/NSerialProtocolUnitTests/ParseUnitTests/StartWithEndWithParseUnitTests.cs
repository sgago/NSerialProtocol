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
    public class StartWithEndWithParseUnitTests : ParseUnitTests
    {
        /// <summary>
        /// Gets test cases for the StartWith and EndWith functions verifying packets are properly parsed
        /// using the DefaultParse method.
        /// </summary>
        public static IEnumerable StartWithEndWith_DefaultParse_TestCases
        {
            get
            {
                // Empty delimiter test cases
                yield return new TestCaseData(0, "", "", "").Returns(new string[] { });
                yield return new TestCaseData(0, "", "", "Data").Returns(new string[] { "Data" });

                // Try several permutations with no data, only "+" and "-" delimiters
                yield return new TestCaseData(1, "+", "-", "").Returns(new string[] { });
                yield return new TestCaseData(1, "+", "-", "+").Returns(new string[] { });
                yield return new TestCaseData(1, "+", "-", "-").Returns(new string[] { });
                yield return new TestCaseData(1, "+", "-", "+-").Returns(new string[] { "+-" });
                yield return new TestCaseData(1, "+", "-", "-+").Returns(new string[] { });
                yield return new TestCaseData(1, "+", "-", "-++").Returns(new string[] { });
                yield return new TestCaseData(1, "+", "-", "++--").Returns(new string[] { "++-" });
                yield return new TestCaseData(1, "+", "-", "+-+-").Returns(new string[] { "+-", "+-" });

                // Try several permutations with data
                yield return new TestCaseData(1, "+", "-", "Garbage").Returns(new string[] { });
                yield return new TestCaseData(1, "+", "-", "Garbage+").Returns(new string[] { });
                yield return new TestCaseData(1, "+", "-", "Garbage+Incomplete").Returns(new string[] { });
                yield return new TestCaseData(1, "+", "-", "+Complete-").Returns(new string[] { "+Complete-" });
                yield return new TestCaseData(1, "+", "-", "+Complete-Incomplete").Returns(new string[] { "+Complete-" });
                yield return new TestCaseData(1, "+", "-", "Garbage+Complete-Garbage").Returns(new string[] { "+Complete-" });
                yield return new TestCaseData(1, "+", "-", "Garbage+Complete-+Incomplete").Returns(new string[] { "+Complete-" });
                yield return new TestCaseData(1, "+", "-", "Garbage+Complete-+Complete-").Returns(new string[] { "+Complete-", "+Complete-" });
                yield return new TestCaseData(1, "+", "-", "Garbage+Complete-Garbage+Complete-Garbage")
                    .Returns(new string[] { "+Complete-", "+Complete-" });
                yield return new TestCaseData(1, "+", "-", "Garbage+Complete-Garbage+Complete-Garbage+Incomplete")
                    .Returns(new string[] { "+Complete-", "+Complete-" });

                // Try whole words as starting and ending delimiters
                yield return new TestCaseData(14, "StartDelimiter", "EndDelimiter", "GarbageStartDelimiterCompleteEndDelimiterGarbage")
                    .Returns(new string[] { "StartDelimiterCompleteEndDelimiter" });

                // Try same starting and ending character
                yield return new TestCaseData(1, "|", "|", "").Returns(new string[] { });
                yield return new TestCaseData(1, "|", "|", "|").Returns(new string[] { });
                yield return new TestCaseData(1, "|", "|", "||").Returns(new string[] { "||" });
                yield return new TestCaseData(1, "|", "|", "|a|").Returns(new string[] { "|a|" });
                yield return new TestCaseData(1, "|", "|", "|||").Returns(new string[] { "||" });
                yield return new TestCaseData(1, "|", "|", "||||").Returns(new string[] { "||", "||" });
                yield return new TestCaseData(1, "|", "|", "|||||").Returns(new string[] { "||", "||" });
                yield return new TestCaseData(1, "|", "|", "|a||a|").Returns(new string[] { "|a|", "|a|" });
                yield return new TestCaseData(1, "|", "|", "|a|yyyyyy|a|").Returns(new string[] { "|a|", "|a|" });
                yield return new TestCaseData(1, "|", "|", "|a|yyyyyy|a|yyyyyy").Returns(new string[] { "|a|", "|a|" });
                yield return new TestCaseData(1, "|", "|", "yyyyyy|a|yyyyyy|a|yyyyyy").Returns(new string[] { "|a|", "|a|" });
            }
        }

        [TestCaseSource(nameof(StartWithEndWith_DefaultParse_TestCases))]
        public string[] StartWithEndWith_DefaultParse_Test(int payloadIndex, string startDelimiter, string endDelimiter, string testString)
        {
            ISerialPacketPrototype serialPackets = new SerialPacketPrototype()
                .StartFlag(startDelimiter)
                .EndFlag(endDelimiter)
                .Payload(payloadIndex);
            StringBuilder inputBuffer = new StringBuilder(testString);

            return ParseSerialPackets(serialPackets, inputBuffer).Select(x => x.Value).ToArray();
        }


        public static IEnumerable StartWithEndWith_InputBuffer_TestCases
        {
            get
            {
                // Empty delimiter test cases
                yield return new TestCaseData(0, "", "", "").Returns("");
                yield return new TestCaseData(0, "", "", "Data").Returns("");

                // Try several permutations with no data, only "+" and "-" delimiters
                yield return new TestCaseData(1, "+", "-", "").Returns("");
                yield return new TestCaseData(1, "+", "-", "+").Returns("+");
                yield return new TestCaseData(1, "+", "-", "-").Returns("");
                yield return new TestCaseData(1, "+", "-", "+-").Returns("");
                yield return new TestCaseData(1, "+", "-", "-+").Returns("+");

                // TODO: Wait is this a legal packet?
                yield return new TestCaseData(1, "+", "-", "-++").Returns("++");

                yield return new TestCaseData(1, "+", "-", "++--").Returns("");
                yield return new TestCaseData(1, "+", "-", "+-+-").Returns("");

                // Try several permutations with data
                yield return new TestCaseData(1, "+", "-", "Garbage").Returns("");
                yield return new TestCaseData(1, "+", "-", "Garbage+").Returns("+");
                yield return new TestCaseData(1, "+", "-", "Garbage+Incomplete").Returns("+Incomplete");
                yield return new TestCaseData(1, "+", "-", "+Complete-").Returns("");
                yield return new TestCaseData(1, "+", "-", "+Complete-Incomplete").Returns("");
                yield return new TestCaseData(1, "+", "-", "Garbage+Complete-Garbage").Returns("");
                yield return new TestCaseData(1, "+", "-", "Garbage+Complete-+Incomplete").Returns("+Incomplete");
                yield return new TestCaseData(1, "+", "-", "Garbage+Complete-+Complete-").Returns("");
                yield return new TestCaseData(1, "+", "-", "Garbage+Complete-Garbage+Complete-Garbage").Returns("");
                yield return new TestCaseData(1, "+", "-", "Garbage+Complete-Garbage+Complete-Garbage+Incomplete").Returns("+Incomplete");

                // Try whole words as starting and ending delimiters
                yield return new TestCaseData(14, "StartDelimiter", "EndDelimiter", "GarbageStartDelimiterCompleteEndDelimiterGarbage")
                    .Returns("");

                // Try same starting and ending character
                yield return new TestCaseData(1, "|", "|", "").Returns("");
                yield return new TestCaseData(1, "|", "|", "|").Returns("|");
                yield return new TestCaseData(1, "|", "|", "||").Returns("");
                yield return new TestCaseData(1, "|", "|", "|a|").Returns("");
                yield return new TestCaseData(1, "|", "|", "|||").Returns("|");
                yield return new TestCaseData(1, "|", "|", "||||").Returns("");
                yield return new TestCaseData(1, "|", "|", "|||||").Returns("|");
                yield return new TestCaseData(1, "|", "|", "|a||a|").Returns("");
                yield return new TestCaseData(1, "|", "|", "|a|yyyyyy|a|").Returns("");
                yield return new TestCaseData(1, "|", "|", "|a|yyyyyy|a|yyyyyy").Returns("");
                yield return new TestCaseData(1, "|", "|", "yyyyyy|a|yyyyyy|a|yyyyyy").Returns("");
            }
        }

        [TestCaseSource(nameof(StartWithEndWith_InputBuffer_TestCases))]
        public string StartWithEndWith_InputBuffer_Test(int payloadIndex, string startDelimiter, string endDelimiter, string testString)
        {
            List<string> packets = new List<string>();
            ISerialPacketPrototype serialPackets = new SerialPacketPrototype()
                .StartFlag(startDelimiter)
                .EndFlag(endDelimiter)
                .Payload(payloadIndex);
            StringBuilder inputBuffer = new StringBuilder(testString);

            serialPackets.Parse(inputBuffer);

            return inputBuffer.ToString();
        }
    }
}
