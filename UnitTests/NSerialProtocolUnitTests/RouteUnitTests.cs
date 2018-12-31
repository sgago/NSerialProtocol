using NSerialProtocol;
using NSubstitute;
using NUnit.Framework;
using System;

namespace NSerialProtocolUnitTests
{
    [TestFixture]
    public class RouteUnitTests
    {
        [Test]
        public void Route_FrameType_GetAccessor()
        {
            ISerialProtocol protocolSub = Substitute.For<ISerialProtocol>();
            Type expected = typeof(ISerialFrame);

            IRoute route = new Route(protocolSub, typeof(ISerialFrame));

            Assert.That(route.FrameType, Is.EqualTo(expected));
        }

        [Test]
        public void Route_FrameType_SetAccessor()
        {
            ISerialProtocol protocolSub = Substitute.For<ISerialProtocol>();
            Type expected = typeof(SerialFrame);

            IRoute route = new Route(protocolSub, typeof(ISerialFrame))
            {
                FrameType = expected
            };

            Assert.That(route.FrameType, Is.EqualTo(expected));
        }

        [Test]
        public void Route_RouteAction_GetAccessor()
        {
            ISerialProtocol protocolSub = Substitute.For<ISerialProtocol>();
            Action<ISerialFrame> expected = (sf) => { };

            IRoute route = new Route(protocolSub, typeof(ISerialFrame), expected);

            Assert.That(route.Action, Is.EqualTo(expected));
        }

        [Test]
        public void Route_RouteAction_SetAccessor()
        {
            ISerialProtocol protocolSub = Substitute.For<ISerialProtocol>();
            Action<ISerialFrame> expected = (sf) => { };

            IRoute route = new Route(protocolSub, typeof(ISerialFrame))
            {
                Action = expected
            };

            Assert.That(route.Action, Is.EqualTo(expected));
        }
    }
}
