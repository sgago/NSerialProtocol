using NByteStuff;
using NByteStuff.Algorithms;
using NFec;
using NFec.Algorithms;
using NSerialProtocol.Attributes;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NSerialProtocol
{
    [ProtoContract]
    [ProtoInclude(1, typeof(SerialPacket))]
    public class MySerialPacket : SerialPacket
    {
        [ProtoMember(2)]
        public int Payload { get; set; } = 123;

        public MySerialPacket()
        {

        }

        [ProtoMember(3)]
        public byte? Ecc { get; set; } = null;
    }

    [SerialFrame]
    [MaxLength(50)]
    [FixedLength(10)]
    [ByteStuff(typeof(Cobs))]
    [StartFlag("")]
    [EndFlag(new byte[] { 123 })]
    public class SerialFrame
    {
        [FrameLength(1)]
        public byte[] FrameLength { get; set; }

        [ErrorCorrectionCode(2, typeof(Checksum8))]
        public byte[] ErrorCorrectionCode { get; set; }

        [Payload(3)]
        public SerialPacket Payload { get; set; }

        public bool HasAttribute(Type attributeType)
        {
            return typeof(SerialFrame)
                .GetProperties()
                .Any(prop => Attribute.IsDefined(prop, attributeType));
        }

        public T GetAttribute<T>(Type attributeType) where T : Attribute
        {
            return GetAttribute<T>(attributeType);
        }

        public object GetPropertyValue(Type attributeType)
        {
            PropertyInfo property = typeof(SerialFrame)
                .GetProperties()
                .Where(prop => Attribute.IsDefined(prop, attributeType))
                .First();

            return property?.GetValue(this, null);
        }

        public T GetPropertyValue<T>(Type attributeType)
        {
            PropertyInfo property = typeof(SerialFrame)
                .GetProperties()
                .Where(prop => Attribute.IsDefined(prop, attributeType))
                .First();

            return (T)property?.GetValue(this, null);
        }

        public byte[] SerializePayload()
        {
            byte[] data;
            SerialPacket packet = GetPropertyValue<SerialPacket>(typeof(PayloadAttribute));

            using (MemoryStream memoryStream = new MemoryStream())
            {
                Serializer.Serialize(memoryStream, packet);

                data = memoryStream.GetBuffer();
            }

            return data;
        }

        public byte[] SerializeFrame()
        {
            byte[] startFlag = GetPropertyValue<byte[]>(typeof(StartFlagAttribute));
            byte[] endFlag = GetPropertyValue<byte[]>(typeof(EndFlagAttribute));
            byte[] serializedPacket = SerializePayload();

            throw new NotImplementedException();
        }
    }
}
