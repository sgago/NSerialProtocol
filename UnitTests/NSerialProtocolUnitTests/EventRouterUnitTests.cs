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

            Route route = new Route(protocolSub, typeof(ISerialFrame));

            Assert.That(route.FrameType, Is.EqualTo(expected));
        }
    }
}
