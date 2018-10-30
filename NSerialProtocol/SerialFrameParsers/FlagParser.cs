using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace NSerialProtocol.SerialFrameParsers
{
    public class FlagParser : FrameParser, IFrameParser
    {
        private const string BadFlagErrorMessage = "Flag cannot be null or the empty string.";

        private const RegexOptions Options =
            RegexOptions.Multiline |
            RegexOptions.CultureInvariant;
          
        private readonly string StartFlag = "";

        private readonly string EndFlag = "";

        /// <summary>
        /// Regex pattern for no delimiters or fixed length packets.
        /// </summary>
        private const string NoFlagsPattern = @"[\s\S]*";

        /// <summary>
        /// Regex pattern for only a starting flag used to demarcate the
        /// start of the packet with an optional fixed length.
        /// </summary>
        private const string StartFlagPattern = @"{0}((?!{0})[\s\S]){2}";

        /// <summary>
        /// Regex pattern for only an ending flag used to demarcate the
        /// end of the packet with an optional fixed length.
        /// </summary>
        private const string EndFlagPattern = @"((?!{1})[\s\S]){2}{1}";

        /// <summary>
        /// Regex pattern for a packet of fixed length without delimiters.
        /// </summary>
        private const string FixedLengthPattern = @"[\s\S]{2}";

        /// <summary>
        /// Regex pattern for a packet consisting of a starting and ending flags and an
        /// optional fixed length.
        /// </summary>
        private const string StartFlagEndFlagPattern = @"{0}((?!{1})[\s\S]){2}{1}";

        private readonly Regex FlagRegex;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="endFlag"></param>
        /// <param name="startFlag"></param>
        /// <remarks>
        /// The parameters of this constructor are flipped.
        /// Ideally, this will encourage folks to avoid a start-flag-only
        /// protocol.  They still can have a start-flag-only protocol, but
        /// they need to willingly code it in versus by mistake.
        /// </remarks>
        public FlagParser(string endFlag, string startFlag = "")
        {
            if (!string.IsNullOrEmpty(startFlag))
            {
                StartFlag = RegexMetaToLiteral(startFlag);
            }
                
            if (!string.IsNullOrEmpty(endFlag))
            {
                EndFlag = RegexMetaToLiteral(endFlag);
            }
            
            FlagRegex = new Regex(GetRegexPattern(StartFlag, EndFlag), Options);
        }

        private string GetRegexPattern(string startFlag, string endFlag)
        {
            string pattern;

            if (!string.IsNullOrEmpty(startFlag) && !string.IsNullOrEmpty(endFlag))
            {
                pattern = StartFlagEndFlagPattern;
            }
            else if (!string.IsNullOrEmpty(startFlag))
            {
                pattern = StartFlagPattern;
            }
            else if (!string.IsNullOrEmpty(endFlag))
            {
                pattern = EndFlagPattern;
            }
            else
            {
                pattern = NoFlagsPattern;
            }

            pattern = string.Format(pattern, StartFlag, EndFlag, "*");

            return pattern;
        }

        public override IList<string> Parse(IList<string> values)
        {
            List<string> matches = new List<string>();

            foreach (string value in values.Where(x => !string.IsNullOrEmpty(x)))
            {
                matches.AddRange(
                    FlagRegex
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
