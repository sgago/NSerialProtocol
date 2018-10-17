using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NSerialProtocol.SerialFrameParsers
{
    public class EndFlagParser : SerialFrameParser, IParser
    {
        private const string BadEndFlagErrorMessage = "End flag for a frame cannot be null or the empty string.";

        private const string EndFlagPattern = @"((?!{0}).)*{0}";

        private const RegexOptions Options =
            RegexOptions.CultureInvariant |
            RegexOptions.Multiline;

        private readonly string EndFlag = "";

        private readonly Regex EndFlagRegex;

        public EndFlagParser(string endFlag)
        {
            if (string.IsNullOrEmpty(endFlag))
            {
                throw new ArgumentException(BadEndFlagErrorMessage, nameof(endFlag));
            }

            EndFlag = RegexMetaToLiteral(endFlag);

            EndFlagRegex = new Regex(string.Format(EndFlagPattern, EndFlag), Options);
        }

        public override IList<string> Parse(IList<string> values)
        {
            List<string> matches = new List<string>();

            foreach (string value in values)
            {
                matches.AddRange(
                    EndFlagRegex
                        .Matches(value)
                        .Cast<Match>()
                        .Select(x => x.Value)
                        .Where(x => x != string.Empty)
                        .ToList()
                );
            }

            return matches;
        }
    }
}
