namespace NFec.Extensions
{
    using System;
    using System.Text;

    /// <summary>
    /// Provides extensions for the Encoding class.
    /// </summary>
    public static class EncodingExtensions
    {
        public static ushort[] ToUInt16Array(this Encoding encoding, byte[] inArray)
        {
            // TODO: What if inArray is a weird size like 3 or 5?
            ushort[] outArray = new ushort[inArray.Length / 2];

            Buffer.BlockCopy(inArray, 0, outArray, 0, inArray.Length);

            return outArray;
        }

        public static uint[] ToUInt32Array(this Encoding encoding, byte[] inArray)
        {
            // TODO: What if inArray is a weird size like 3 or 5?
            uint[] outArray = new uint[inArray.Length / 4];

            Buffer.BlockCopy(inArray, 0, outArray, 0, inArray.Length);

            return outArray;
        }
    }
}
