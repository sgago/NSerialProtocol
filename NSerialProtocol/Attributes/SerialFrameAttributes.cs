using NByteStuff;
using NFec;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NSerialProtocol.Attributes
{
    public class ByteStuffAttribute : Attribute
    {
        Type ByteStuff { get; set; }

        public ByteStuffAttribute(Type byteStuff)
        {
            ByteStuff = byteStuff;
        }
    }

    public class EndFlagAttribute : Attribute
    {
        public byte[] EndFlag { get; private set; }

        public EndFlagAttribute(byte[] endFlag)
        {
            EndFlag = endFlag;
        }
    }

    public class ErrorCorrectionCodeAttribute : Attribute
    {
        public int Tag { get; set; }
        public Type Fec { get; private set; }

        public ErrorCorrectionCodeAttribute(int tag, Type fec)
        {
            Tag = tag;
            Fec = fec;
        }
    }

    public class FixedLengthAttribute : Attribute
    {
        private int v;

        public FixedLengthAttribute(int v)
        {
            this.v = v;
        }
    }

    public class MaxLengthAttribute : Attribute
    {
        private int v;

        public MaxLengthAttribute(int v)
        {
            this.v = v;
        }
    }

    public class FrameLengthAttribute : Attribute
    {
        public int Tag { get; set; }

        public FrameLengthAttribute(int tag)
        {
            Tag = tag;
        }
    }

    public class PayloadAttribute : Attribute
    {
        public int Tag { get; set; }

        public PayloadAttribute(int tag)
        {
            Tag = tag;
        }
    }

    public class SerialFrameAttribute : Attribute
    {
    }

    public class LengthAttribute : Attribute
    {
    }

    public class StartFlagAttribute : Attribute
    {
        private string v;

        public StartFlagAttribute(string v)
        {
            this.v = v;
        }
    }
}
