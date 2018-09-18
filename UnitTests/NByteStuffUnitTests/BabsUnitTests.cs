namespace NByteStuffUnitTests
{
    using NByteStuff;
    using NByteStuff.Algorithms;
    using NUnit.Framework;
    using System.Collections;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Contains unit tests for the
    /// </summary>
    [TestFixture]
    public class BabsUnitTests
    {
        public static IEnumerable Stuff_TestCases
        {
            get
            {
                //yield return new TestCaseData("\u007F", 128, 127, Encoding.ASCII).Returns("\u0000\u0001");
                //yield return new TestCaseData("\u007F\u007F", 128, 127, Encoding.Unicode).Returns("\u0000\u0001\u0000");

                //yield return new TestCaseData("\r\r",
                //    16, 10, Encoding.Unicode).Returns("\u0000\u0001\u0000");

                // TODO: Should this return "\u0000\u0001" or "\u0000\u0001"?
                // TODO: I suspect it should be "\u0001\u0000"
                //yield return new TestCaseData("\u007F", 128, 127, Encoding.ASCII).Returns("\u0000\u0001");
                //yield return new TestCaseData("\u007F", 128, 127, Encoding.ASCII).Returns("\u0001\u0000");
                //yield return new TestCaseData("\u007D\u007F", 128, 127, Encoding.ASCII).Returns("\u007E\u0000");
                //yield return new TestCaseData("\u007F", 128, 127, Encoding.ASCII).Returns("\u0001\u0000");

                //yield return new TestCaseData("\u0080", 128, 127, Encoding.ASCII).Returns("\u0000\u0001");
                //yield return new TestCaseData("\u0001", 256, 255, Encoding.GetEncoding(1252)).Returns("\u0001");
                yield return new TestCaseData("\u00FE", 256, 255, Encoding.GetEncoding(1252)).Returns("\u00FE");
                yield return new TestCaseData("\u00FF", 256, 255, Encoding.GetEncoding(1252)).Returns("\u0001\u0000");
            }
        }

        [TestCaseSource(nameof(Stuff_TestCases))]
        public string Stuff_Test(string value, int inputBase, int outputBase, Encoding encoding)
        {
            IByteStuff babs = new Babs(inputBase, outputBase, encoding);

            // TODO: Character values must be less than the inputBase - 1.
            // TODO: For instance, we can't represent "\u0080" via 7-bit ASCII.
            if (value.Any(x => x > inputBase - 1))
            {
                throw new System.Exception("Invalid character detected.");
            }

            string result = babs.Stuff(value);
            string unstuff = babs.Unstuff(result);

            return result;
        }

        public static IEnumerable Unstuff_TestCases
        {
            get
            {
                yield return new TestCaseData("", 128, 127, Encoding.ASCII).Returns("\u007F");
            }
        }

        [TestCaseSource(nameof(Unstuff_TestCases))]
        public string Unstuff_Test(string value, int inputBase, int outputBase, Encoding encoding)
        {
            IByteStuff babs = new Babs(inputBase, outputBase, encoding);

            string result = babs.Unstuff("\u0000\u0001");

            return result;
        }
    }
}
