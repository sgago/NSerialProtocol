namespace NSerialPort
{
    public class LineReceivedEventArgs
    {
        public string LineReceived { get; }

        public LineReceivedEventArgs(string lineReceived)
        {
            LineReceived = lineReceived;
        }
    }
}