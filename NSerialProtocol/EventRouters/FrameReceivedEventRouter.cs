using NSerialProtocol.EventArgs;

namespace NSerialProtocol.EventRouters
{
    public class FrameReceivedEventRouter : EventRouter<ISerialFrame>
    {
        public FrameReceivedEventRouter(ISerialProtocol protocol)
            : base(protocol)
        {
            protocol.OnFrameReceived += Protocol_SerialFrameReceived;
        }

        private void Protocol_SerialFrameReceived(object sender, SerialFrameReceivedEventArgs e)
        {
            ISerialFrame receivedFrame;

            foreach (IRoute<ISerialFrame> route in Routes)
            {
                if (e.SerialFrame.GetType() == route.Type)
                {
                    receivedFrame = e.SerialFrame as ISerialFrame;

                    if (route.CanExecute(receivedFrame))
                    {
                        route.Action?.Invoke(receivedFrame);
                    }
                }
            }
        }
    }
}