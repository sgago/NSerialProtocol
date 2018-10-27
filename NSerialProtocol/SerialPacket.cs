using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSerialProtocol
{
    public interface ISerialPacket
    {

    }

    [ProtoContract]
    public class SerialPacket : ISerialPacket
    {
        public SerialPacket()
        {

        }
    }
}