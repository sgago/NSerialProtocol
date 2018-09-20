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
        /// Initializes a new instance of the Checksum8 class.
        /// </summary>
        public Checksum8()
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
        public byte[] Compute(byte[] values)
        {
            byte checksum8 = 0;

            foreach(byte b in values)
            {
                // Sum the byte values, ignoring overflow
                checksum8 = unchecked((byte)(checksum8 + b));
            }

            // TODO: What is the encoding of the returned string????
            return new byte[] { checksum8 };
        }

        public byte[] Compute(string value, Encoding encoding)
        {
            return Compute(encoding.GetBytes(value));
        }

        public byte[] Compute(string value)
        {
            return Compute(value, Encoding.Default);
        }
    }
}
