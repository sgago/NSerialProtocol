namespace NSerialProtocol
{
    public class SerialFrameErrorEventArgs
    {
        public string Frame { get; }
        public FrameErrorType FrameErrorType { get; }

        public SerialFrameErrorEventArgs(string frame, FrameErrorType frameErrorType)
        {
            Frame = frame;
            FrameErrorType = FrameErrorType;
        }
    }
}