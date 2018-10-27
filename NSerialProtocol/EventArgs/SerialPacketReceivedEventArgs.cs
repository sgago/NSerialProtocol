namespace NSerialProtocol
{
    public class SerialPacketReceivedEventArgs
    {
        public SerialPacket serialPacket;

        public SerialPacketReceivedEventArgs(SerialPacket serialPacket)
        {
            this.serialPacket = serialPacket;
        }
    }
}