// TODO: Need to tests with null for arguments

namespace CrcUnitTests
{
    using NFec.Algorithms;
    using NUnit.Framework;
    using System.Collections;

    /// <summary>
    /// Contains unit tests for the Checksum8 forward error correction method.
    /// </summary>
    [TestFixture]
    public class Checksum8AsciiUnitTests
    {
        public static IEnumerable Checksum8Ascii_Compute_TestCases
        {
            get
            {
                yield return new TestCaseData("").Returns("00");
                yield return new TestCaseData("\0").Returns("00");
                yield return new TestCaseData("\0\0").Returns("00");
                yield return new TestCaseData("a").Returns("61");
                yield return new TestCaseData("aa").Returns("C2");
                yield return new TestCaseData("aaa").Returns("23");
                yield return new TestCaseData("\u00FF").Returns("FF");
                yield return new TestCaseData("\u0102").Returns("03");
                yield return new TestCaseData("\u0200").Returns("02");
                yield return new TestCaseData("\u0201").Returns("03");
            }
        }

        [TestCaseSource(nameof(Checksum8Ascii_Compute_TestCases))]
        public string Checksum8Ascii_Compute_Test(string text)
        {
            string result = new Checksum8Ascii().Compute(text);

            return result;
        }
    }
}
