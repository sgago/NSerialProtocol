using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NSerialProtocol
{
    public enum ParsingError
    {
        NoStartFlag,
        
    }

    public abstract class SerialFrameParser : IParser
    {
        private IParser Successor { get; set; }

        public abstract IList<string> Parse(IList<string> values);

        public IList<string> Parse(string value)
        {
            return Parse(new List<string>() { value });
        }

        public void SetSuccessor(IParser nextParser)
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
