using NSerialProtocol;
using NSerialProtocol.FrameParsers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NSerialProtocolUnitTests
{
    // 1st string = start flag
    // 2nd string = end flag
    // 3rd string = data to parse
    // 4th string[] = expected results
    using ParserTestCaseTuple = Tuple<string, string, string, string[]>;

    [TestFixture]
    public class FlagParserUnitTests
    {
        public static readonly IEnumerable<ParserTestCaseTuple> FlagParserTestCases
            = new List<ParserTestCaseTuple>
        {   
            // No Flags test
            new ParserTestCaseTuple("", "", null, new string[] {  }),
            new ParserTestCaseTuple("", "", "", new string[] {  }),
            new ParserTestCaseTuple("", "", "data", new string[] { "data" }),
            new ParserTestCaseTuple("", "", "datadata", new string[] { "datadata" }),
            new ParserTestCaseTuple("", "", "data\r\ndata\n\rdata", new string[] { "data\r\ndata\n\rdata" }),

            // StartFlag tests
            new ParserTestCaseTuple("|", "", null, new string[] {  }),
            new ParserTestCaseTuple("|", "", "", new string[] {  }),
            new ParserTestCaseTuple("|", "", "garbage", new string[] {  }),
            new ParserTestCaseTuple("|", "", "|", new string[] { "|" }),
            new ParserTestCaseTuple("|", "", "||", new string[] { "|", "|" }),
            new ParserTestCaseTuple("|", "", "|data", new string[] { "|data" }),
            new ParserTestCaseTuple("|", "", "||data", new string[] { "|", "|data" }),
            new ParserTestCaseTuple("|", "", "garbage|data", new string[] { "|data" }),
            new ParserTestCaseTuple("|", "", "|data|data", new string[] { "|data", "|data" }),
            new ParserTestCaseTuple("StartFlag", "", "StartFlag", new string[] { "StartFlag" }),
            new ParserTestCaseTuple("StartFlag", "", "StartFlagdata", new string[] { "StartFlagdata" }),
            new ParserTestCaseTuple("StartFlag", "", "garbageStartFlagdata", new string[] { "StartFlagdata" }),
            new ParserTestCaseTuple("StartFlag", "", "StartFlagStartFlagdata", new string[] { "StartFlag", "StartFlagdata" }),
            new ParserTestCaseTuple("StartFlag", "", "garbageStartFlagStartFlagdata", new string[] { "StartFlag", "StartFlagdata" }),

            // EndFlag tests
            new ParserTestCaseTuple("", "\n", null, new string[] {  }),
            new ParserTestCaseTuple("", "\n", "", new string[] {  }),
            new ParserTestCaseTuple("", "\n", "garbage", new string[] {  }),
            new ParserTestCaseTuple("", "\n", "\n", new string[] { "\n" }),
            new ParserTestCaseTuple("", "\n", "\n\n", new string[] { "\n", "\n" }),
            new ParserTestCaseTuple("", "\n", "data\n", new string[] { "data\n" }),
            new ParserTestCaseTuple("", "\n", "\ndata\n", new string[] { "\n", "data\n" }),
            new ParserTestCaseTuple("", "\n", "data\ngarbage", new string[] { "data\n" }),
            new ParserTestCaseTuple("", "\n", "data\ndata\n", new string[] { "data\n", "data\n" }),
            new ParserTestCaseTuple("", "EndFlag", "EndFlag", new string[] { "EndFlag" }),
            new ParserTestCaseTuple("", "EndFlag", "dataEndFlag", new string[] { "dataEndFlag" }),
            new ParserTestCaseTuple("", "EndFlag", "EndFlaggarbage", new string[] { "EndFlag" }),
            new ParserTestCaseTuple("", "EndFlag", "dataEndFlaggarbage", new string[] { "dataEndFlag" }),
            new ParserTestCaseTuple("", "EndFlag", "dataEndFlagEndFlag", new string[] { "dataEndFlag", "EndFlag" }),
            new ParserTestCaseTuple("", "EndFlag", "dataEndFlagdataEndFlag", new string[] { "dataEndFlag", "dataEndFlag" }),

            // StartFlag and EndFlag tests
            new ParserTestCaseTuple("|", "\n", null, new string[] {  }),
            new ParserTestCaseTuple("|", "\n", "", new string[] {  }),
            new ParserTestCaseTuple("|", "\n", "garbage", new string[] {  }),
            new ParserTestCaseTuple("|", "\n", "|garbage", new string[] {  }),
            new ParserTestCaseTuple("|", "\n", "data\n", new string[] {  }),
            new ParserTestCaseTuple("|", "\n", "garbage|data", new string[] {  }),
            new ParserTestCaseTuple("|", "\n", "|\n", new string[] { "|\n" }),
            new ParserTestCaseTuple("|", "\n", "|data\n", new string[] { "|data\n" }),
            new ParserTestCaseTuple("|", "\n", "|data\ngarbage", new string[] { "|data\n" }),
            new ParserTestCaseTuple("|", "\n", "garbage|data\n", new string[] { "|data\n" }),
            new ParserTestCaseTuple("|", "\n", "garbage|data\ngarbage", new string[] { "|data\n" }),
            new ParserTestCaseTuple("|", "\n", "|data\n|data\n", new string[] { "|data\n", "|data\n" }),
            new ParserTestCaseTuple("|", "\n", "garbage|data\n|data\n", new string[] { "|data\n", "|data\n" }),
            new ParserTestCaseTuple("|", "\n", "garbage|data\ngarbage|data\n", new string[] { "|data\n", "|data\n" }),
            new ParserTestCaseTuple("|", "\n", "garbage|data\ngarbage|data\ngarbage", new string[] { "|data\n", "|data\n" }),

            // The same string for Start and End Flags
            new ParserTestCaseTuple("|", "|", null, new string[] {  }),
            new ParserTestCaseTuple("|", "|", "", new string[] {  }),
            new ParserTestCaseTuple("|", "|", "|", new string[] {  }),
            new ParserTestCaseTuple("|", "|", "garbage", new string[] {  }),
            new ParserTestCaseTuple("|", "|", "|garbage", new string[] {  }),
            new ParserTestCaseTuple("|", "|", "garbage|", new string[] {  }),
            new ParserTestCaseTuple("|", "|", "||", new string[] { "||" }),
            new ParserTestCaseTuple("|", "|", "|data|", new string[] { "|data|" }),
            new ParserTestCaseTuple("|", "|", "garbage|data|", new string[] { "|data|" }),
            new ParserTestCaseTuple("|", "|", "|data|garbage", new string[] { "|data|" }),
            new ParserTestCaseTuple("|", "|", "garbage|data|garbage", new string[] { "|data|" }),
            new ParserTestCaseTuple("|", "|", "|data||data|", new string[] { "|data|", "|data|" }),
            new ParserTestCaseTuple("|", "|", "garbage|data||data|", new string[] { "|data|", "|data|" }),
            new ParserTestCaseTuple("|", "|", "garbage|data|garbage|data|", new string[] { "|data|", "|data|" }),
            new ParserTestCaseTuple("|", "|", "garbage|data|garbage|data|garbabe", new string[] { "|data|", "|data|" }),
        
            // Check white space characters
            new ParserTestCaseTuple("\r\n\t \0", "\r\n\t \0", "\r\n\t \0", new string[] {  }),
            new ParserTestCaseTuple("\r\n\t \0", "\r\n\t \0", "garbage", new string[] {  }),
            new ParserTestCaseTuple("\r\n\t \0", "\r\n\t \0", "\r\n\t \0garbage", new string[] {  }),
            new ParserTestCaseTuple("\r\n\t \0", "\r\n\t \0", "garbage\r\n\t \0", new string[] {  }),
            new ParserTestCaseTuple("\r\n\t \0", "\r\n\t \0", "\r\n\t \0\r\n\t \0", new string[] { "\r\n\t \0\r\n\t \0" }),
            new ParserTestCaseTuple("\r\n\t \0", "\r\n\t \0", "\r\n\t \0data\r\n\t \0", new string[] { "\r\n\t \0data\r\n\t \0" }),
            new ParserTestCaseTuple("\r\n\t \0", "\r\n\t \0", "garbage\r\n\t \0data\r\n\t \0", new string[] { "\r\n\t \0data\r\n\t \0" }),
            new ParserTestCaseTuple("\r\n\t \0", "\r\n\t \0", "\r\n\t \0data\r\n\t \0garbage", new string[] { "\r\n\t \0data\r\n\t \0" }),
            new ParserTestCaseTuple("\r\n\t \0", "\r\n\t \0", "garbage\r\n\t \0data\r\n\t \0garbage", new string[] { "\r\n\t \0data\r\n\t \0" }),
            new ParserTestCaseTuple("\r\n\t \0", "\r\n\t \0", "\r\n\t \0data\r\n\t \0\r\n\t \0data\r\n\t \0", new string[] { "\r\n\t \0data\r\n\t \0", "\r\n\t \0data\r\n\t \0" }),
            new ParserTestCaseTuple("\r\n\t \0", "\r\n\t \0", "garbage\r\n\t \0data\r\n\t \0\r\n\t \0data\r\n\t \0", new string[] { "\r\n\t \0data\r\n\t \0", "\r\n\t \0data\r\n\t \0" }),
            new ParserTestCaseTuple("\r\n\t \0", "\r\n\t \0", "garbage\r\n\t \0data\r\n\t \0garbage\r\n\t \0data\r\n\t \0", new string[] { "\r\n\t \0data\r\n\t \0", "\r\n\t \0data\r\n\t \0" }),
            new ParserTestCaseTuple("\r\n\t \0", "\r\n\t \0", "garbage\r\n\t \0data\r\n\t \0garbage\r\n\t \0data\r\n\t \0garbabe", new string[] { "\r\n\t \0data\r\n\t \0", "\r\n\t \0data\r\n\t \0" }),
        };

        private static IEnumerable<TestCaseData> GetFlagParserTestCaseData()
        {
            foreach (ParserTestCaseTuple data in FlagParserTestCases)
            {
                yield return new TestCaseData(data.Item1, data.Item2, data.Item3)
                    .Returns(data.Item4);
            }
        }

        [Test]
        [TestCaseSource(nameof(GetFlagParserTestCaseData))]
        public string[] FlagParser_Test(string startFlag, string  endFlag, string data)
        {
            IFrameParser startFlagParser = new FlagParser(endFlag, startFlag);

            return startFlagParser.Parse(data).ToArray();
        }
    }
}
