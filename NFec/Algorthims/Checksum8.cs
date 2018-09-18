namespace NFec.Algorithms
{
    using System;
    using System.Text;

    /// <summary>
    /// Calculates the 8-bit checksum of a string
    /// and returns the result in plain ASCII text.
    /// </summary>
    public class Checksum8 : IFec
    {
        /// <summary>
        /// The encoding of the received
        /// </summary>
        private Encoding encoding;

        /// <summary>
        /// Gets or sets the encoding used to
        /// convert strings to byte arrays and vice versa.
        /// </summary>
        protected Encoding Encoding
        {
            get
            {
                return encoding;
            }

            set
            {
                encoding = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the Checksum8Ascii using a specified
        /// encoding to convert strings to byte arrays.
        /// </summary>
        /// <param name="encoding">The string encoding used.</param>
        public Checksum8(Encoding encoding)
        {
            Encoding = encoding;
        }

        /// <summary>
        /// Initializes a new instance of the Checksum8Ascii.
        /// </summary>
        public Checksum8()
            : this(Encoding.Unicode)
        {
        }

        /// <summary>
        /// Computes the 8-bit checksum of a string in plain text.
        /// </summary>
        /// <param name="value">String to calculate the 8-bit checksum for.</param>
        /// <returns>The computed error correction code (ECC).</returns>
        /// <remarks>
        /// Note that the returned error correction code (ECC) is returned
        /// in plain text with a length of 2 characters instead of a single byte
        /// value.
        /// </remarks>
        public byte[] Compute(byte[] bytes)
        {
            byte checksum8 = 0;

            foreach(byte b in bytes)
            {
                // Sum the byte values, ignoring overflow
                checksum8 = unchecked((byte)(checksum8 + b));
            }

            // TODO: What is the encoding of the returned string????
            return BitConverter.GetBytes(checksum8);
        }
    }
}
