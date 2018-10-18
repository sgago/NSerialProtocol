namespace NSerialProtocol.EventArgs
{
    public class SerialFrameReceivedEventArgs
    {
        public string Frame { get; }

        public SerialFrameReceivedEventArgs(string frame)
        {
            Frame = frame;
        }
    }
}