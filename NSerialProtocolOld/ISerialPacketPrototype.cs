using NByteStuff;
using NFec;
using System;
using System.Text;

namespace NSerialProtocol
{
    public interface ISerialPacketPrototype
    {
        Action<StringBuilder> Parse { get; set; }

        event SerialPacketPrototype.SerialPacketReceivedEventHandler SerialPacketReceived;
        event SerialPacketPrototype.SerialPacketErrorEventHandler SerialPacketError;

        ISerialPacketPrototype AddField(string key, IField field);

        ISerialPacketPrototype AddStaticField(string key, int index, int length, string value, int order);
        ISerialPacketPrototype AddStaticField(string key, int index, int length, string value);

        ISerialPacketPrototype AddDynamicField(string key, int index, string value, int order);
        ISerialPacketPrototype AddDynamicField(string key, int index);

        void RemoveField(string key);

        ISerialPacketPrototype StartFlag(string flag);

        ISerialPacketPrototype EndFlag(string flag);

        ISerialPacketPrototype Payload(int index);

        ISerialPacketPrototype FixedLength(int length);

        ISerialPacketPrototype DynamicLength(int index, string formatSpecifier = "");

        ISerialPacketPrototype Fec(IFec fec, int index, int length);

        ISerialPacketPrototype ByteStuff(IByteStuff byteStuff);

        void CustomParse(Action<StringBuilder> parseAction);

        void DefaultParse(StringBuilder inputBuffer);

        string CreateSerialPacket(string text);
    }

}
