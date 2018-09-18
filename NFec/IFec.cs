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
        /// Computes the error correction code (ECC) for a
        /// given string value.
        /// </summary>
        /// <param name="value">String to calculate error correction code (ECC) for.</param>
        /// <returns>The computed error correction code (ECC).</returns>
        byte[] Compute(byte[] value);
    }
}
