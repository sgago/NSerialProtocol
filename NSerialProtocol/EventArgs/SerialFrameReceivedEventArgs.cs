namespace NSerialProtocol.EventArgs
{
    public class SerialFrameReceivedEventArgs
    {
        public readonly object SerialFrame;

        public SerialFrameReceivedEventArgs(object serialFrame)
        {
            SerialFrame = serialFrame;
        }
    }
}