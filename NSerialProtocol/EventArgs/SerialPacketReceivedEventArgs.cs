namespace NSerialProtocol
{
    public class SerialPacketReceivedEventArgs<T>
    {
        public T serialPacket;

        public SerialPacketReceivedEventArgs(T serialPacket)
        {
            this.serialPacket = serialPacket;
        }
    }
}