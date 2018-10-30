namespace NSerialProtocol.EventArgs
{
    public class SerialFrameErrorEventArgs
    {
        public string Frame { get; }

        public SerialFrameErrorEventArgs(string frame)
        {
            Frame = frame;
        }
    }
}