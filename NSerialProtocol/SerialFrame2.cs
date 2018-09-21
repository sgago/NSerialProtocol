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
    public class Field
    {
        public int Tag { get; set; }

        public byte[] Value { get; set; }

        public int Length { get; set; }

        public Field(int tag, int length)
        {
            Tag = tag;
            Length = length;
        }

        public Field(int tag, byte[] value, int length)
        {
            Tag = tag;
            Value = value;
            Length = length;
        }
    }

    public class SerialFrame2
    {
        private List<Field> Fields { get; set; } = new List<Field>();

        public SerialFrame2 AddField(int tag, byte[] value, int length)
        {
            Fields.Add(new Field(tag, value, length));

            return this;
        }



        public byte[] Serialize()
        {
            int index = 0;
            int frameLength = Fields.Sum(x => x.Value?.Length ?? 0);
            byte[] frame = new byte[frameLength];
            Fields.OrderBy(x => x.Tag);

            foreach (Field field in Fields.Where(x => x.Value != null))
            {
                Array.Copy(field.Value, 0, frame, index, field.Length);

                index += field.Value.Length;
            }

            return frame;
        }

        public void Deserialize(byte[] frame)
        {
            int index = 0;

            foreach (Field field in Fields.OrderBy(x => x.Tag))
            {
                Array.Copy(frame, index, field.Value, 0, field.Length);

                index += field.Length;
            }
        }
        

    }
}
