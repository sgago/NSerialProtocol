using NSerialProtocol;
using NSerialProtocol.FrameParsers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

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


        //private const int ExtendedAsciiCodePage = 437;
        //private static readonly Encoding ExtendedAsciiEncoding = Encoding.GetEncoding(ExtendedAsciiCodePage);

        //[Test]
        //public void VariableLengthParser_ParseByteType_Test()
        //{
        //    string expected = ExtendedAsciiEncoding.GetString(
        //        new byte[] { 4, 97, 98, 99 }
        //    );

        //    IFrameParser variableLengthParser = new VariableLengthParser(0, typeof(byte));

        //    string[] actual = variableLengthParser.Parse(
        //        new string[] { expected + expected, expected + expected }
        //    ).ToArray();

        //    Assert.That(actual.All(x => string.Equals(x, expected)), Is.True);
        //}

        //[Test]
        //public void VariableLengthParser_ParseInt32Type_Test()
        //{
        //    string expected = ExtendedAsciiEncoding.GetString(
        //        new byte[] { 4, 0, 0, 0, 97, 98, 99 }
        //    );

        //    IFrameParser variableLengthParser = new VariableLengthParser(0, typeof(int));

        //    string[] actual = variableLengthParser.Parse(
        //        new string[] { expected + expected, expected + expected }
        //    ).ToArray();

        //    Assert.That(actual.All(x => string.Equals(x, expected)), Is.True);
        //}

        private const int ExtendAsciiCodePage = 437;
        private const int LengthBytePosition = 0;

        private static IEnumerable<TestCaseData> GetVariableLengthParser_Types_TestCaseData()
        {
            // Can't use "\u0004" or similar with NUnit and NUnit Test Runner
            // Using a byte array instead, see NUnitBug project for more information

            // 1 byte
            yield return new TestCaseData(typeof(sbyte), new byte[] { 4, 97, 98, 99 });
            yield return new TestCaseData(typeof(byte), new byte[] { 4, 97, 98, 99 });
            yield return new TestCaseData(typeof(char), new byte[] { 4, 97, 98, 99 });

            // 2 bytes
            yield return new TestCaseData(typeof(short), new byte[] { 5, 0, 97, 98, 99 });
            yield return new TestCaseData(typeof(ushort), new byte[] { 5, 0, 97, 98, 99 });

            // 4 bytes
            yield return new TestCaseData(typeof(int), new byte[] { 7, 0, 0, 0, 97, 98, 99 });
            yield return new TestCaseData(typeof(uint), new byte[] { 7, 0, 0, 0, 97, 98, 99 });

            // 8 bytes
            yield return new TestCaseData(typeof(long),
                new byte[] { 12, 0, 0, 0, 0, 0, 0, 0, 0, 97, 98, 99 });
            yield return new TestCaseData(typeof(ulong),
                new byte[] { 12, 0, 0, 0, 0, 0, 0, 0, 0, 97, 98, 99 });
        }

        [Test]
        [TestCaseSource(nameof(GetVariableLengthParser_Types_TestCaseData))]
        public void VariableLengthParser_Types_Test(Type lengthType, byte[] bytes)
        {
            string frame = Encoding.GetEncoding(ExtendAsciiCodePage).GetString(bytes);

            // Make a convoluted test string to parse
            string[] testString = new string[]
            {
                null, "", frame, null, "",
                frame + frame, null, "", frame, null, "",
                frame + frame + frame, null, ""
            };

            IFrameParser variableLengthParser =
                new VariableLengthParser(LengthBytePosition, lengthType);

            string[] actual = variableLengthParser.Parse(testString).ToArray();

            Assert.Multiple(() =>
            {
                // There are 7 "frame" in the test string
                Assert.That(actual.Count(), Is.EqualTo(7));

                // Assert that all 7 are the same thing
                Assert.That(actual.All(x => string.Equals(x, frame)), Is.True);
            });
            
        }

        /// <summary>
        /// Tests that invalid frame lengths less than or equal to zero
        /// ??????????????? what should happen?
        /// </summary>
        [Test]
        public void VariableLengthParse_InvalidLengths_Test()
        {
            IFrameParser variableLengthParser =
                new VariableLengthParser(0, typeof(byte));

            variableLengthParser.Parse("\0");
        }

        /// <summary>
        /// Tests that invalid frame lengths less than or equal to zero
        /// ??????????????? what should happen?
        /// </summary>
        [Test]
        public void VariableLengthParse__Test()
        {
            IFrameParser variableLengthParser =
                new VariableLengthParser(0, typeof(byte));

            variableLengthParser.Parse("\0");
        }
    }
}
