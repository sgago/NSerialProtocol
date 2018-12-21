using NSerialProtocol;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSerialProtocolUnitTests
{
    [TestFixture]
    public class EventRouterUnitTests
    {
        [Test]
        public void Route_FrameType_GetAccessor()
        {
            Type expected = typeof(ISerialFrame);

            Route route = new Route(typeof(ISerialFrame));

            Assert.That(route.FrameType, Is.EqualTo(expected));
        }
    }
}
