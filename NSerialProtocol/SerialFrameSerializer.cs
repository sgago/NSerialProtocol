using NSerialProtocol.Attributes;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace NSerialProtocol
{
    using SerialFrameMemberKvp = KeyValuePair<FrameMemberAttribute, PropertyInfo>;

    // TODO: Need to create an ISerialize interface and this class needs to use it
    // when serializing and deserializing data
    // TODO: This should work on properties and fields
    // TODO: This should have various string encodings
    // TODO: This should allow various length prefixes for strings
    // TODO: This should allow various string postfixes for strings (C strings)
    // TODO: This should have a non-prepared serialize and deserialize methods
    // TODO: Maybe this should have an empty constructor
    // TODO: This monster could probably use various events during the serialization process
    // TODO: Async methods for serialization and deserialization?
    // TODO: Convert this entire monster from reflection to fastmember type
    /// <summary>
    /// Represents methods of serializing and deserializing SerialFramae instances.
    /// </summary>
    public class SerialFrameSerializer
    {
        private List<SerialFrameMemberKvp> SerialFrameMembers { get; set; }
            = new List<SerialFrameMemberKvp>();

        public SerialFrameSerializer(SerialFrame serialFrame)
        {
            // Get the properties on the SerialFrame instance
            List<PropertyInfo> properties = serialFrame.GetType()
                .GetProperties()
                .Where(x => x.IsDefined(typeof(FrameMemberAttribute)))
                .ToList();

            // For each property in frame...
            foreach (PropertyInfo property in properties)
            {
                // FIXME: This line is painful to read
                // Grab the SerialFrameAttribute connected to the property
                SerialFrameMembers.Add(new SerialFrameMemberKvp
                    ((FrameMemberAttribute)Attribute.GetCustomAttribute(property,
                        typeof(FrameMemberAttribute)), property));
            }

            // Sort the KVPs by tag number
            SerialFrameMembers = SerialFrameMembers.OrderBy(x => x.Key.Tag).ToList();
        }

        /// <summary>
        /// Serializes a SerialFrame instance into a byte array.
        /// </summary>
        /// <param name="frame">The SerialFrame to serialize.</param>
        /// <returns>The serialized frame as a byte array.</returns>
        public byte[] Serialize(SerialFrame frame)
        {
            byte[] serializedFrame = null;
            BinaryWriter binaryWriter;

            using (MemoryStream stream = new MemoryStream())
            {
                binaryWriter = new BinaryWriter(stream);

                foreach (SerialFrameMemberKvp member in SerialFrameMembers)
                {
                    PropertyInfo property = member.Value;

                    object value = property.GetValue(frame);

                    // TODO: Use dynamic or long if block?
                    // I hate the use of the dynamic keyword.
                    // Trying to decide if I should a 5 million line long if-block
                    // or just use stupid dynamic.  Here's stupid dynamic:
                    //dynamic value = Convert.ChangeType(property.GetValue(frame), property.PropertyType);
                    //binaryWriter.Write(value);

                    if (property.PropertyType == typeof(byte))
                    {
                        binaryWriter.Write((byte)value);
                    }
                    else if (property.PropertyType == typeof(sbyte))
                    {
                        binaryWriter.Write((sbyte)value);
                    }
                    else if (property.PropertyType == typeof(char))
                    {
                        binaryWriter.Write((char)value);
                    }
                    else if (property.PropertyType == typeof(bool))
                    {
                        binaryWriter.Write((bool)value);
                    }
                    else if (property.PropertyType == typeof(short))
                    {
                        binaryWriter.Write((short)value);
                    }
                    else if (property.PropertyType == typeof(int))
                    {
                        binaryWriter.Write((int)value);
                    }
                    else if (property.PropertyType == typeof(long))
                    {
                        binaryWriter.Write((long)value);
                    }
                    else if (property.PropertyType == typeof(ushort))
                    {
                        binaryWriter.Write((ushort)value);
                    }
                    else if (property.PropertyType == typeof(uint))
                    {
                        binaryWriter.Write((uint)value);
                    }
                    else if (property.PropertyType == typeof(ulong))
                    {
                        binaryWriter.Write((ulong)value);
                    }
                    else if (property.PropertyType == typeof(float))
                    {
                        binaryWriter.Write((float)value);
                    }
                    else if (property.PropertyType == typeof(double))
                    {
                        binaryWriter.Write((double)value);
                    }
                    else if (property.PropertyType == typeof(decimal))
                    {
                        binaryWriter.Write((decimal)value);
                    }
                    else if (property.PropertyType == typeof(string))
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
                    else if (property.PropertyType == typeof(byte[]))
                    {
                        binaryWriter.Write((byte[])value);
                    }
                    else if (property.PropertyType == typeof(char[]))
                    {
                        binaryWriter.Write((char[])value);
                    }

                    // TODO: Needs unit tests
                    else if (property.PropertyType.BaseType == typeof(SerialPacket))
                    {
                        //binaryWriter.Write((value as ISerialize).Serialize());
                        byte[] data;

                        using (var ms = new MemoryStream())
                        {
                            Serializer.Serialize(ms, value);

                            data = ms.ToArray();
                        }

                        byte[] lengthPrefix = BitConverter.GetBytes(data.Length);
                        byte[] packetBytes = new byte[lengthPrefix.Length + data.Length];
                        Array.Copy(lengthPrefix, packetBytes, lengthPrefix.Length);
                        Array.Copy(data, 0, packetBytes, lengthPrefix.Length, data.Length);

                        binaryWriter.Write(packetBytes);
                    }
                }

                serializedFrame = stream.ToArray();
            }

            return serializedFrame;
        }

        // TODO: Need the ability to deserialize frames that are different than the one that was transmitted!!!!
        /// <summary>
        /// 
        /// </summary>
        /// <param name="frameType"></param>
        /// <param name="serializedFrame"></param>
        /// <returns></returns>
        public SerialFrame Deserialize(Type frameType, byte[] serializedFrame)
        {
            BinaryReader binaryReader;
            SerialFrame newSerialFrame = Activator.CreateInstance(frameType) as SerialFrame;

            using (MemoryStream stream = new MemoryStream(serializedFrame))
            {
                binaryReader = new BinaryReader(stream);

                foreach (SerialFrameMemberKvp member in SerialFrameMembers)
                {
                    PropertyInfo property = member.Value;

                    if (property.PropertyType == typeof(byte))
                    {
                        property.SetValue(newSerialFrame, binaryReader.ReadByte());
                    }
                    else if (property.PropertyType == typeof(sbyte))
                    {
                        property.SetValue(newSerialFrame, binaryReader.ReadSByte());
                    }
                    else if (property.PropertyType == typeof(char))
                    {
                        property.SetValue(newSerialFrame, binaryReader.ReadChar());
                    }
                    else if (property.PropertyType == typeof(bool))
                    {
                        property.SetValue(newSerialFrame, binaryReader.ReadBoolean());
                    }
                    else if (property.PropertyType == typeof(short))
                    {
                        property.SetValue(newSerialFrame, binaryReader.ReadInt16());
                    }
                    else if (property.PropertyType == typeof(int))
                    {
                        property.SetValue(newSerialFrame, binaryReader.ReadInt32());
                    }
                    else if (property.PropertyType == typeof(long))
                    {
                        property.SetValue(newSerialFrame, binaryReader.ReadInt64());
                    }
                    else if (property.PropertyType == typeof(ushort))
                    {
                        property.SetValue(newSerialFrame, binaryReader.ReadUInt16());
                    }
                    else if (property.PropertyType == typeof(uint))
                    {
                        property.SetValue(newSerialFrame, binaryReader.ReadUInt32());
                    }
                    else if (property.PropertyType == typeof(ulong))
                    {
                        property.SetValue(newSerialFrame, binaryReader.ReadUInt64());
                    }
                    else if (property.PropertyType == typeof(float))
                    {
                        property.SetValue(newSerialFrame, binaryReader.ReadSingle());
                    }
                    else if (property.PropertyType == typeof(double))
                    {
                        property.SetValue(newSerialFrame, binaryReader.ReadDouble());
                    }
                    else if (property.PropertyType == typeof(decimal))
                    {
                        property.SetValue(newSerialFrame, binaryReader.ReadDecimal());
                    }
                    else if (property.PropertyType == typeof(string))
                    {
                        // TODO: Support for different string encodings?
                        property.SetValue(newSerialFrame, binaryReader.ReadString());
                    }
                    else if (property.PropertyType == typeof(byte[]))
                    {
                        property.SetValue(newSerialFrame, binaryReader.ReadBytes(
                                (property.GetValue(newSerialFrame) as Array).Length
                        ));
                    }
                    else if (property.PropertyType == typeof(char[]))
                    {
                        property.SetValue(newSerialFrame, binaryReader.ReadChars(
                                (property.GetValue(newSerialFrame) as Array).Length
                        ));
                    }

                    // TODO: Needs unit tests
                    else if (property.PropertyType.BaseType == typeof(SerialPacket))
                    {
                        // FIXME: All this code needs to be cleaned up completely
                        // This looks like a disaster
                        int length = -1;
                        
                        Serializer.TryReadLengthPrefix(binaryReader.BaseStream, PrefixStyle.Fixed32, out length);

                        // TODO: Need to allow the programmer to specify how the length prefix will look
                        byte[] serialPacketBytes = new byte[length];

                        Array.Copy(serializedFrame, (int)binaryReader.BaseStream.Position, serialPacketBytes, 0, serialPacketBytes.Length);

                        MemoryStream serialPacketStream = new MemoryStream(serialPacketBytes);

                        object sp = Serializer.Deserialize(property.PropertyType, serialPacketStream);

                        property.SetValue(newSerialFrame, sp);
                    }
                }
            }

            return newSerialFrame;
        }

        // FIXME: This should probably go into a reflection extension class or similar
        public IList<TAttribute> GetAttributesByType<TAttribute>() where TAttribute : Attribute
        {
            return GetType().GetCustomAttributes<TAttribute>(true).ToList();
        }

        // FIXME: This should probably go into a reflection extension class or similar
        public List<PropertyInfo> GetPropertiesByAttributeType<TAttribute>(SerialFrame serialFrame) where TAttribute : Attribute
        {
            return serialFrame.GetType().GetProperties()
                .Where(x => x.IsDefined(typeof(TAttribute)))
                .ToList();
        }

    }
}
