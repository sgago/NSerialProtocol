
namespace NSerialProtocolUnitTests
{
    using NSerialProtocol;
    using NUnit.Framework;
    using System.Collections;

    [TestFixture]
    public class SerialPacketFormatUnitTest
    {
        public IEnumerable TestCases
        {
            get
            {
                yield return new TestCaseData("a123z").Returns("a123z");
            }
        }

        [TestCaseSource(nameof(TestCases))]
        public string Parse(string value)
        {
            //ISerialPacketConverter packet = new SerialPacketConverter();

            //return "";
            return "";
        }
    }
}
