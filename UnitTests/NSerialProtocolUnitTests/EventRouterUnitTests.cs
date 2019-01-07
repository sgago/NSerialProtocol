using NSerialProtocol;
using NSubstitute;
using NUnit.Framework;
using System;

namespace NSerialProtocolUnitTests
{
    [TestFixture]
    public class EventRouterUnitTests
    {
        [Test]
        public void Route_FrameType_GetAccessor()
        {
            Type expected = typeof(ISerialFrame);
            ISerialProtocol protocolSub = Substitute.For<ISerialProtocol>();

            Route<ISerialFrame> route = new Route<ISerialFrame>(protocolSub, typeof(ISerialFrame));

            Assert.That(route.Type, Is.EqualTo(expected));
        }
    }
}
