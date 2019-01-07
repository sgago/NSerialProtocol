using NSerialProtocol.EventArgs;

namespace NSerialProtocol.EventRouters
{
    public class PacketReceivedEventRouter : EventRouter<ISerialPacket>
    {
        public PacketReceivedEventRouter(ISerialProtocol protocol)
            : base(protocol)
        {
            protocol.OnPacketReceived += Protocol_SerialPacketReceived;
        }

        private void Protocol_SerialPacketReceived(object sender, SerialPacketReceivedEventArgs e)
        {
            ISerialPacket receivedPacket;

            foreach (IRoute<ISerialPacket> route in Routes)
            {
                if (e.SerialPacket.GetType() == route.Type)
                {
                    receivedPacket = e.SerialPacket as ISerialPacket;

                    if (route.CanExecute(receivedPacket))
                    {
                        route.Action?.Invoke(receivedPacket);
                    }
                }
            }
        }
    }
}
