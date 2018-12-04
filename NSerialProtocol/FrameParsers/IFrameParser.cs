using System.Collections.Generic;

namespace NSerialProtocol
{
    public interface IFrameParser
    {
        IList<string> Parse(IList<string> values);

        IList<string> Parse(string value);

        void SetSuccessor(IFrameParser nextParser);
    }
}
