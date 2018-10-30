using ProtoBuf;

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