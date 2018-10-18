using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NSerialProtocol.SerialFrameParsers
{
    // TODO: StartFlag and EndFlag Parsers need to be merged into one
    // Otherwise, users won't be able to have the same starting and ending flag.
    public class StartFlagParser : SerialFrameParser, IParser
    {
        private const string BadStartFlagErrorMessage = "Start flag for a frame cannot be null or the empty string.";

        private const string StartFlagPattern = @"{0}((?!{0}).)*";

        private const RegexOptions Options =
            RegexOptions.CultureInvariant |
            RegexOptions.Multiline;

        private readonly string StartFlag = "";

        private readonly Regex StartFlagRegex;

        public StartFlagParser(string startFlag)
        {
            if (string.IsNullOrEmpty(startFlag))
            {
                throw new ArgumentException(BadStartFlagErrorMessage, nameof(startFlag));
            }

            StartFlag = RegexMetaToLiteral(startFlag);

            StartFlagRegex = new Regex(string.Format(StartFlagPattern, StartFlag), Options);
        }

        public override IList<string> Parse(IList<string> values)
        {
            List<string> matches = new List<string>();

            foreach (string value in values)
            {
                matches.AddRange(
                    StartFlagRegex
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
