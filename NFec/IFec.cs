using System.Text;

namespace NFec
{
    /// <summary>
    /// Represents a forward error correction (FEC) algorithm
    /// used to compute error correction codes (ECC) such as
    /// checksums and cyclic redundancy checks (CRC).
    /// </summary>
    public interface IFec
    {
        /// <summary>
        /// Computes the error correction code (ECC) for a given byte array.
        /// </summary>
        /// <param name="value">String to calculate error correction code (ECC) for.</param>
        /// <returns>The computed error correction code (ECC).</returns>
        byte[] Compute(byte[] value);

        /// <summary>
        /// Computes the error correction code (ECC) of a string using the specified
        /// encoding.
        /// </summary>
        /// <param name="value">String to calculate the error correction code (ECC) for.</param>
        /// <param name="encoding">The encoding used to convert the string to a byte array.</param>
        /// <returns>The computed error correction code (ECC) as a byte array.</returns>
        byte[] Compute(string value, Encoding encoding);

        /// <summary>
        /// Computes the error correction code (ECC) of a string using the Default encoding.
        /// </summary>
        /// <param name="value">
        /// String to calculate the error correction code (ECC) for.
        /// The Default encoding is used to convert the string to a byte array.
        /// </param>
        /// <returns>The computed error correction code (ECC) as a byte array.</returns>
        byte[] Compute(string value);
    }
}
