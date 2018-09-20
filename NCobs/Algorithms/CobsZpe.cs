namespace NCobs.Algorithms
{
    using NByteStuff;
    using System;

    // Cobs zero pair elimination (ZPE).  Rules:
    // 1. Zero is not allowed
    // 1. Add an overhead byte to the start of the packet
    // 2. 01 - DF (1 - 223) data bytes followed by a single zero (like normal cobs)
    // 3. E0 for 223 data bytes not followed by a zero
    // 4. E1 - FF for 0 to 30 data bytes followed by 2 (pair of) zeros.
    // 5. Presumably, if a pair of zeros is is > 30 bytes, we use 1 - DF like normal.
    public class CobsZpe : IByteStuff
    {
        public string Stuff(string text)
        {
            throw new NotImplementedException();
        }

        public byte[] Stuff(byte[] value)
        {
            throw new NotImplementedException();
        }

        public string Unstuff(string text)
        {
            throw new NotImplementedException();
        }

        public byte[] Unstuff(byte[] value)
        {
            throw new NotImplementedException();
        }
    }
}