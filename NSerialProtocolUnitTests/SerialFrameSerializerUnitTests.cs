using NSerialProtocol;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSerialProtocolUnitTests
{
    class EmptyTestSerialFrame : SerialFrame
    {

    }

    class BoolTestSerialFrame : SerialFrame
    {
        [SerialFrameMember(1)]
        public bool Value { get; set; } = true;

        public BoolTestSerialFrame(bool value)
        {
            Value = value;
        }
    }

    class CharTestSerialFrame : SerialFrame
    {
        [SerialFrameMember(1)]
        public char Value { get; set; } = 'c';

        public CharTestSerialFrame(char value)
        {
            Value = value;
        }
    }

    class SByteTestSerialFrame : SerialFrame
    {
        [SerialFrameMember(1)]
        public sbyte Value { get; set; } = 123;

        public SByteTestSerialFrame(sbyte value)
        {
            Value = value;
        }
    }

    class Int16TestSerialFrame : SerialFrame
    {
        [SerialFrameMember(1)]
        public short Value { get; set; } = 123;

        public Int16TestSerialFrame(short value)
        {
            Value = value;
        }
    }

    class Int32TestSerialFrame : SerialFrame
    {
        [SerialFrameMember(1)]
        public int Value { get; set; } = 123;

        public Int32TestSerialFrame(int value)
        {
            Value = value;
        }
    }

    class Int64TestSerialFrame : SerialFrame
    {
        [SerialFrameMember(1)]
        public long Value { get; set; } = 123;

        public Int64TestSerialFrame(long value)
        {
            Value = value;
        }
    }

    class ByteTestSerialFrame : SerialFrame
    {
        [SerialFrameMember(1)]
        public byte Value { get; set; } = 123;

        public ByteTestSerialFrame(byte value)
        {
            Value = value;
        }
    }

    class UInt16TestSerialFrame : SerialFrame
    {
        [SerialFrameMember(1)]
        public ushort Value { get; set; } = 123;

        public UInt16TestSerialFrame(ushort value)
        {
            Value = value;
        }
    }

    class UInt32TestSerialFrame : SerialFrame
    {
        [SerialFrameMember(1)]
        public uint Value { get; set; } = 123;

        public UInt32TestSerialFrame(uint value)
        {
            Value = value;
        }
    }

    class UInt64TestSerialFrame : SerialFrame
    {
        [SerialFrameMember(1)]
        public ulong Value { get; set; } = 123;

        public UInt64TestSerialFrame(ulong value)
        {
            Value = value;
        }
    }

    class ByteArrayTestSerialFrame : SerialFrame
    {
        [SerialFrameMember(1)]
        public byte[] Value { get; set; }

        public ByteArrayTestSerialFrame(byte[] value)
        {
            Value = value;
        }
    }

    class CharArrayTestSerialFrame : SerialFrame
    {
        [SerialFrameMember(1)]
        public char[] Value { get; set; } = new char[0];

        public CharArrayTestSerialFrame(char[] value)
        {
            Value = value;
        }
    }

    class StringTestSerialFrame : SerialFrame
    {
        [SerialFrameMember(1)]
        public string Value { get; set; } = "abc";

        public StringTestSerialFrame(string value)
        {
            Value = value;
        }
    }

    public class ManySimpleTypesTestFrame : SerialFrame
    {
        [SerialFrameMember(1)]
        public byte Value1 { get; } = 1;

        [SerialFrameMember(2)]
        public int Value2 { get; } = 2;

        [SerialFrameMember(3)]
        public string Value3 { get; } = "abc";
    }




    [TestFixture]
    public class SerialFrameSerializerUnitTests
    {
        public static readonly IEnumerable<Tuple<SerialFrame, byte[]>> simpleTypesTestFrames
            = new List<Tuple<SerialFrame, byte[]>>
        {
            new Tuple<SerialFrame, byte[]>(new EmptyTestSerialFrame(), new byte[0]),
            new Tuple<SerialFrame, byte[]>(new BoolTestSerialFrame(false), new byte[1]{ 0 }),
            new Tuple<SerialFrame, byte[]>(new BoolTestSerialFrame(true), new byte[1]{ 1 }),
            new Tuple<SerialFrame, byte[]>(new CharTestSerialFrame('a'), new byte[1]{ 97 }),
            new Tuple<SerialFrame, byte[]>(new SByteTestSerialFrame(123), new byte[1]{ 123 }),
            new Tuple<SerialFrame, byte[]>(new Int16TestSerialFrame(123), new byte[2]{ 123, 0 }),
            new Tuple<SerialFrame, byte[]>(new Int32TestSerialFrame(123), new byte[4]{ 123, 0, 0, 0 }),
            new Tuple<SerialFrame, byte[]>(new Int64TestSerialFrame(123), new byte[8]{ 123, 0, 0, 0, 0, 0, 0, 0 }),
            new Tuple<SerialFrame, byte[]>(new ByteTestSerialFrame(123), new byte[1]{ 123 }),
            new Tuple<SerialFrame, byte[]>(new UInt16TestSerialFrame(123), new byte[2]{ 123, 0 }),
            new Tuple<SerialFrame, byte[]>(new UInt32TestSerialFrame(123), new byte[4]{ 123, 0, 0, 0 }),
            new Tuple<SerialFrame, byte[]>(new UInt64TestSerialFrame(123), new byte[8]{ 123, 0, 0, 0, 0, 0, 0, 0 }),
            new Tuple<SerialFrame, byte[]>(new ByteArrayTestSerialFrame(new byte[3] { 1, 2, 3 }), new byte[3]{ 1, 2, 3 }),
            new Tuple<SerialFrame, byte[]>(new CharArrayTestSerialFrame(new char[3] { 'a', 'b', 'c' }), new byte[3]{ 97, 98, 99 }),
            new Tuple<SerialFrame, byte[]>(new StringTestSerialFrame("abc"), new byte[3]{ 97, 98, 99 }),
            new Tuple<SerialFrame, byte[]>(new ManySimpleTypesTestFrame(),
                new byte[8]{ 1, 2, 0, 0, 0, 97, 98, 99 }),
        };

        private static IEnumerable<TestCaseData> GetSerialFrameTestCaseData()
        {
            foreach (Tuple<SerialFrame, byte[]> data in simpleTypesTestFrames)
            {
                yield return new TestCaseData(data.Item1).Returns(data.Item2);
            }
        }

        [Test]
        [TestCaseSource(nameof(GetSerialFrameTestCaseData))]
        public byte[] Serialize_SimpleTypes_Test(SerialFrame frame)
        {
            SerialFrameSerializer serializer = new SerialFrameSerializer();

            return serializer.Serialize(frame);
        }

    }
}
