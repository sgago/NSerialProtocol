namespace NByteStuff
{
    /// <summary>
    /// Represents a byte stuffing algorithm which removes byte values
    /// used for packet boundary demarcation or control.
    /// </summary>
    public interface IByteStuff
    {
        /// <summary>
        /// Encodes the specified string by removing all byte values used as control characters
        /// or to demarcate packet boundaries.
        /// </summary>
        /// <param name="value">The string to encode.</param>
        /// <returns>The byte stuffed transformation of the text.</returns>
        byte[] Stuff(byte[] value);

        /// <summary>
        /// Decodes a byte stuffed string to its original value.
        /// </summary>
        /// <param name="value">The string to decode.</param>
        /// <returns>The original, decoded string value.</returns>
        byte[] Unstuff(byte[] value);
    }
}
