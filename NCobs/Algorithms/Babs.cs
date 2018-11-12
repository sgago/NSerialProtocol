using System.Collections.Generic;
using System.Linq;

namespace NByteStuff.Algorithms
{
    /// <summary>
    /// Bandwidth-efficient byte stuffing (BABS) algorithm.
    /// </summary>
    /// <see cref="http://www.inescporto.pt/~jsc/publications/conferences/2007JaimeICC.pdf"/>
    /// <see cref="http://ieeexplore.ieee.org/xpl/login.jsp?tp=&arnumber=4289693&url=http%3A%2F%2Fieeexplore.ieee.org%2Fxpls%2Fabs_all.jsp%3Farnumber%3D4289693"/>
    /// </remarks>
    public class Babs : IByteStuff
    {
        private const int DefaultStuffedNumberBase = 127;
        private const int DefaultUnstuffedNumberBase = 128;

        /// <summary>
        /// Gets the number base of the decoded string.
        /// </summary>
        protected int UnstuffedNumberBase { get; set; } = DefaultUnstuffedNumberBase;

        /// <summary>
        /// Gets the number base of the encoded string.
        /// </summary>
        protected int StuffedNumberBase { get; set; } = DefaultStuffedNumberBase;

        /// <summary>
        /// Instantiates a new instance of the Bandwidth-efficient byte stuffing (BABS) algorithm.
        /// </summary>
        /// <param name="unstuffedNumberBase"></param>
        /// <param name="stuffedNumberBase"></param>
        public Babs(int unstuffedNumberBase, int stuffedNumberBase)
        {
            StuffedNumberBase = stuffedNumberBase;
            UnstuffedNumberBase = unstuffedNumberBase;
        }

        /// <summary>
        /// Instantiates a new instance of the Bandwidth-efficient byte stuffing (BABS) algorithm
        /// with
        /// </summary>
        public Babs()
            : this(DefaultUnstuffedNumberBase, DefaultStuffedNumberBase)
        {

        }

        /// <summary>
        /// Treats a string as a single value and converts it from
        /// one base to another.
        /// </summary>
        /// <param name="dataIn">
        /// A byte array with the most-significant byte value at index position 0.
        /// </param>
        /// <param name="baseIn">The input number base of the value string.</param>
        /// <param name="baseOut">The output number base of the returned string.</param>
        /// <returns>The converted byte array value.</returns>
        /// <remarks>
        /// 
        /// </remarks>
        /// <example>
        /// 
        /// 
        /// 
        /// </example>
        public List<byte> Convert(List<byte> dataIn, int baseIn, int baseOut)
        {
            int extraZeroes = 0;
            int limSup = dataIn.Count - 1;
            List<byte> dataOut = new List<byte>();

            // Need to reverse byte order for this to work
            dataIn.Reverse();

            while (limSup >= 0 && dataIn[limSup] == 0)
            {
                limSup--;
            }

            extraZeroes = dataIn.Count - 1 - limSup;

            for (int i = 0; limSup >= 0; i++)
            {
                ushort temporary = 0;

                for (int j = limSup; j >= 0; j--)
                {
                    temporary = (ushort)(temporary * baseIn + dataIn[j]);
                    dataIn[j] = (byte)(temporary / (baseOut));
                    temporary -= (ushort)(dataIn[j] * (baseOut));
                }

                dataOut.Insert(0, (byte)temporary);

                while ((limSup >= 0) && (dataIn[limSup] == 0))
                {
                    limSup--;
                }
            }

            for (; extraZeroes > 0; extraZeroes--)
            {
                dataOut.Insert(0, 0);
            }


            return dataOut;
        }

        /// <summary>
        /// Encodes the specified string value by removing all
        /// byte values instances of 0xFF (255).
        /// </summary>
        /// <param name="value">The string to encode.</param>
        /// <returns>The encoded string value.</returns>
        public virtual byte[] Stuff(byte[] value)
        {
            return Convert(value.ToList(), UnstuffedNumberBase, StuffedNumberBase).ToArray();
        }

        /// <summary>
        /// Decodes the specified, BABS-encoded string value.
        /// </summary>
        /// <param name="value">The string value to decode.</param>
        /// <returns>The decoded string value.</returns>
        public virtual byte[] Unstuff(byte[] value)
        {
            return Convert(value.ToList(), StuffedNumberBase, UnstuffedNumberBase).ToArray();
        }
    }
}
