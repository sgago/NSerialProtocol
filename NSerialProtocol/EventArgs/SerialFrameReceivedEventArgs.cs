namespace NSerialProtocol.EventArgs
{
    public class SerialFrameReceivedEventArgs
    {
        public readonly ISerialFrame SerialFrame;

        public SerialFrameReceivedEventArgs(ISerialFrame serialFrame)
        {
            SerialFrame = serialFrame;
        }
    }
}