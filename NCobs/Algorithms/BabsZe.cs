namespace NByteStuff.Algorithms
{
    /// <summary>
    /// BABS Zero Elimination (BABS/ZE) byte-stuffing algorithm.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class BabsZe : Babs
    {
        // TODO: Inherit babs, but then add
        // one to each array element when stuffing after BABS runs
        // and subtract one when unstuffing before BABS runs.
        public override byte[] Stuff(byte[] dataIn)
        {
            byte[] dataOut = base.Stuff(dataIn);

            for (int i = 0; i < dataOut.Length; i++)
            {
                dataOut[i]++;
            }

            return dataOut;
        }

        public override byte[] Unstuff(byte[] dataIn)
        {
            for (int i = 0; i < dataIn.Length; i++)
            {
                dataIn[i]--;
            }

            return base.Unstuff(dataIn);
        }
    }
}
