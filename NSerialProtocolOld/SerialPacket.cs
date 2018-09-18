namespace NSerialProtocol
{
    using NByteStuff;
    using NFec;
    using System.Collections.Generic;
    using System.Linq;
    using System;
    using System.Text;

    public enum DataType
    {
        Ascii,
        Binary
    }

    public class SerialPacket : ISerialPacket, IDeepCloneable<ISerialPacket>
    {
        /// <summary>
        /// Fields in the serial packet.
        /// </summary>
        private readonly IDictionary<string, IField> fields =
            new Dictionary<string, IField>();

        public IDynamicLength DynamicLengthConverter { get; private set; } = null;

        public IFec Fec { get; private set; } = null;
        public IByteStuff ByteStuff { get; private set; } = null;

        /// <summary>
        /// Gets or sets the fields within a serial packet.
        /// </summary>
        public IDictionary<string, IField> Fields
        {
            get
            {
                return fields;
            }
        }

        /// <summary>
        /// Gets the length of the SerialPacket.
        /// </summary>
        public int Length
        {
            get
            {
                return Fields.Sum(x => x.Value.Value.Length);
            }
        }

        /// <summary>
        /// Gets the maximum length of the SerialPacket.
        /// </summary>
        public int MaxLength { get; private set; }

        /// <summary>
        /// Gets the string value of the SerialPacket.
        /// </summary>
        public string Value
        {
            get
            {
                int index = 0;
                int length = -1;
                char[] packet = new char[Length];

                foreach (Field field in Fields.Values.OrderBy(x => x.Order))
                {
                    index = GetAbsoluteFieldIndex(field, packet.Length);
                    length = GetFieldLength(field, packet.Length);

                    field.Value.CopyTo(0, packet, index, length);
                }

                return string.Concat(packet);
            }
        }

        public string RawValue
        {
            get
            {
                return ByteStuff?.Stuff(Value) ?? Value;
            }
        }

        public IField Payload
        {
            get
            {
                Fields.TryGetValue(nameof(Payload), out IField result);

                return result;
            }
        }

        public IField StartFlag
        {
            get
            {
                Fields.TryGetValue(nameof(StartFlag), out IField result);

                return result;
            }
        }

        public IField EndFlag
        {
            get
            {
                Fields.TryGetValue(nameof(EndFlag), out IField result);

                return result;
            }
        }

        public IField DynamicLength
        {
            get
            {
                Fields.TryGetValue(nameof(DynamicLength), out IField result);

                return result;
            }
        }

        public IField Ecc
        {
            get
            {
                Fields.TryGetValue(nameof(Ecc), out IField result);

                return result;
            }
        }

        public SerialPacket()
        {

        }

        private SerialPacket(ISerialPacket serialPacket)
        {
            //DynamicLengthConverter = serialPacket.DynamicLengthConverter;

            Fec = serialPacket.Fec;
            ByteStuff = serialPacket.ByteStuff;

            foreach (KeyValuePair<string, IField> field in serialPacket.Fields)
            {
                // TODO: Not sure I like this cast
                AddField(field.Key, (field.Value as IDeepCloneable<Field>)?.DeepClone());
            }
        }

        /// <summary>
        /// Computes the absolute, non-negative index of a specified field.
        /// </summary>
        /// <param name="field">SerialPacketField to compute index of.</param>
        /// <returns>Absolute index of the specified field.</returns>
        protected int GetAbsoluteFieldIndex(IField field, int maxLength)
        {
            int index = 0;

            if (field.Index >= 0)
            {
                index = field.Index;
            }
            else
            {
                index = maxLength + field.Index - field.Length + 1;
            }

            return index;
        }

        /// <summary>
        /// Computes the length of a specified field.
        /// </summary>
        /// <param name="field">SerialPacketField to compute length of.</param>
        /// <returns>Length of the specified field.</returns>
        protected int GetFieldLength(IField field, int maxLength)
        {
            int index = 0;
            int length = -1;

            if (field.KnownLength)
            {
                length = field.Length;
            }
            else
            {
                index = GetAbsoluteFieldIndex(field, maxLength);

                // Do any *other* fields use negative indexing?
                if (Fields.Any(x => GetAbsoluteFieldIndex(x.Value, maxLength) >= index && !(x.Value.Equals(field))))
                {
                    // Ok, some other field uses negative indexing.
                    // Now, find the negative fields that
                    length = Fields
                        .Where(x => GetAbsoluteFieldIndex(x.Value, maxLength) >= field.Index && !(x.Value.Equals(field)))
                        .Select(x => GetAbsoluteFieldIndex(x.Value, maxLength)).Min() - index;
                }
                else
                {
                    // Ok, this field goes to the end of the packet
                    length = maxLength - index;
                }
            }

            return length;
        }

        public void AddField(string key, IField field)
        {
            Fields.Add(key, field);
        }

        public void RemoveField(string key)
        {
            Fields.Remove(key);
        }

        public void SetStartFlag(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                AddField(nameof(StartFlag), new Field(0, value.Length, value, int.MinValue, true));
            }
        }

        public void SetEndFlag(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                AddField(nameof(EndFlag), new Field(-1, value.Length, value, int.MaxValue, true));
            }
        }

        public void SetPayload(int index)
        {
            AddField(nameof(Payload), new Field(index, 0, "", false));
        }

        /*
         * TODO:
         * Need the ability to set a dynamic length as/with:
         *  - Binary (length of 10 = "some crazy control char")
         * This should be dead-simple to use yet allow users high flexibility
         */
        public void SetDynamicLength(int index, string formatSpecifier = "")
        {
            // Function for converting the dynamic length from an int to an string
            // We need this as a dyanmic lenght of 72 could be ASCII = "72" or Binary = "a"
            Func<int, string> convertIntToStringFunc = null;

            // Function for converting the dynamic length from a string to an int
            // The functional inverse of convertIntToStringFunc
            Func<string, int> convertStringToIntFunc = null;

            convertIntToStringFunc = i =>
            {
                return i.ToString(formatSpecifier);
            };

            convertStringToIntFunc = s =>
            {
                int length = -1;

                int.TryParse(s, out length);

                return length;
            };
            
            DynamicLengthConverter = new DynamicLength(convertIntToStringFunc, convertStringToIntFunc);
        }

        public void SetMaxLength(int length)
        {
            MaxLength = length;
        }

        public void SetFec(IFec fec, int index, int length)
        {
            Fec = fec;

            AddField(nameof(Ecc), new Field(index, length, new string('\0', length)));
        }

        internal void SetFec(IFec fec)
        {
            Fec = fec;
        }

        public void SetByteStuff(IByteStuff byteStuff)
        {
            ByteStuff = byteStuff;
        }

        public string ComputeEcc()
        {
            string value = string.Concat(Fields.Where(x => x.Key != nameof(Ecc))
                                               .OrderBy(x => x.Value.Order)
                                               .ThenBy(x => x.Key)
                                               .Select(x => x.Value));

            return Fec?.Compute(value);
        }

        public bool HasValidEcc(IFec fec)
        {
            bool hasValidEcc = false;

            Fields.TryGetValue(nameof(Ecc), out IField expected);

            if (expected?.Value != null)
            {
                hasValidEcc = ComputeEcc() == expected.Value;
            }

            return hasValidEcc;
        }

        public ISerialPacket DeepClone()
        {
            return new SerialPacket(this);
        }

        object IDeepCloneable.DeepClone()
        {
            return DeepClone();
        }

        public ISerialPacket ParseFrame(string frame)
        {
            int index = 0;
            int length = 0;

            ISerialPacket serialPacket = DeepClone();

            string decodedFrame = ByteStuff?.Unstuff(frame) ?? frame;

            // Sort serial packet fields by order, then by key (name)
            // Then, for each field, get the index, length, and value
            foreach (IField field in serialPacket.Fields.OrderBy(x => x.Value.Order).ThenBy(x => x.Key).Select(x => x.Value))
            {
                index = GetAbsoluteFieldIndex(field, frame.Length);
                length = GetFieldLength(field, frame.Length);

                field.Value = decodedFrame.Substring(index, length);
            }

            return serialPacket;
        }
    }
}
