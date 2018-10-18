using NSerialProtocol.Extensions;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSerialProtocol.PacketParsers
{
    //public class EndFlagPacketParser<T> : SerialPacketParser<T> where T : SerialPacket
    //{
    //    public string EndFlag { get; private set; }
    //    public Encoding Encoding { get; private set; } = Encoding.Default;

    //    public delegate void SerialPacketReceivedEventHandler(object sender, SerialPacketReceivedEventArgs<T> e);

    //    public event SerialPacketReceivedEventHandler OnSerialPacketReceived;

    //    public EndFlagPacketParser(EndFlagFrameParser frameParser, string endFlag, Encoding encoding)
    //    {
    //        EndFlag = endFlag;
    //        Encoding = encoding;
    //        frameParser.SerialFrameReceived += FrameParser_OnSerialFrameReceived;
    //        frameParser.SerialFrameError += FrameParser_OnSerialFrameError;
    //    }

    //    private void FrameParser_OnSerialFrameReceived(object sender, SerialFrameReceivedEventArgs e)
    //    {
    //        string originalFrame = e.Frame;
    //        T serialPacket = default(T);

    //        string rawPacket = originalFrame.RemoveFirstOccurenceOf(EndFlag);
    //        byte[] bytes = Encoding.GetBytes(rawPacket);

    //        serialPacket = Deserialize<T>(bytes);

    //        OnSerialPacketReceived?.Invoke(this, new SerialPacketReceivedEventArgs<T>(serialPacket));
    //    }

    //    public static U Deserialize<U>(byte[] data)
    //    {
    //        U serialPacket;

    //        using (var ms = new MemoryStream(data))
    //        {
    //            serialPacket = Serializer.Deserialize<U>(ms);
    //        }

    //        return serialPacket;
    //    }

    //    private void FrameParser_OnSerialFrameError(object sender, SerialFrameErrorEventArgs e)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
