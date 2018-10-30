using NByteStuff;
using NFec;
using System;
using System.Text;

namespace NSerialProtocol.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class FrameMemberAttribute : Attribute
    {
        public int Tag { get; }

        public FrameMemberAttribute(int tag)
        {
            Tag = tag;
        }
    }

    public class PayloadAttribute : FrameMemberAttribute
    {
        public PayloadAttribute(int tag)
            : base(tag)
        {
        }
    }

    public class StartFlagAttribute : FrameMemberAttribute
    {
        public StartFlagAttribute()
            : base(int.MinValue)
        {

        }
    }

    public class EndFlagAttribute : FrameMemberAttribute
    {
        public EndFlagAttribute()
            : base(int.MaxValue)
        {

        }
    }

    public enum StringSerializationOptions
    {
        LengthPrefix,
        NullTerminated
    }

    public class StringMemberAttribute : FrameMemberAttribute
    {
        public StringMemberAttribute(int tag)
            : base(tag)
        {

        }

        public Encoding Encoding { get; set; } = Encoding.Default;
    }

    public class SerialPacketMemberAttribute : FrameMemberAttribute
    {
        public SerialPacketMemberAttribute(int tag)
            : base(tag)
        {

        }

        public Type LengthPrefixType { get; } = typeof(int);
    }

    public class FecAttribute : Attribute
    {
        //public  Fec { get; }

        //public FecAttribute( fec)
        //{
        //    Fec = fec;
        //}
    }

    public class ByteStuffAttribute : Attribute
    {
        //public  ByteStuff { get; }

        //public ByteStuffAttribute()
        //{
        //    ByteStuff = byteStuff;
        //}
    }
}
