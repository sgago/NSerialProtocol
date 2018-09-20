namespace NByteStuff.Algorithms
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Bandwidth-efficient byte stuffing (BABS) algorithm.
    /// </summary>
    /// <remarks>
    /// BABS treats strings as single numbers of a specified number base.
    /// Encoding is performed by converting the string into a smaller number base
    /// and vice versa for decoding.
    ///
    /// For instance:
    ///
    ///         Base 127 => Base 126
    /// Max value is 127 => Max value is now 126
    ///              127 => 1 0
    ///
    ///          Base 256 => Base 255
    ///  Max value is 255 => Max value is now 254
    ///               255 => 1 0
    ///             0 255 => 0 1 0
    ///           0 0 255 => 0 0 1 0
    /// 0 253 255 250 255 => 0 1 1 254 247 248
    ///
    /// Note that the
    ///
    /// <see cref="http://www.inescporto.pt/~jsc/publications/conferences/2007JaimeICC.pdf"/>
    /// <see cref="http://ieeexplore.ieee.org/xpl/login.jsp?tp=&arnumber=4289693&url=http%3A%2F%2Fieeexplore.ieee.org%2Fxpls%2Fabs_all.jsp%3Farnumber%3D4289693"/>
    /// </remarks>
    public class Babs : IByteStuff
    {
        private const int DefaultEncodeNumberBase = 127;
        private const int DefaultDecodeNumberBase = 128;

        /// <summary>
        /// Number base of the decoded string.
        /// </summary>
        private readonly int decodedNumberBase;

        /// <summary>
        /// Number base of the encoded string.
        /// </summary>
        private readonly int encodedNumberBase;

        /// <summary>
        /// The encoding of the string arguments to the Stuff and Unstuff methods.
        /// </summary>
        private readonly Encoding inputEncoding;

        /// <summary>
        /// Gets the number base of the decoded string.
        /// </summary>
        protected int DecodedNumberBase
        {
            get
            {
                return decodedNumberBase;
            }
        }

        /// <summary>
        /// Gets the number base of the encoded string.
        /// </summary>
        protected int EncodedNumberBase
        {
            get
            {
                return encodedNumberBase;
            }
        }

        /// <summary>
        /// Gets the input string's encoding.
        /// </summary>
        protected Encoding InputEncoding
        {
            get
            {
                return inputEncoding;
            }
        }

        /// <summary>
        /// Instantiates a new instance of the Bandwidth-efficient byte stuffing (BABS) algorithm.
        /// </summary>
        /// <param name="decodedNumberBase"></param>
        /// <param name="encodedNumberBase"></param>
        /// <param name="inputEncoding"></param>
        public Babs(int decodedNumberBase, int encodedNumberBase, Encoding inputEncoding)
        {
            this.decodedNumberBase = decodedNumberBase;
            this.encodedNumberBase = encodedNumberBase;
            this.inputEncoding = inputEncoding;
        }

        /// <summary>
        /// Instantiates a new instance of the Bandwidth-efficient byte stuffing (BABS) algorithm
        /// with
        /// </summary>
        public Babs()
            : this(DefaultDecodeNumberBase, DefaultEncodeNumberBase, Encoding.ASCII)
        {

        }

        /// <summary>
        /// Encodes the specified string value by removing all
        /// byte values instances of 0xFF (255).
        /// </summary>
        /// <param name="value">The string to encode.</param>
        /// <returns>The encoded string value.</returns>
        public string Stuff(string value)
        {
            return Convert(value, DecodedNumberBase, EncodedNumberBase, InputEncoding);
        }

        /// <summary>
        /// Decodes the specified, BABS-encoded string value.
        /// </summary>
        /// <param name="value">The string value to decode.</param>
        /// <returns>The decoded string value.</returns>
        public string Unstuff(string value)
        {
            return Convert(value, EncodedNumberBase, DecodedNumberBase, InputEncoding);
        }

        /// <summary>
        /// Treats a string as a single value and converts it from
        /// one base to another.
        /// </summary>
        /// <param name="value">The specified string value to convert with the
        /// least significant value at index 0.</param>
        /// <param name="baseIn">The input number base of the value string.</param>
        /// <param name="baseOut">The output number base of the returned string.</param>
        /// <param name="inputEncoding">The encoding of the input string.</param>
        /// <returns>The converted string value with the Unicode encoding.</returns>
        /// <remarks>
        /// The least significant value of the output and input strings is located at index 0.
        /// </remarks>
        private string Convert(string value, int baseIn, int baseOut, Encoding inputEncoding)
        {
            List<byte> dataIn = inputEncoding.GetBytes(value).ToList();
            List<byte> dataOut = new List<byte>();
            int extraZeroes = 0;
            int limSup = dataIn.Count - 1;

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

                //dataOut.Insert(i, (byte)temporary);
                dataOut.Insert(0, (byte)temporary);

                while ((limSup >= 0) && (dataIn[limSup] == 0))
                {
                    limSup--;
                }
            }

            for (; extraZeroes > 0; extraZeroes--)
            {
                //dataOut.Add(0);
                dataOut.Insert(0, 0);
            }

            return string.Join("", dataOut.Select(x => (char)x));
        }

        public byte[] Stuff(byte[] value)
        {
            throw new System.NotImplementedException();
        }

        public byte[] Unstuff(byte[] value)
        {
            throw new System.NotImplementedException();
        }
    }
}
