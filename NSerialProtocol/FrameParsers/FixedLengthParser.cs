using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NSerialProtocol.FrameParsers
{
    public class FixedLengthParser : FrameParser, IFrameParser
    {
        private const string BadLengthErrorMessage = "Fixed frame length cannot be less than or equal to 0.";

        private readonly int FixedLength = -1;

        public FixedLengthParser(int fixedLength)
        {
            if (fixedLength <= 0)
            {
                throw new ArgumentException(BadLengthErrorMessage, nameof(fixedLength));
            }

            FixedLength = fixedLength;
        }

        public override IList<string> Parse(IList<string> values)
        {
            List<string> matches = new List<string>();

            foreach (string value in values)
            {
                if (value.Length == FixedLength)
                {
                    matches.Add(value);
                }
            }

            return matches;
        }
    }
}
