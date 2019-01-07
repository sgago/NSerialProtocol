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

            IRoute<ISerialFrame> route = new Route<ISerialFrame>(protocolSub, typeof(ISerialFrame));

            Assert.That(route.Type, Is.EqualTo(expected));
        }

        [Test]
        public void Route_FrameType_SetAccessor()
        {
            ISerialProtocol protocolSub = Substitute.For<ISerialProtocol>();
            Type expected = typeof(SerialFrame);

            IRoute<ISerialFrame> route = new Route<ISerialFrame>(protocolSub, typeof(ISerialFrame))
            {
                Type = expected
            };

            Assert.That(route.Type, Is.EqualTo(expected));
        }

        [Test]
        public void Route_RouteAction_GetAccessor()
        {
            ISerialProtocol protocolSub = Substitute.For<ISerialProtocol>();
            Action<ISerialFrame> expected = (sf) => { };

            IRoute<ISerialFrame> route = new Route<ISerialFrame>(protocolSub, typeof(ISerialFrame), expected);

            Assert.That(route.Action, Is.EqualTo(expected));
        }

        [Test]
        public void Route_RouteAction_SetAccessor()
        {
            ISerialProtocol protocolSub = Substitute.For<ISerialProtocol>();
            Action<ISerialFrame> expected = (sf) => { };

            IRoute<ISerialFrame> route = new Route<ISerialFrame>(protocolSub, typeof(ISerialFrame))
            {
                Action = expected
            };

            Assert.That(route.Action, Is.EqualTo(expected));
        }


    }
}
