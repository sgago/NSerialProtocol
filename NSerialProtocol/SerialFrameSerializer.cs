using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NSerialProtocol
{
    public class SerialFrameSerializer
    {
        public byte[] Serialize(SerialFrame frame)
        {
            byte[] serializedFrame = null;
            BinaryWriter binaryWriter;

            //List<PropertyInfo> properties = GetPropertiesByAttributeType<SerialFrameMemeberAttribute>(frame);

            //List<SerialFrameMemberAttribute> attributes = typeof(SerialFrame).GetType()
            //    .GetCustomAttributes<SerialFrameMemberAttribute>(true).ToList();

            List<PropertyInfo> properties = frame.GetType()
                .GetProperties()
                .Where(x => x.IsDefined(typeof(SerialFrameMemberAttribute)))
                .ToList();

            //SortedDictionary<SerialFrameMemberAttribute, PropertyInfo> dictionary =
            //    attributes.Zip(properties, (k, v) => new { k, v })
            //        .ToDictionary(x => x.k, x => x.v);

            //SortedDictionary<SerialFrameMemberAttribute, PropertyInfo> dict =
            //    new SortedDictionary<SerialFrameMemberAttribute, PropertyInfo>();

            //foreach (PropertyInfo property in properties)
            //{
            //    dict.Add((SerialFrameMemberAttribute)Attribute.GetCustomAttribute(property, typeof(SerialFrameMemberAttribute)),
            //        property);
            //}

            //dict.OrderBy(x => x.Key.Tag);

            List<KeyValuePair<SerialFrameMemberAttribute, PropertyInfo>> pairs
                = new List<KeyValuePair<SerialFrameMemberAttribute, PropertyInfo>>();

            foreach (PropertyInfo property in properties)
            {
                pairs.Add(new KeyValuePair<SerialFrameMemberAttribute, PropertyInfo>
                    ((SerialFrameMemberAttribute)Attribute.GetCustomAttribute(property, typeof(SerialFrameMemberAttribute)),
                    property));
            }

            pairs = pairs.OrderBy(x => x.Key.Tag).ToList();

            

            using (MemoryStream stream = new MemoryStream())
            {
                binaryWriter = new BinaryWriter(stream);

                foreach (PropertyInfo property in properties)
                {
                    object value = property.GetValue(frame);

                    // TODO: Use dynamic or long if block?
                    // I hate the use of the dynamic keyword
                    // Trying to decide if I should a 5 million line long if-block
                    // or just use stupid dynamic.  Here's stupid dynamic:
                    //dynamic value = Convert.ChangeType(property.GetValue(frame), property.PropertyType);
                    //binaryWriter.Write(value);

                    // Here's the 5 million lines of if...
                    //object value = property.GetValue(frame);

                    //if (property.PropertyType == typeof(byte))
                    //{
                    //    binaryWriter.Write((byte)value);
                    //}
                    //else if (property.PropertyType == typeof(int))
                    //{
                    //    binaryWriter.Write((int)value);
                    //}
                    // [Insert 10 billion types here]
                    //else if (property.PropertyType == typeof(byte[]))
                    //{
                    //    binaryWriter.Write((byte[])value);
                    //}

                    // So which to pick?
                    // I'll worry about optimizations later and use the if...

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
                        binaryWriter.Write(Encoding.Default.GetBytes((string)value));
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

        public IList<TAttribute> GetAttributesByType<TAttribute>() where TAttribute : Attribute
        {
            return GetType().GetCustomAttributes<TAttribute>(true).ToList();
        }

        public List<PropertyInfo> GetPropertiesByAttributeType<TAttribute>(SerialFrame serialFrame) where TAttribute : Attribute
        {
            return serialFrame.GetType().GetProperties()
                .Where(x => x.IsDefined(typeof(TAttribute)))
                .ToList();
        }

    }
}
