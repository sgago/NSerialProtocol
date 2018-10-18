namespace NSerialProtocol
{
    public class SerialFrameReceivedEventArgs
    {
        public readonly SerialFrame SerialFrame;

        public SerialFrameReceivedEventArgs(SerialFrame serialFrame)
        {
            SerialFrame = serialFrame;
        }
    }
}