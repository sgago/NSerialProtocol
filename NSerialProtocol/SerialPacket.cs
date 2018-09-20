using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSerialProtocol
{
    [ProtoContract]
    public class SerialPacket
    {
        public SerialPacket()
        {

        }

        [ProtoAfterSerialization]
        public void AfterSerialization()
        {

        }
    }
}
