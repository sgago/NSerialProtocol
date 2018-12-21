namespace NSerialProtocol.EventArgs
{
    public class SerialPacketReceivedEventArgs
    {
        public object SerialPacket { get; }

        public SerialPacketReceivedEventArgs(object serialPacket)
        {
            SerialPacket = serialPacket;
        }
    }
}