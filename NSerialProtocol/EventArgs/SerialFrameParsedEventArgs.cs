namespace NSerialProtocol.EventArgs
{
    public class SerialFrameParsedEventArgs
    {
        public string Frame { get; }

        public SerialFrameParsedEventArgs(string frame)
        {
            Frame = frame;
        }
    }
}