using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace NUnitBug
{
    [TestFixture]
    public class NUnitBug
    {
        public static IEnumerable<TestCaseData> GetTestCaseData
        {
            get
            {
                yield return new TestCaseData("\0");
                yield return new TestCaseData("\u0001");
                yield return new TestCaseData("\u0002");
                yield return new TestCaseData("\u0003");
                yield return new TestCaseData("\u0004");
                yield return new TestCaseData("\u0005");
                yield return new TestCaseData("\u0097");
            }
        }

        [Test]
        [TestCaseSource(nameof(GetTestCaseData))]
        public void Test(string str)
        {
            
        }
    }
}
