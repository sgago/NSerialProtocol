using NSerialProtocol;

namespace SerialProtocolCallbacks
{
    using NSerialProtocol.Extensions;

    class Program
    {
        static void Main(string[] args)
        {
            ISerialProtocol protocol = new SerialProtocol();

            protocol.OnFrameReceived<ISerialFrame>()
                .Do((sf) =>
                {
                    ISerialFrame frame = sf as ISerialFrame;
                });
        }
    }
}
