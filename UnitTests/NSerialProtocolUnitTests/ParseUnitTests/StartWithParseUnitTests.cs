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
    public class StartWithParseUnitTests : ParseUnitTests
    {
        /// <summary>
        /// Gets test cases for the StartWith function verifying packets are properly parsed
        /// using the DefaultParse method.
        /// </summary>
        public static IEnumerable StartWith_DefaultParse_TestCases
        {
            get
            {
                string end = "Complete";
                string garbage = "Garbage";

                List<string> delimiters = Enumerable.Range(MinDelimiter, MaxDelimiter).Select(x => ((char)x).ToString()).ToList();
                List<string> delimiterTestStrings = new List<string>();
                List<string> delimiterTestResults = new List<string>();

                // Create each delimiter test and results string
                foreach (string delimiter in delimiters)
                {
                    // Replace removes delimiters from our start and end strings
                    delimiterTestStrings.Add(string.Format("{0}{1}{2}",
                        garbage.Replace(delimiter, ""), delimiter, end.Replace(delimiter, "")));

                    delimiterTestResults.Add(string.Format("{0}{1}",
                        delimiter, end.Replace(delimiter, "")));
                }

                // Try several permutations with no data, only '+' delimiter
                yield return new TestCaseData(0, "", "").Returns(new string[] { });
                yield return new TestCaseData(0, "", "Data").Returns(new string[] { "Data" });
                yield return new TestCaseData(1, "+", "").Returns(new string[] { });
                yield return new TestCaseData(1, "+", "-").Returns(new string[] { });
                yield return new TestCaseData(1, "+", "+").Returns(new string[] { "+" });
                yield return new TestCaseData(1, "+", "+-").Returns(new string[] { "+-" });
                yield return new TestCaseData(1, "+", "-+").Returns(new string[] { "+" });
                yield return new TestCaseData(1, "+", "++").Returns(new string[] { "+", "+" });
                yield return new TestCaseData(1, "+", "++-").Returns(new string[] { "+", "+-" });

                // Try several permutations with data and '+' delimiter
                yield return new TestCaseData(1, "+", "Incomplete").Returns(new string[] { });
                yield return new TestCaseData(1, "+", "+Complete").Returns(new string[] { "+Complete" });
                yield return new TestCaseData(1, "+", "Garbage+Complete").Returns(new string[] { "+Complete" });
                yield return new TestCaseData(1, "+", "Garbage+Complete+Complete").Returns(new string[] { "+Complete", "+Complete" });

                // Try the very common carriage return and new line as the delimiter
                yield return new TestCaseData(2, "\r\n", "\r\nComplete").Returns(new string[] { "\r\nComplete" });
                yield return new TestCaseData(2, "\r\n", "\rGarbage\r\nComplete").Returns(new string[] { "\r\nComplete" });
                yield return new TestCaseData(2, "\r\n", "\n\rGarbage\r\nComplete").Returns(new string[] { "\r\nComplete" });

                // Try a whole word "Delimiter" as a delimiter
                yield return new TestCaseData(9, "Delimiter", "GarbageDelimiterCompleteDelimiterComplete")
                    .Returns(new string[] { "DelimiterComplete", "DelimiterComplete" });

                // Try many different characters as delimiters
                for (int i = 0; i < delimiters.Count; i++)
                {
                    yield return new TestCaseData(1, delimiters[i], delimiterTestStrings[i])
                        .Returns(new string[] { delimiterTestResults[i] });
                }
            }
        }

        [TestCaseSource(nameof(StartWith_DefaultParse_TestCases))]
        public string[] StartWith_DefaultParse_Test(int payloadIndex, string startDelimiter, string testString)
        {
            ISerialPacketPrototype serialPackets = new SerialPacketPrototype()
                .StartFlag(startDelimiter)
                .Payload(payloadIndex);
            StringBuilder inputBuffer = new StringBuilder(testString);

            return ParseSerialPackets(serialPackets, inputBuffer).Select(x => x.Value).ToArray();
        }

        /// <summary>
        /// Gets test cases for the StartWith function verifying that completed packets are properly removed from the
        /// serial port's input buffer using the DefaultParse method.
        /// </summary>
        public static IEnumerable StartWith_InputBuffer_TestCases
        {
            get
            {
                // Empty delimiter test cases
                yield return new TestCaseData(0, "", "").Returns("");
                yield return new TestCaseData(0, "", "Data").Returns("");

                // Try several permutations with no data, only '+' delimiter
                yield return new TestCaseData(1, "+", "").Returns("");
                yield return new TestCaseData(1, "+", "-").Returns("");
                yield return new TestCaseData(1, "+", "+").Returns("");
                yield return new TestCaseData(1, "+", "+-").Returns("");
                yield return new TestCaseData(1, "+", "-+").Returns("");
                yield return new TestCaseData(1, "+", "++").Returns("");
                yield return new TestCaseData(1, "+", "++-").Returns("");

                // Try several permutations with data and '+' delimiter
                yield return new TestCaseData(1, "+", "Incomplete").Returns("");
                yield return new TestCaseData(1, "+", "+Complete").Returns("");
                yield return new TestCaseData(1, "+", "Garbage+Complete").Returns("");
                yield return new TestCaseData(1, "+", "Garbage+Complete+Complete").Returns("");

                // Try the very common carriage return and new line as the delimiter
                yield return new TestCaseData(2, "\r\n", "\r\nComplete").Returns("");
                yield return new TestCaseData(2, "\r\n", "\rGarbage\r\nComplete").Returns("");
                yield return new TestCaseData(2, "\r\n", "\n\rGarbage\r\nComplete").Returns("");

                // Try a whole word "Delimiter" as a delimiter
                yield return new TestCaseData(9, "Delimiter", "GarbageDelimiterCompleteDelimiterComplete").Returns("");
            }
        }

        [TestCaseSource(nameof(StartWith_InputBuffer_TestCases))]
        public string StartWith_InputBuffer_Test(int payloadIndex, string startDelimiter, string testString)
        {
            List<string> packets = new List<string>();
            ISerialPacketPrototype serialPackets = new SerialPacketPrototype()
                .StartFlag(startDelimiter)
                .Payload(payloadIndex);
            StringBuilder inputBuffer = new StringBuilder(testString);

            serialPackets.Parse(inputBuffer);

            return inputBuffer.ToString();
        }
    }
}
