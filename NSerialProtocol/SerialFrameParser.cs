using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NSerialProtocol
{
    public enum FrameErrorType
    {
        BufferOverrun,
        InvalidLength
    }

    public class SerialFrameParser
    {
        protected RegexOptions RegexOptions { get; } =
            RegexOptions.CultureInvariant | RegexOptions.Multiline;
    }
}
