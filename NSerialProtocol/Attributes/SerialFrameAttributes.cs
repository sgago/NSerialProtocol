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
    public class FrameMemberAttribute : Attribute
    {
        public int Tag { get; }

        public FrameMemberAttribute(int tag)
        {
            Tag = tag;
        }
    }

    public class StartFlagAttribute : FrameMemberAttribute
    {
        public StartFlagAttribute()
            : base (int.MinValue)
        {
        }
    }

    public class EndFlagAttribute : FrameMemberAttribute
    {
        public EndFlagAttribute()
            : base (int.MaxValue)
        {

        }
    }
}
