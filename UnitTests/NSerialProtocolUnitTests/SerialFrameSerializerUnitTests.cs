using NSerialProtocol;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace NSerialProtocolUnitTests
{
    using NSerialProtocolUnitTests.Extensions;

    class EmptyTestSerialFrame : SerialFrame
    {

    }

    class BoolTestSerialFrame : SerialFrame
    {
        [SerialFrameMember(1)]
        public bool Value { get; set; } = default(bool);

        public BoolTestSerialFrame()
        {

        }

        public BoolTestSerialFrame(bool value)
        {
            Value = value;
        }
    }

    class CharTestSerialFrame : SerialFrame
    {
        [SerialFrameMember(1)]
        public char Value { get; set; } = default(char);

        public CharTestSerialFrame()
        {

        }

        public CharTestSerialFrame(char value)
        {
            Value = value;
        }
    }

    class SByteTestSerialFrame : SerialFrame
    {
        [SerialFrameMember(1)]
        public sbyte Value { get; set; } = default(sbyte);

        public SByteTestSerialFrame()
        {

        }

        public SByteTestSerialFrame(sbyte value)
        {
            Value = value;
        }
    }

    class Int16TestSerialFrame : SerialFrame
    {
        [SerialFrameMember(1)]
        public short Value { get; set; } = default(short);

        public Int16TestSerialFrame()
        {

        }

        public Int16TestSerialFrame(short value)
        {
            Value = value;
        }
    }

    class Int32TestSerialFrame : SerialFrame
    {
        [SerialFrameMember(1)]
        public int Value { get; set; } = default(int);

        public Int32TestSerialFrame()
        {

        }

        public Int32TestSerialFrame(int value)
        {
            Value = value;
        }
    }

    class Int64TestSerialFrame : SerialFrame
    {
        [SerialFrameMember(1)]
        public long Value { get; set; } = default(long);

        public Int64TestSerialFrame()
        {

        }

        public Int64TestSerialFrame(long value)
        {
            Value = value;
        }
    }

    class ByteTestSerialFrame : SerialFrame
    {
        [SerialFrameMember(1)]
        public byte Value { get; set; } = default(byte);

        public ByteTestSerialFrame()
        {

        }

        public ByteTestSerialFrame(byte value)
        {
            Value = value;
        }
    }

    class UInt16TestSerialFrame : SerialFrame
    {
        [SerialFrameMember(1)]
        public ushort Value { get; set; } = default(ushort);

        public UInt16TestSerialFrame()
        {

        }

        public UInt16TestSerialFrame(ushort value)
        {
            Value = value;
        }
    }

    class UInt32TestSerialFrame : SerialFrame
    {
        [SerialFrameMember(1)]
        public uint Value { get; set; } = default(uint);

        public UInt32TestSerialFrame()
        {

        }

        public UInt32TestSerialFrame(uint value)
        {
            Value = value;
        }
    }

    class UInt64TestSerialFrame : SerialFrame
    {
        [SerialFrameMember(1)]
        public ulong Value { get; set; } = default(ulong);

        public UInt64TestSerialFrame()
        {

        }

        public UInt64TestSerialFrame(ulong value)
        {
            Value = value;
        }
    }

    class ByteArrayTestSerialFrame : SerialFrame
    {
        [SerialFrameMember(1)]
        public byte[] Value { get; set; } = new byte[3];

        public ByteArrayTestSerialFrame()
        {

        }

        public ByteArrayTestSerialFrame(int length)
        {
            Value = new byte[length];
        }

        public ByteArrayTestSerialFrame(byte[] value)
        {
            Value = value;
        }
    }

    class CharArrayTestSerialFrame : SerialFrame
    {
        [SerialFrameMember(1)]
        public char[] Value { get; set; } = new char[3];

        public CharArrayTestSerialFrame()
        {

        }

        public CharArrayTestSerialFrame(int length)
        {
            Value = new char[length];
        }

        public CharArrayTestSerialFrame(char[] value)
        {
            Value = value;
        }
    }

    class StringTestSerialFrame : SerialFrame
    {
        [SerialFrameMember(1)]
        public string Value { get; set; } = default(string);

        public StringTestSerialFrame()
        {

        }

        public StringTestSerialFrame(string value)
        {
            Value = value;
        }
    }

    public class ManySimpleTypesTestFrame : SerialFrame
    {
        [SerialFrameMember(1)]
        public byte Value1 { get; set; } = 1;

        [SerialFrameMember(2)]
        public int Value2 { get; set; } = 2;

        [SerialFrameMember(3)]
        public string Value3 { get; set; } = "abc";
    }

    public class FieldOrderTestFrame : SerialFrame
    {
        [SerialFrameMember(3)]
        public byte Value1 { get; set; } = 1;

        [SerialFrameMember(2)]
        public int Value2 { get; set; } = 2;

        [SerialFrameMember(1)]
        public string Value3 { get; set; } = "abc";
    }

    [TestFixture]
    public class SerialFrameSerializerUnitTests
    {
        public static readonly IEnumerable<Tuple<SerialFrame, byte[]>> SimpleTypesSerializeTestCases
            = new List<Tuple<SerialFrame, byte[]>>
        {
            new Tuple<SerialFrame, byte[]>(new EmptyTestSerialFrame(), new byte[0]),

            new Tuple<SerialFrame, byte[]>(new BoolTestSerialFrame(false), new byte[1]{ 0 }),

            new Tuple<SerialFrame, byte[]>(new BoolTestSerialFrame(true), new byte[1]{ 1 }),

            new Tuple<SerialFrame, byte[]>(new CharTestSerialFrame('a'), new byte[1]{ 97 }),

            new Tuple<SerialFrame, byte[]>(new SByteTestSerialFrame(123), new byte[1]{ 123 }),

            new Tuple<SerialFrame, byte[]>(new Int16TestSerialFrame(123), new byte[2]{ 123, 0 }),

            new Tuple<SerialFrame, byte[]>(new Int32TestSerialFrame(123),
                new byte[4]{ 123, 0, 0, 0 }),

            new Tuple<SerialFrame, byte[]>(new Int64TestSerialFrame(123),
                new byte[8]{ 123, 0, 0, 0, 0, 0, 0, 0 }),

            new Tuple<SerialFrame, byte[]>(new ByteTestSerialFrame(123), new byte[1]{ 123 }),

            new Tuple<SerialFrame, byte[]>(new UInt16TestSerialFrame(123),
                new byte[2]{ 123, 0 }),

            new Tuple<SerialFrame, byte[]>(new UInt32TestSerialFrame(123),
                new byte[4]{ 123, 0, 0, 0 }),

            new Tuple<SerialFrame, byte[]>(new UInt64TestSerialFrame(123),
                new byte[8]{ 123, 0, 0, 0, 0, 0, 0, 0 }),

            new Tuple<SerialFrame, byte[]>(new ByteArrayTestSerialFrame(new byte[3] { 1, 2, 3 }),
                new byte[3]{ 1, 2, 3 }),

            new Tuple<SerialFrame, byte[]>(new CharArrayTestSerialFrame(new char[3] { 'a', 'b', 'c' }),
                new byte[3]{ 97, 98, 99 }),

            new Tuple<SerialFrame, byte[]>(new StringTestSerialFrame("abc"),
                new byte[4]{ 3, 97, 98, 99 }),

            new Tuple<SerialFrame, byte[]>(new ManySimpleTypesTestFrame(),
                new byte[9]{ 1, 2, 0, 0, 0, 3, 97, 98, 99 }),

            new Tuple<SerialFrame, byte[]>(new FieldOrderTestFrame(),
                new byte[9]{ 3, 97, 98, 99, 2, 0, 0, 0, 1 }),
        };

        private static IEnumerable<TestCaseData> GetSerialFrameTestCaseData()
        {
            foreach (Tuple<SerialFrame, byte[]> data in SimpleTypesSerializeTestCases)
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





        public static readonly IEnumerable<Tuple<Type, byte[], SerialFrame>> SimpleTypesDeserializeTestFrames
            = new List<Tuple<Type, byte[], SerialFrame>>
        {
            new Tuple<Type, byte[], SerialFrame>(typeof(EmptyTestSerialFrame),
                new byte[0], new EmptyTestSerialFrame()),

            new Tuple<Type, byte[], SerialFrame>(typeof(BoolTestSerialFrame),
                new byte[1]{ 0 }, new BoolTestSerialFrame(false)),

            new Tuple<Type, byte[], SerialFrame>(typeof(BoolTestSerialFrame),
                new byte[1]{ 1 }, new BoolTestSerialFrame(true)),

            new Tuple<Type, byte[], SerialFrame>(typeof(CharTestSerialFrame),
                new byte[1]{ 120 }, new CharTestSerialFrame('x')),

            new Tuple<Type, byte[], SerialFrame>(typeof(SByteTestSerialFrame),
                new byte[1]{ 120 }, new SByteTestSerialFrame(120)),

            new Tuple<Type, byte[], SerialFrame>(typeof(Int16TestSerialFrame),
                new byte[2]{ 120, 0 }, new Int16TestSerialFrame(120)),

            new Tuple<Type, byte[], SerialFrame>(typeof(Int32TestSerialFrame),
                new byte[4]{ 120, 0, 0, 0 }, new Int32TestSerialFrame(120)),

            new Tuple<Type, byte[], SerialFrame>(typeof(Int64TestSerialFrame),
                new byte[8]{ 120, 0, 0, 0, 0, 0, 0, 0 }, new Int64TestSerialFrame(120)),

            new Tuple<Type, byte[], SerialFrame>(typeof(ByteTestSerialFrame),
                new byte[1]{ 120 }, new ByteTestSerialFrame(120)),

            new Tuple<Type, byte[], SerialFrame>(typeof(UInt16TestSerialFrame),
                new byte[2]{ 120, 0 }, new UInt16TestSerialFrame(120)),

            new Tuple<Type, byte[], SerialFrame>(typeof(UInt32TestSerialFrame),
                new byte[4]{ 120, 0, 0, 0 }, new UInt32TestSerialFrame(120)),

            new Tuple<Type, byte[], SerialFrame>(typeof(UInt64TestSerialFrame),
                new byte[8]{ 120, 0, 0, 0, 0, 0, 0, 0 }, new UInt64TestSerialFrame(120)),

            new Tuple<Type, byte[], SerialFrame>(typeof(ByteArrayTestSerialFrame),
                new byte[3]{ 1, 2, 3 }, new ByteArrayTestSerialFrame(new byte[3]{ 1, 2, 3 })),

            new Tuple<Type, byte[], SerialFrame>(typeof(CharArrayTestSerialFrame),
                new byte[3]{ 120, 121, 122 }, new CharArrayTestSerialFrame(new char[3]{ 'x', 'y', 'z' })),

            new Tuple<Type, byte[], SerialFrame>(typeof(ManySimpleTypesTestFrame),
                new byte[9]{ 1, 2, 0, 0, 0, 3, 97, 98, 99 },
                new ManySimpleTypesTestFrame()),

            new Tuple<Type, byte[], SerialFrame>(typeof(FieldOrderTestFrame),
                new byte[9]{ 3, 97, 98, 99, 2, 0, 0, 0, 1 },
                new FieldOrderTestFrame()),
        };

        private static IEnumerable<TestCaseData> GetSerialFrameDeserializedTestCaseData()
        {
            foreach (Tuple<Type, byte[], SerialFrame> data in SimpleTypesDeserializeTestFrames)
            {
                yield return new TestCaseData(data.Item1, data.Item2, data.Item3);
            }
        }

        [Test]
        [TestCaseSource(nameof(GetSerialFrameDeserializedTestCaseData))]
        public void Deserialize_SimpleTypes_Test(Type type, byte[] serializedFrame, SerialFrame expected)
        {
            SerialFrameSerializer serializer = new SerialFrameSerializer();

            SerialFrame actual = (SerialFrame)Activator.CreateInstance(type);

            actual = serializer.Deserialize(type, serializedFrame);

            AssertExtensions.PropertyValuesAreEquals(actual, expected);

        }

    }
}
