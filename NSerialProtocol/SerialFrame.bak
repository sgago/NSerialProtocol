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
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NSerialProtocol
{
    [ProtoContract]
    [ProtoInclude(1, typeof(SerialPacket))]
    public class MySerialPacket : SerialPacket
    {
        [ProtoMember(2)]
        public int Value { get; set; } = 1234567890;

        public MySerialPacket()
        {

        }
    }

    //[SerialFrame]
    //[MaxLength(50)]
    //[FixedLength(10)]
    //[ByteStuff(typeof(Cobs))]
    //[StartFlag("")]
    //[EndFlag(new byte[] { 123 })]
    public class SerialFrame
    {
        //[FrameLength(1)]
        //public byte[] FrameLength { get; set; }

        //[ErrorCorrectionCode(2, typeof(Checksum8))]
        //public byte[] ErrorCorrectionCode { get; set; }

        //[Payload(3)]
        //public SerialPacket Payload { get; set; }


        //public byte[] SerializePayload()
        //{
        //    byte[] data;
        //    SerialPacket packet = GetPropertyValueByAttributeType<PayloadAttribute, SerialPacket>();

        //    using (MemoryStream memoryStream = new MemoryStream())
        //    {
        //        Serializer.Serialize(memoryStream, packet);

        //        data = memoryStream.GetBuffer();
        //    }

        //    return data;
        //}

        //public byte[] SerializeFrame()
        //{
        //    // Input Attributes: These tell us how to serialize the frame
        //    StartFlagAttribute startFlagAttribute = GetAttributeByType<StartFlagAttribute>();
        //    EndFlagAttribute endFlagAttribute = GetAttributeByType<EndFlagAttribute>();
        //    ByteStuffAttribute byteStuffAttribute = GetAttributeByType<ByteStuffAttribute>();

        //    // Output Properties: These properties will be written to when the frame is serialized
        //    FrameLengthAttribute frameLengthAttribute = GetAttributeByType<FrameLengthAttribute>();
        //    PropertyInfo frameLengthProperty = GetPropertyByAttributeType<FrameLengthAttribute>();
        //    Type propType = frameLengthProperty.PropertyType;

        //    // Get the serialpacket (payload of the frame) in serialized form
        //    byte[] serializedPayload = SerializePayload();

        //    int serializeFrameLength =
        //        (startFlagAttribute?.StartFlag?.Length ?? 0) +
        //        (
        //            Marshal.SizeOf(frameLengthProperty.PropertyType)
        //        ) +
        //        (serializedPayload?.Length ?? 0) +
        //        (endFlagAttribute?.Bytes?.Length ?? 0);

        //    // Calculate size of the serializedFrame array
        //    byte[] serializedFrame = new byte[
        //        serializeFrameLength
        //    ];

        //    // 
        //    //frameLengthProperty?.SetValue(this, BitConverter.GetBytes(serializedFrame.Length));

        //    /*
        //     * 
        //     */


        //    List<SerialFrameMemberAttribute> members = GetAttributesByType<SerialFrameMemberAttribute>()
        //        .OrderBy(x => x.Tag).ToList();


        //    int index = 0;

        //    Array.Copy(startFlagAttribute?.StartFlag, 0, serializedFrame, index, startFlagAttribute?.StartFlag?.Length ?? 0);

        //    index += startFlagAttribute?.StartFlag?.Length ?? 0;

        //    Array.Copy(serializedPayload, 0, serializedFrame, index, serializedPayload.Length);

        //    index += serializedPayload.Length;

        //    Array.Copy(endFlagAttribute?.Bytes, 0, serializedFrame, index, endFlagAttribute?.Bytes?.Length ?? 0);

        //    return serializedFrame;
        //}

        public byte[] SerializePayload()
        {
            byte[] data;
            SerialPacket packet = GetPropertyValueByAttributeType<FramePayloadAttribute, SerialPacket>();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                Serializer.Serialize(memoryStream, packet);

                data = memoryStream.GetBuffer();
            }

            return data;
        }

        public byte[] SerializeFrame()
        {
            byte[] serializedFrame = new byte[0];

            IList<SerialFrameMemberAttribute> members =
                GetAttributesByType<SerialFrameMemberAttribute>().ToList();

            members.Add(GetAttributeByType<FramePayloadAttribute>());

            members.OrderBy(x => x.Tag);


            int frameLength = members.Sum(x => x.GetByteCount());

            return serializedFrame;
        }

        public TAttribute GetAttributeByType<TAttribute>() where TAttribute : Attribute
        {
           return GetType().GetCustomAttribute<TAttribute>(true);
        }

        public IList<TAttribute> GetAttributesByType<TAttribute>() where TAttribute : Attribute
        {
            return GetType().GetCustomAttributes<TAttribute>(true).ToList();
        }

        public PropertyInfo GetPropertyByAttributeType<TAttribute>() where TAttribute : Attribute
        {
            return GetType().GetProperties()
                .Where(x => x.IsDefined(typeof(TAttribute)))
                .First();
        }

        public List<PropertyInfo> GetPropertiesByAttributeType<TAttribute>() where TAttribute : Attribute
        {
            return GetType().GetProperties()
                .Where(x => x.IsDefined(typeof(TAttribute)))
                .ToList();
        }

        public TValue GetPropertyValueByAttributeType<TAttribute, TValue>() where TAttribute : Attribute
        {
            return (TValue)GetPropertyByAttributeType<TAttribute>().GetValue(this);
        }
    }
}
