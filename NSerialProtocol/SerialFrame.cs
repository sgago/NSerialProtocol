﻿using NByteStuff;
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
    public class SerialFrame
    {

    }

    public class SerialFrameMemberAttribute : Attribute
    {
        public int Tag { get; }

        public SerialFrameMemberAttribute(int tag)
        {
            Tag = tag;
        }
    }
}
