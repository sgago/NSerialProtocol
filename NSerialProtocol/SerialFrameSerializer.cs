using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace NSerialProtocol
{
    using SerialMemberKvp = KeyValuePair<SerialFrameMemberAttribute, PropertyInfo>;

    /// <summary>
    /// Represents methods of serializing and deserializing SerialFramae instances.
    /// </summary>
    public class SerialFrameSerializer
    {
        /// <summary>
        /// Serializes a SerialFrame instance into a byte array.
        /// </summary>
        /// <param name="frame">The SerialFrame to serialize.</param>
        /// <returns>The serialized frame as a byte array.</returns>
        public byte[] Serialize(SerialFrame frame)
        {
            byte[] serializedFrame = null;
            BinaryWriter binaryWriter;

            // Dictionary of attributes and properties for sorting, reading, etc.
            List<SerialMemberKvp> pairs = new List<SerialMemberKvp>();

            // Get the properties on the SerialFrame instance
            List<PropertyInfo> properties = frame.GetType()
                .GetProperties()
                .Where(x => x.IsDefined(typeof(SerialFrameMemberAttribute)))
                .ToList();

            // For each property in frame...
            foreach (PropertyInfo property in properties)
            {
                // FIXME: This line is painful to read
                // Grab the SerialFrameAttribute connected to the property
                pairs.Add(new SerialMemberKvp
                    ((SerialFrameMemberAttribute)Attribute.GetCustomAttribute(property,
                        typeof(SerialFrameMemberAttribute)), property));
            }

            // Sort the KVPs by tag number
            pairs = pairs.OrderBy(x => x.Key.Tag).ToList();

            using (MemoryStream stream = new MemoryStream())
            {
                binaryWriter = new BinaryWriter(stream);

                foreach (SerialMemberKvp pair in pairs)
                {
                    PropertyInfo property = pair.Value;

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
                }

                serializedFrame = stream.ToArray();
            }

            return serializedFrame;
        }
    
        public T Deserialize<T>(T frame)
        {
            throw new NotImplementedException();
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
