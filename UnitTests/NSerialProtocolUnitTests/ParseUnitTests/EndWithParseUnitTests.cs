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
    public class EndWithParseUnitTests : ParseUnitTests
    {
        /// <summary>
        /// Gets test cases for the EndWith function verifying packets are properly parsed
        /// using the DefaultParse method.
        /// </summary>
        public static IEnumerable EndWith_DefaultParse_TestCases
        {
            get
            {
                string start = "Complete";
                string end = "Incomplete";

                // List of all delimiters to test
                List<string> delimiters = Enumerable.Range(MinDelimiter, MaxDelimiter)
                    .Select(x => ((char)x).ToString()).ToList();

                List<string> delimiterTestStrings = new List<string>();
                List<string> delimiterTestResults = new List<string>();

                // Create each delimiter test and results string
                foreach (string delimiter in delimiters)
                {
                    // Replace removes delimiters from our start and end strings
                    delimiterTestStrings.Add(string.Format("{0}{1}{2}",
                        start.Replace(delimiter, ""), delimiter, end.Replace(delimiter, "")));

                    delimiterTestResults.Add(string.Format("{0}{1}",
                        start.Replace(delimiter, ""), delimiter));
                }

                // Empty delimiter test cases
                yield return new TestCaseData(0, "", "").Returns(new string[] { });
                yield return new TestCaseData(0, "", "Data").Returns(new string[] { "Data" });

                // Try several permutations with no data, only '-' delimiter
                yield return new TestCaseData(0, "-", "").Returns(new string[] { });
                yield return new TestCaseData(0, "-", "-").Returns(new string[] { "-" });
                yield return new TestCaseData(0, "-", "+").Returns(new string[] { });
                yield return new TestCaseData(0, "-", "-+").Returns(new string[] { "-" });
                yield return new TestCaseData(0, "-", "--").Returns(new string[] { "-", "-" });
                yield return new TestCaseData(0, "-", "--+").Returns(new string[] { "-", "-" });

                // Try several permutations with data and '-' delimiter
                yield return new TestCaseData(0, "-", "Incomplete").Returns(new string[] { });
                yield return new TestCaseData(0, "-", "Complete-").Returns(new string[] { "Complete-" });
                yield return new TestCaseData(0, "-", "Complete-Incomplete").Returns(new string[] { "Complete-" });
                yield return new TestCaseData(0, "-", "Complete-Complete-").Returns(new string[] { "Complete-", "Complete-" });
                yield return new TestCaseData(0, "-", "Complete-Complete-Incomplete").Returns(new string[] { "Complete-", "Complete-" });

                // Try the very common carriage return and new line as the delimiter
                yield return new TestCaseData(0, "\r\n", "Complete\r\nIncomplete").Returns(new string[] { "Complete\r\n" });
                yield return new TestCaseData(0, "\r\n", "Complete\r\nIncomplete\r").Returns(new string[] { "Complete\r\n" });
                yield return new TestCaseData(0, "\r\n", "Complete\r\nIncomplete\n\r").Returns(new string[] { "Complete\r\n" });

                // Try a whole word "Delimiter" as a delimiter
                yield return new TestCaseData(0, "Delimiter", "CompleteDelimiterCompleteDelimiterIncomplete")
                    .Returns(new string[] { "CompleteDelimiter", "CompleteDelimiter" });

                // Try many different characters as delimiters
                for (int i = 0; i < delimiters.Count; i++)
                {
                    yield return new TestCaseData(0, delimiters[i], delimiterTestStrings[i]).Returns(new string[] { delimiterTestResults[i] });
                }
            }
        }

        [TestCaseSource(nameof(EndWith_DefaultParse_TestCases))]
        public string[] EndWith_DefaultParse_Test(int payloadIndex, string endDelimiter, string testString)
        {
            ISerialPacketPrototype serialPackets = new SerialPacketPrototype()
                .EndFlag(endDelimiter)
                .Payload(payloadIndex);
            StringBuilder inputBuffer = new StringBuilder(testString);

            return ParseSerialPackets(serialPackets, inputBuffer).Select(x => x.Value).ToArray();
        }

        /// <summary>
        /// Gets test cases for the EndWith function verifying that completed packets are properly removed from the
        /// serial port's input buffer using the DefaultParse method.
        /// </summary>
        public static IEnumerable EndWith_InputBuffer_TestCases
        {
            get
            {
                // Empty delimiter test cases
                yield return new TestCaseData(0, "", "").Returns("");
                yield return new TestCaseData(0, "", "Data").Returns("");

                // Try several permutations with no data, only '-' delimiter
                yield return new TestCaseData(0, "-", "").Returns("");
                yield return new TestCaseData(0, "-", "-").Returns("");
                yield return new TestCaseData(0, "-", "+").Returns("+");
                yield return new TestCaseData(0, "-", "-+").Returns("+");
                yield return new TestCaseData(0, "-", "--").Returns("");
                yield return new TestCaseData(0, "-", "--+").Returns("+");

                // Try several permutations with data and '-' delimiter
                yield return new TestCaseData(0, "-", "Incomplete").Returns("Incomplete");
                yield return new TestCaseData(0, "-", "Complete-").Returns("");
                yield return new TestCaseData(0, "-", "Complete-Incomplete").Returns("Incomplete");
                yield return new TestCaseData(0, "-", "Complete-Complete-").Returns("");
                yield return new TestCaseData(0, "-", "Complete-Complete-Incomplete").Returns("Incomplete");

                // Try the very common carriage return and new line as the delimiter
                yield return new TestCaseData(0, "\r\n", "Complete\r\nIncomplete").Returns("Incomplete");
                yield return new TestCaseData(0, "\r\n", "Complete\r\nIncomplete\r").Returns("Incomplete\r");
                yield return new TestCaseData(0, "\r\n", "Complete\r\nIncomplete\n\r").Returns("Incomplete\n\r");

                // Try a whole word "Delimiter" as a delimiter
                yield return new TestCaseData(0, "Delimiter", "CompleteDelimiterCompleteDelimiterIncomplete").Returns("Incomplete");
            }
        }

        [TestCaseSource(nameof(EndWith_InputBuffer_TestCases))]
        public string EndWIth_InputBuffer_Test(int payloadIndex, string endDelimiter, string testString)
        {
            ISerialPacketPrototype serialPackets = new SerialPacketPrototype()
                .EndFlag(endDelimiter)
                .Payload(payloadIndex);
            StringBuilder inputBuffer = new StringBuilder(testString);

            serialPackets.Parse(inputBuffer);

            return inputBuffer.ToString();
        }
    }
}
