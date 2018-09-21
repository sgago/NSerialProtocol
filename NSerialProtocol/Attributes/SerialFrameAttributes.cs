using NByteStuff;
using NFec;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NSerialProtocol.Attributes
{

    // SerialFrameConstantAttribute?
    // SerialFrameVariableAttribute?


    public abstract class SerialFrameMemberAttribute : Attribute
    {
        public int Tag { get; set; }

        public SerialFrameMemberAttribute(int tag)
        {
            Tag = tag;
        }

        public abstract int GetByteCount();
    }








    public class SerialFrameConstantAttribute : SerialFrameMemberAttribute
    {
        public byte[] Bytes = new byte[0];

        public SerialFrameConstantAttribute(int tag, byte[] bytes)
            : base(tag)
        {

        }

        public override int GetByteCount()
        {
            throw new NotImplementedException();
        }
    }


    public class SerialFrameVariableAttribute : SerialFrameMemberAttribute
    {
        public PropertyInfo Property { get; set; }

        public SerialFrameVariableAttribute(int tag, Type propertyType, string propertyName)
            : base(tag)
        {

        }

        public PropertyInfo GetProperty<T>(string propertyName)
        {
            return typeof(T).GetProperty(propertyName);
        }

        public Type GetPropertyType<T>(string propertyName)
        {
            return GetProperty<T>(propertyName).PropertyType;
        }


        public override int GetByteCount()
        {
            throw new NotImplementedException();
        }
    }




    public class SerialFrameDataAttribute : SerialFrameMemberAttribute
    {
        public byte[] Bytes { get; set; }

        public SerialFrameDataAttribute(int tag, byte[] bytes)
            : base(tag)
        {
            Bytes = bytes ?? new byte[0];
        }

        public override int GetByteCount()
        {
            return Bytes?.Length ?? 0;
        }
    }





    public class SerialFrameDescriptorAttribute : Attribute
    {

    }


    public class FrameByteStuffAttribute : Attribute
    {
        Type ByteStuff { get; set; }

        public FrameByteStuffAttribute(Type byteStuffType)
        {
            ByteStuff = byteStuffType;
        }
    }

    public class FrameEndFlagAttribute : SerialFrameDataAttribute
    {
        public FrameEndFlagAttribute(byte[] endFlag)
            : base(int.MaxValue, endFlag)
        {

        }
    }

    public class FrameErrorCorrectionCodeAttribute : SerialFrameMemberAttribute
    {
        public Type Fec { get; private set; }

        public FrameErrorCorrectionCodeAttribute(int tag, Type fec)
            : base(tag)
        {
            Fec = fec;
        }

        public override int GetByteCount()
        {
            return Marshal.SizeOf(Fec);
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

    public class FrameLengthAttribute : SerialFrameMemberAttribute
    {
        public Type Type { get; }

        public FrameLengthAttribute(int tag, Type type)
            : base(tag)
        {
            Type = type;
        }

        public override int GetByteCount()
        {


            return Marshal.SizeOf(Type);
        }
    }

    public class FramePayloadAttribute : SerialFrameMemberAttribute
    {
        public FramePayloadAttribute(int tag)
            : base(tag)
        {
            
        }

        public override int GetByteCount()
        {
            throw new NotImplementedException();
        }
    }

    public class SerialFrameAttribute : Attribute
    {
    }

    public class FrameStartFlagAttribute : SerialFrameDataAttribute
    {
        public FrameStartFlagAttribute(byte[] startFlag)
            : base(int.MinValue, startFlag)
        {

        }
    }
}
