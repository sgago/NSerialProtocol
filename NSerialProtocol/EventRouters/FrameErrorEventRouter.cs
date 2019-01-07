using NSerialProtocol.EventArgs;

namespace NSerialProtocol.EventRouters
{
    public class FrameErrorEventRouter : EventRouter<string>
    {
        public FrameErrorEventRouter(ISerialProtocol protocol)
            : base(protocol)
        {
            protocol.OnFrameError += Protocol_SerialFrameError;
        }

        private void Protocol_SerialFrameError(object sender, SerialFrameErrorEventArgs e)
        {
            string error;

            foreach (IRoute<string> route in Routes)
            {
                error = e.Frame;

                if (route.CanExecute(error))
                {
                    route.Action?.Invoke(error);
                }
            }
        }
    }
}