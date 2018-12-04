using NSerialProtocol;
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
            Type expected = typeof(ISerialFrame);

            Route route = new Route(typeof(ISerialFrame));

            Assert.That(route.FrameType, Is.EqualTo(expected));
        }

        [Test]
        public void Route_FrameType_SetAccessor()
        {
            Type expected = typeof(SerialFrame);

            Route route = new Route(typeof(ISerialFrame));

            route.FrameType = expected;

            Assert.That(route.FrameType, Is.EqualTo(expected));
        }

        [Test]
        public void Route_RouteAction_GetAccessor()
        {
            Action<ISerialFrame> expected = (sf) => { };

            Route route = new Route(typeof(ISerialFrame), expected);

            Assert.That(route.Action, Is.EqualTo(expected));
        }

        [Test]
        public void Route_RouteAction_SetAccessor()
        {
            Action<ISerialFrame> expected = (sf) => { };

            Route route = new Route(typeof(ISerialFrame));

            route.Action = expected;

            Assert.That(route.Action, Is.EqualTo(expected));
        }
    }
}
