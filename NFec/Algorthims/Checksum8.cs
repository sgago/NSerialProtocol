using System.Text;

namespace NFec.Algorithms
{
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
        /// <returns>The computed error correction code (ECC) as a byte array.</returns>
        public byte[] Compute(byte[] values)
        {
            byte checksum8 = 0;

            foreach(byte b in values)
            {
                // Sum the byte values, ignoring overflow
                checksum8 = unchecked((byte)(checksum8 + b));
            }
            
            return new byte[] { checksum8 };
        }

        /// <summary>
        /// Computes the 8-bit checksum of a string in plain text.
        /// </summary>
        /// <param name="value">String to calculate the 8-bit checksum for.</param>
        /// <param name="encoding">The encoding used to convert the string to a byte array.</param>
        /// <returns>The computed error correction code (ECC) as a byte array.</returns>
        public byte[] Compute(string value, Encoding encoding)
        {
            return Compute(encoding.GetBytes(value));
        }

        /// <summary>
        /// Computes the 8-bit checksum of a string in plain text.
        /// </summary>
        /// <param name="value">
        /// String to calculate the 8-bit checksum for.
        /// The Default encoding is used to convert the string to a byte array.
        /// </param>
        /// <returns>The computed error correction code (ECC) as a byte array.</returns>
        public byte[] Compute(string value)
        {
            return Compute(value, Encoding.Default);
        }
    }
}
