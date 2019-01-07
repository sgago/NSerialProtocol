using NSerialProtocol.EventArgs;

namespace NSerialProtocol.EventRouters
{
    public class FrameParsedEventRouter : EventRouter<string>
    {
        public FrameParsedEventRouter(ISerialProtocol protocol)
            : base(protocol)
        {
            protocol.OnFrameParsed += Protocol_SerialFrameParsed;
        }

        private void Protocol_SerialFrameParsed(object sender, SerialFrameParsedEventArgs e)
        {
            string parsedFrame;

            foreach (IRoute<string> route in Routes)
            {
                parsedFrame = e.Frame;

                if (route.CanExecute(parsedFrame))
                {
                    route.Action?.Invoke(parsedFrame);
                }
            }
        }
    }
}