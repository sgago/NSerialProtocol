using FastMember;
using NSerialProtocol.Attributes;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NSerialProtocol
{
    public interface ISerialFrameSerializer
    {
        object Deserialize(Type type, byte[] serializedFrame);
        object Deserialize(Type type, string serializedFrame);
        object Deserialize(Type type, string serializedFrame, Encoding encoding);
        T Deserialize<T>(byte[] serializedFrame) where T : class, ISerialFrame;
        T Deserialize<T>(string serializedFrame) where T : class, ISerialFrame;
        T Deserialize<T>(string serializedFrame, Encoding encoding) where T : class, ISerialFrame;
        void Prepare(Type type);
        void Prepare<T>() where T : ISerialFrame;
        byte[] Serialize(ISerialFrame frame);
    }

    // TODO: This should work on properties and fields
    // TODO: This should have various string encodings
    // TODO: This should allow various length prefixes for strings
    // TODO: This should allow various string postfixes for strings (C strings)
    // TODO: This should have a non-prepared serialize and deserialize methods
    // TODO: Maybe this should have an empty constructor
    // TODO: This monster could probably use various events during the serialization process
    // TODO: Async methods for serialization and deserialization?
    // TODO: Need to extract an interface and use dependency inversion principle.
    /// <summary>
    /// Represents methods of serializing and deserializing SerialFramae instances.
    /// </summary>
    public class SerialFrameSerializer : ISerialFrameSerializer
    {
        private TypeAccessor TypeAccessor { get; set; }

        private List<Member> Members { get; set; }

        public SerialFrameSerializer()
        {
            
        }

        public void Prepare(Type type)
        {
            TypeAccessor = TypeAccessor.Create(type);

            Members = TypeAccessor
                .GetMembers()
                .Where(x => x.IsDefined(typeof(FrameMemberAttribute)))
                .OrderBy(x => (x.GetAttribute(typeof(FrameMemberAttribute), true)
                   as FrameMemberAttribute).Tag)
                .ToList();
        }

        public void Prepare<T>() where T : ISerialFrame
        {
            Prepare(typeof(T));
        }

        /// <summary>
        /// Serializes a SerialFrame instance into a byte array.
        /// </summary>
        /// <param name="frame">The SerialFrame to serialize.</param>
        /// <returns>The serialized frame as a byte array.</returns>
        public byte[] Serialize(ISerialFrame frame)
        {
            byte[] serializedFrame = null;
            BinaryWriter binaryWriter;

            if (TypeAccessor == null || Members == null)
            {
                Prepare(frame.GetType());
            }

            ObjectAccessor serialFrameAccessor = ObjectAccessor.Create(frame);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                binaryWriter = new BinaryWriter(memoryStream);

                foreach (Member member in Members)
                {
                    object value = TypeAccessor[frame, member.Name];
                    TypeCode typeCode = Type.GetTypeCode(member.Type);
                    
                    if (member.Type == typeof(byte))
                    {
                        binaryWriter.Write((byte)value);
                    }
                    else if (member.Type == typeof(sbyte))
                    {
                        binaryWriter.Write((sbyte)value);
                    }
                    else if (member.Type == typeof(char))
                    {
                        binaryWriter.Write((char)value);
                    }
                    else if (member.Type == typeof(bool))
                    {
                        binaryWriter.Write((bool)value);
                    }
                    else if (member.Type == typeof(short))
                    {
                        binaryWriter.Write((short)value);
                    }
                    else if (member.Type == typeof(int))
                    {
                        binaryWriter.Write((int)value);
                    }
                    else if (member.Type == typeof(long))
                    {
                        binaryWriter.Write((long)value);
                    }
                    else if (member.Type == typeof(ushort))
                    {
                        binaryWriter.Write((ushort)value);
                    }
                    else if (member.Type == typeof(uint))
                    {
                        binaryWriter.Write((uint)value);
                    }
                    else if (member.Type == typeof(ulong))
                    {
                        binaryWriter.Write((ulong)value);
                    }
                    else if (member.Type == typeof(float))
                    {
                        binaryWriter.Write((float)value);
                    }
                    else if (member.Type == typeof(double))
                    {
                        binaryWriter.Write((double)value);
                    }
                    else if (member.Type == typeof(decimal))
                    {
                        binaryWriter.Write((decimal)value);
                    }
                    else if (member.Type == typeof(string))
                    {
                        // TODO: Support for different string encodings?
                        // TODO: This saves the length of the string then the string value
                        // For instance, "abc" becomes 3, 97, 98, 99
                        // Should we add a length prefix, null-terminated, and
                        // vanilla string values?

                        // Writes a string without length prefix:
                        //binaryWriter.Write(Encoding.Default.GetBytes((string)value));
                        binaryWriter.Write((string)value);
                    }
                    else if (member.Type == typeof(byte[]))
                    {
                        binaryWriter.Write((byte[])value);
                    }
                    else if (member.Type == typeof(char[]))
                    {
                        binaryWriter.Write((char[])value);
                    }

                    // TODO: Needs unit tests
                    else if (member.Type.BaseType == typeof(SerialPacket))
                    {
                        SerialPacketMemberAttribute attribute =
                            member.GetAttribute(typeof(SerialPacketMemberAttribute), true)
                            as SerialPacketMemberAttribute;

                        binaryWriter.Write(SerializePacket(value as ISerialPacket));
                    }
                }

                serializedFrame = memoryStream.ToArray();
            }

            return serializedFrame;
        }

        private byte[] SerializePacket(ISerialPacket serialPacket)
        {
            byte[] data;

            using (MemoryStream ms = new MemoryStream())
            {
                Serializer.Serialize(ms, serialPacket);

                data = ms.ToArray();
            }

            byte[] lengthPrefix = BitConverter.GetBytes(data.Length);

            byte[] serializedPacket = new byte[lengthPrefix.Length + data.Length];

            Array.Copy(lengthPrefix, serializedPacket, lengthPrefix.Length);

            Array.Copy(data, 0, serializedPacket, lengthPrefix.Length, data.Length);

            return serializedPacket;
        }

        public object Deserialize(Type type, byte[] serializedFrame)
        {
            BinaryReader binaryReader;

            if (TypeAccessor == null || Members == null)
            {
                Prepare(type);
            }

            object newSerialFrame = TypeAccessor.CreateNew();
            ObjectAccessor objectAccessor = ObjectAccessor.Create(newSerialFrame);

            using (MemoryStream stream = new MemoryStream(serializedFrame))
            {
                binaryReader = new BinaryReader(stream);

                foreach (Member member in Members)
                {
                    if (member.Type == typeof(byte))
                    {
                        objectAccessor[member.Name] = binaryReader.ReadByte();
                    }
                    else if (member.Type == typeof(sbyte))
                    {
                        objectAccessor[member.Name] = binaryReader.ReadSByte();
                    }
                    else if (member.Type == typeof(char))
                    {
                        objectAccessor[member.Name] = binaryReader.ReadChar();
                    }
                    else if (member.Type == typeof(bool))
                    {
                        objectAccessor[member.Name] = binaryReader.ReadBoolean();
                    }
                    else if (member.Type == typeof(short))
                    {
                        objectAccessor[member.Name] = binaryReader.ReadInt16();
                    }
                    else if (member.Type == typeof(int))
                    {
                        objectAccessor[member.Name] = binaryReader.ReadInt32();
                    }
                    else if (member.Type == typeof(long))
                    {
                        objectAccessor[member.Name] = binaryReader.ReadInt64();
                    }
                    else if (member.Type == typeof(ushort))
                    {
                        objectAccessor[member.Name] = binaryReader.ReadUInt16();
                    }
                    else if (member.Type == typeof(uint))
                    {
                        objectAccessor[member.Name] = binaryReader.ReadUInt32();
                    }
                    else if (member.Type == typeof(ulong))
                    {
                        objectAccessor[member.Name] = binaryReader.ReadUInt64();
                    }
                    else if (member.Type == typeof(float))
                    {
                        objectAccessor[member.Name] = binaryReader.ReadSingle();
                    }
                    else if (member.Type == typeof(double))
                    {
                        objectAccessor[member.Name] = binaryReader.ReadDouble();
                    }
                    else if (member.Type == typeof(decimal))
                    {
                        objectAccessor[member.Name] = binaryReader.ReadDecimal();
                    }
                    else if (member.Type == typeof(string))
                    {
                        // TODO: Support for different string encodings
                        // TODO: Support for different string prefix and postfix serialization methods
                        objectAccessor[member.Name] = binaryReader.ReadString();
                    }
                    else if (member.Type == typeof(byte[]))
                    {
                        objectAccessor[member.Name] = binaryReader.ReadBytes(
                                (objectAccessor[member.Name] as Array).Length);
                    }
                    else if (member.Type == typeof(char[]))
                    {
                        objectAccessor[member.Name] = binaryReader.ReadChars(
                                (objectAccessor[member.Name] as Array).Length);
                    }

                    else if (member.Type.BaseType == typeof(SerialPacket))
                    {
                        int length = -1;

                        Serializer.TryReadLengthPrefix(binaryReader.BaseStream, PrefixStyle.Fixed32, out length);

                        // TODO: Need to allow the programmer to specify how the length prefix will look
                        byte[] serialPacketBytes = new byte[length];

                        Array.Copy(serializedFrame, (int)binaryReader.BaseStream.Position,
                                   serialPacketBytes, 0,
                                   serialPacketBytes.Length);

                        objectAccessor[member.Name] = DeserializePacket(member.Type, serialPacketBytes);

                        // Advance the position of the binary reader's base stream
                        // Because there's no call to binaryReader methods, we need to do this ourselves
                        binaryReader.BaseStream.Position += length;
                    }
                }
            }

            return newSerialFrame;
        }

        private object DeserializePacket(Type serialPacketType, byte[] bytes)
        {
            object serialPacket;

            byte[] serialPacketBytes = new byte[bytes.Length - 4];

            Array.Copy(bytes, 4, serialPacketBytes, 0, serialPacketBytes.Length);

            using (MemoryStream memoryStream = new MemoryStream(bytes))
            {
                serialPacket = Serializer.Deserialize(serialPacketType, memoryStream);
            }

            return serialPacket;
        }

        public object Deserialize(Type type, string serializedFrame, Encoding encoding)
        {
            return Deserialize(type, encoding.GetBytes(serializedFrame));
        }

        public object Deserialize(Type type, string serializedFrame)
        {
            return Deserialize(type, serializedFrame, Encoding.Default);
        }

        public T Deserialize<T>(byte[] serializedFrame) where T : class, ISerialFrame
        {
            return Deserialize(typeof(T), serializedFrame) as T;
        }

        public T Deserialize<T>(string serializedFrame, Encoding encoding) where T : class, ISerialFrame
        {
            return Deserialize(typeof(T), serializedFrame, encoding) as T;
        }

        public T Deserialize<T>(string serializedFrame) where T : class, ISerialFrame
        {
            return Deserialize(typeof(T), serializedFrame, Encoding.Default) as T;
        }
    }
}
