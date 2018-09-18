
namespace NSerialProtocol
{
    using System;

    /// <summary>
    /// Represents an individual field in a serial packet.
    /// </summary>
    public interface IField
    {
        /// <summary>
        /// Gets or sets the starting index of this serial packet field.
        /// </summary>
        int Index { get; set; }

        /// <summary>
        /// Gets or sets the length of this serial packet field.
        /// </summary>
        int Length { get; set; }

        /// <summary>
        /// True if the length of this serial packet field is known; otherwise, false.
        /// </summary>
        bool KnownLength { get; set; }

        /// <summary>
        /// Gets or sets a function which returns a string representing the
        /// value of this field.
        /// </summary>
        Func<string> ValueFunc { get; set; }

        /// <summary>
        /// Gets or sets the value of this serial packet field.
        /// </summary>
        string Value { get; set; }

        /// <summary>
        /// Gets or sets the order in which this field is added
        /// to a serial packet.
        /// </summary>
        int Order { get; set; }
    }

    /// <summary>
    /// Represents an individual field in a serial packet.
    /// </summary>
    public class Field : IField, IEquatable<Field>, IDeepCloneable<Field>
    {
        /// <summary>
        /// The starting index of this serial packet field.
        /// </summary>
        private int index = 0;

        /// <summary>
        /// The length of this serial packet field.
        /// </summary>
        private int length = 0;

        /// <summary>
        /// True if the length is known; otherwise, false.
        /// </summary>
        bool knownLength = true;

        /// <summary>
        /// The value of this serial packet field.
        /// </summary>
        private string value = string.Empty;

        /// <summary>
        /// A function which returns a string representing the value of this field.
        /// </summary>
        private Func<string> valueFunc = null;

        /// <summary>
        /// The order in which fields values should be added to the serial packet.
        /// </summary>
        private int order = 0;

        /// <summary>
        /// Gets or sets the starting index of this serial packet field.
        /// </summary>
        public int Index
        {
            get
            {
                return index;
            }

            set
            {
                index = value;
            }
        }

        /// <summary>
        /// Gets or sets the length of this serial packet field.
        /// </summary>
        public int Length
        {
            get
            {
                return length;
            }

            set
            {
                length = value;
            }
        }

        /// <summary>
        /// True if the length of this serial packet field is known; otherwise, false.
        /// </summary>
        public bool KnownLength
        {
            get
            {
                return knownLength;
            }

            set
            {
                knownLength = value;
            }
        }

        /// <summary>
        /// Gets or sets a function which returns a string representing the
        /// value of this field.
        /// </summary>
        public Func<string> ValueFunc
        {
            get
            {
                return valueFunc;
            }

            set
            {
                valueFunc = value;
            }
        }

        /// <summary>
        /// Gets or sets the value of this serial packet field.
        /// </summary>
        public string Value
        {
            get
            {
                return ValueFunc?.Invoke() ?? value;
            }

            set
            {
                ValueFunc = null;
                this.value = value;
            }
        }

        /// <summary>
        /// Gets or sets the order in which this field is added
        /// to a serial packet.
        /// </summary>
        public int Order
        {
            get
            {
                return order;
            }

            set
            {
                order = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the Field class.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="length"></param>
        /// <param name="value"></param>
        /// <param name="order"></param>
        /// <param name="knownLength"></param>
        public Field(int index, int length, string value, int order, bool knownLength = true)
        {
            Index = index;
            Length = length;
            Value = value;
            Order = order;
            KnownLength = knownLength;
        }

        /// <summary>
        /// Initializes a new instance of the Field class.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="length"></param>
        /// <param name="value"></param>
        /// <param name="order"></param>
        /// <param name="knownLength"></param>
        public Field(int index, int length, Func<string> value, int order, bool knownLength = true)
        {
            Index = index;
            Length = length;
            ValueFunc = value;
            Order = order;
            KnownLength = knownLength;
        }

        /// <summary>
        /// Initializes a new instance of the Field class.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="length"></param>
        /// <param name="value"></param>
        /// <param name="knownLength"></param>
        public Field(int index, int length, string value, bool knownLength = true)
            : this(index, length, value, int.MaxValue - 1, knownLength)
        {
            Index = index;
            Length = length;
            Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the Field class.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="length"></param>
        /// <param name="value"></param>
        /// <param name="knownLength"></param>
        public Field(int index, int length, Func<string> value, bool knownLength = true)
            : this(index, length, value, int.MaxValue - 1, knownLength)
        {
            Index = index;
            Length = length;
            ValueFunc = value;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="field"></param>
        private Field(IField field)
        {
            Index = field.Index;
            Length = field.Length;
            ValueFunc = field.ValueFunc;
            Value = field.Value;
            Order = field.Order;
            KnownLength = field.KnownLength;
        }

        public Field DeepClone()
        {
            return new Field(this);
        }

        object IDeepCloneable.DeepClone()
        {
            return DeepClone();
        }

        public override bool Equals(object obj)
        {
            bool equals = false;
            Field field;

            if (ReferenceEquals(this, obj))
            {
                equals = true;
            }
            else
            {
                field = obj as Field;

                equals = Equals(field);
            }

            return equals;
        }

        public bool Equals(Field field)
        {
            bool equals = false;

            if (!(field is null))
            {
                equals =
                    Index == field.Index &&
                    Length == field.Length &&
                    ValueFunc == field.ValueFunc &&
                    Value == field.Value &&
                    Order == field.Order &&
                    KnownLength == field.KnownLength;
            }

            return equals;
        }

        /// <summary>
        /// Gets a hashcode for this object.
        /// </summary>
        /// <returns>A hashcode for the current object.</returns>
        public override int GetHashCode()
        {
            int hash = 17;
            int valueFuncHash = ValueFunc?.GetHashCode() ?? 0;
            int valueHash = Value?.GetHashCode() ?? 0;

            unchecked
            {
                hash = hash * 23 + Index.GetHashCode();
                hash = hash * 23 + Length.GetHashCode();
                hash = hash * 23 + valueFuncHash;
                hash = hash * 23 + valueHash;
                hash = hash * 23 + Order.GetHashCode();
                hash = hash * 23 + KnownLength.GetHashCode();
            }

            return hash;
        }

        public static bool operator ==(Field a, Field b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Field a, Field b)
        {
            return !(a == b);
        }
    }
}
