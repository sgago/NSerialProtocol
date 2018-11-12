using NSerialProtocol;
using NSerialProtocol.FrameParsers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NSerialProtocolUnitTests
{
    // 1st int = Byte position of length in the frame
    // 2nd type = Type of the length
    // 3rd string = the received data
    // 4th string[] = the expected frames that will be parsed
    using ParserTestCaseTuple = Tuple<int, Type, string, string[]>;

    [TestFixture]
    public class VariableLengthParserUnitTests
    {
        public static readonly IEnumerable<ParserTestCaseTuple> VariableLengthParserTestCases
            = new List<ParserTestCaseTuple>
        {   
            // Try null and empty strings as received data
            new ParserTestCaseTuple(0, typeof(byte), null, new string[] {  }),
            new ParserTestCaseTuple(0, typeof(byte), "", new string[] {  }),

            // Try null and empty strings as received data with the length byte
            // starting at byte position 1
            new ParserTestCaseTuple(1, typeof(byte), null, new string[] {  }),
            new ParserTestCaseTuple(1, typeof(byte), "", new string[] {  }),

            // Try length parsing with a very short message, garbage data, etc.
            new ParserTestCaseTuple(1, typeof(byte), "a", new string[] {  }),
            new ParserTestCaseTuple(3, typeof(byte), "abc", new string[] {  }),
            new ParserTestCaseTuple(0, typeof(byte), "\u0001", new string[] { "\u0001" }),
            new ParserTestCaseTuple(0, typeof(byte), "\u0002", new string[] {  }),
            new ParserTestCaseTuple(0, typeof(byte), "\u0002a", new string[] { "\u0002a" }),
            new ParserTestCaseTuple(1, typeof(byte), "a\u0002", new string[] { "a\u0002" }),
            
            // Note that the "g" in "garbage" is processed as a length byte
            new ParserTestCaseTuple(0, typeof(byte), "\u0002agarbage", new string[] { "\u0002a" }),
            // Note that the first "a" in "garbage" is processed as a length byte
            new ParserTestCaseTuple(1, typeof(byte), "a\u0002garbage", new string[] { "a\u0002" }),


            new ParserTestCaseTuple(0, typeof(int), null,
                new string[] {  }),
            new ParserTestCaseTuple(0, typeof(int), "",
                new string[] {  }),
            new ParserTestCaseTuple(0, typeof(int), "\u0008",
                new string[] {  }),
            new ParserTestCaseTuple(0, typeof(int), "\u0008\u0000",
                new string[] {  }),
            new ParserTestCaseTuple(0, typeof(int), "\u0008\u0000\u0000",
                new string[] {  }),
            new ParserTestCaseTuple(0, typeof(int), "\u0008\u0000\u0000\u0000data",
                new string[] { "\u0008\u0000\u0000\u0000data" }),

            // Note that the "garb" in "garbage is processed as a very large integer
            new ParserTestCaseTuple(0, typeof(int), "\u0008\u0000\u0000\u0000datagarbage",
                new string[] { "\u0008\u0000\u0000\u0000data" }),

            // Note that the "garb" in "garbage" is processed as very large integer
            new ParserTestCaseTuple(4, typeof(int), "data\u0008\u0000\u0000\u0000a",
                new string[] { "data\u0008\u0000\u0000\u0000" }),
            new ParserTestCaseTuple(4, typeof(int), "data\u0008\u0000\u0000\u0000garbage",
                new string[] { "data\u0008\u0000\u0000\u0000" }),



        };

        private static IEnumerable<TestCaseData> GetVariableLengthParserTestCaseData()
        {
            foreach (ParserTestCaseTuple data in VariableLengthParserTestCases)
            {
                yield return new TestCaseData(data.Item1, data.Item2, data.Item3)
                    .Returns(data.Item4);
            }
        }

        [Test]
        [TestCaseSource(nameof(GetVariableLengthParserTestCaseData))]
        public string[] VariableLengthParser_Test(int bytePosition, Type lengthType, string data)
        {
            IFrameParser variableLengthParser = new VariableLengthParser(bytePosition, lengthType);

            return variableLengthParser.Parse(data).ToArray();
        }
    }
}
