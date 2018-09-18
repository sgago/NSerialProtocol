namespace NByteStuffUnitTests
{
    using NByteStuff.Algorithms;
    using NUnit.Framework;
    using System.Collections;

    /// <summary>
    /// Contains unit tests for the consistent overhead byte stuffing (COBS) algorithm.
    /// </summary>
    [TestFixture]
    public class CobsUnitTests
    {
        private static string longString = new string('a', 254);

        public static IEnumerable Stuff_TestCases
        {
            get
            {
                yield return new TestCaseData(null).Returns(null);
                yield return new TestCaseData("").Returns("");
                yield return new TestCaseData("\0").Returns("\u0001\u0001");
                yield return new TestCaseData("\0\0").Returns("\u0001\u0001\u0001");
                yield return new TestCaseData("Hello\0World!").Returns("\u0006Hello\u0007World!");
            }
        }

        [TestCaseSource(nameof(Stuff_TestCases))]
        public string Stuff_Test(string text)
        {
            string result = new Cobs().Stuff(text);

            return result;
        }


        [TestCase]
        public void Stuff_LongString_Test()
        {
            string expected = "\u00FF" + longString;
            string actual = new Cobs().Stuff(longString);

            Assert.That(expected, Is.EqualTo(actual));
        }

        [TestCase]
        public void Stuff_MultipleLongStrings_Test()
        {
            string expected = "\u00FF" + longString + "\u00FF" + longString + "\u00FF" + longString;
            string actual = new Cobs().Stuff(longString + longString + longString);

            Assert.That(actual, Is.EqualTo(expected));
        }


        public static IEnumerable Unstuff_TestCases
        {
            get
            {
                yield return new TestCaseData(null).Returns(null);
                yield return new TestCaseData("").Returns("");
                yield return new TestCaseData("\u0001\u0001").Returns("\0");
                yield return new TestCaseData("\u0001\u0001\u0001").Returns("\0\0");
                yield return new TestCaseData("\u0006Hello\u0007World!").Returns("Hello\0World!");
            }
        }

        [TestCaseSource(nameof(Unstuff_TestCases))]
        public string Untuff_Test(string text)
        {
            string result = new Cobs().Unstuff(text);

            return result;
        }



    }
}
