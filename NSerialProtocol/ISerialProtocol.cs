using System;

namespace NSerialProtocol
{
    public interface ISerialProtocol
    {
        event SerialProtocol.FrameErrorEventHandler OnFrameError;
        event SerialProtocol.FrameParsedEventHandler OnFrameParsed;
        event SerialProtocol.FrameReceivedEventHandler OnFrameReceived;
        event SerialProtocol.PacketReceivedEventHandler OnPacketReceived;

        object ReadFrame(int timeout = -1);
        object ReadFrame(TimeSpan timeout);
        TFrame ReadFrame<TFrame>(int timeout = -1) where TFrame : ISerialFrame;
        TFrame ReadFrame<TFrame>(TimeSpan timeout) where TFrame : ISerialFrame;
        ISerialPacket ReadPacket();
        ISerialProtocol SetFlags(string endFlag, string startFlag = "");
        ISerialProtocol SetFramePrototype(Type type);
        ISerialProtocol SetFramePrototype<T>() where T : ISerialFrame;
        object TranceiveFrame(ISerialFrame serialFrame, int timeout = -1, int retries = 0);
        object TranceiveFrame(ISerialFrame serialFrame, TimeSpan timeout, int retries = 0);
        TFrame TranceiveFrame<TFrame>(ISerialFrame serialFrame, int timeout = -1, int retries = 0) where TFrame : ISerialFrame;
        TFrame TranceiveFrame<TFrame>(ISerialFrame serialFrame, TimeSpan timeout, int retries = 0) where TFrame : ISerialFrame;
        ISerialPacket TranceivePacket(SerialPacket serialPacket, int timeout = -1, int retries = 0);
        void WriteFrame(ISerialFrame serialFrame);
        void WriteFrame<TPayload>(TPayload payload);
        void WritePacket(ISerialPacket serialPacket);
    }
}