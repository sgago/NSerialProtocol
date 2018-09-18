using NUnit.Framework;
using System.Collections;
using System.Linq;

namespace NUnitBug
{
    [TestFixture]
    public class NUnitBug
    {
        private static IEnumerable AsciiCharacters = Enumerable.Range(1, 0x7F).ToList()
            .ConvertAll(x => ((char)x).ToString());


        public static IEnumerable TestCases
        {
            get
            {
                yield return new TestCaseData("\u0000 (null) runs.");

                yield return new TestCaseData("\u0001 (SOH) doesn't run");
                yield return new TestCaseData("\u0002 (STX) doesn't run");
                yield return new TestCaseData("\u0003 (ETX) doesn't run");
                yield return new TestCaseData("\u0004 (EOT) doesn't run");
                yield return new TestCaseData("\u0005 (ENQ) doesn't run");
                yield return new TestCaseData("\u0006 (ACK) doesn't run");
                yield return new TestCaseData("\a (BEL) also run");
                yield return new TestCaseData("\u000A (LF) runs");
                yield return new TestCaseData("\u0021 (NAK) runs");

                yield return new TestCaseData("\u0048 (H) runs");

                yield return new TestCaseData("£ (£) runs");
                yield return new TestCaseData("\u00A3 (£) also runs");
                yield return new TestCaseData("Ḃ (Ḃ) runs");
                yield return new TestCaseData("\u1E02 (Ḃ) also runs");
            }
        }

        [TestCaseSource(nameof(AsciiCharacters))]
        public void Test(string value)
        {

        }

    }
}
