using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace NSerialProtocol.FrameParsers
{
    public enum ParsingError
    {
        NoStartFlag,
        
    }

    public abstract class FrameParser : IFrameParser
    {
        private IFrameParser Successor { get; set; }

        public abstract IList<string> Parse(IList<string> values);

        public IList<string> Parse(string value)
        {
            return Parse(new List<string>() { value });
        }

        public void SetSuccessor(IFrameParser nextParser)
        {
            Successor = nextParser;
        }

        protected string RegexMetaToLiteral(string value)
        {
            string result = string.Empty;

            if (!string.IsNullOrEmpty(value))
            {
                // Regex pattern of regex meta characters to replace
                string regexMetaCharactersPattern = @"(?=[\\^\$\{\}\[\]\(\)\.\*\+\?\|\<\>\&])";

                // Puts a "\" in front of all regex meta characters
                result = Regex.Replace(value, regexMetaCharactersPattern, @"\");
            }

            return result;
        }
    }
}
