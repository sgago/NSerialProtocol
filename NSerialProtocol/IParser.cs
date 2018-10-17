using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSerialProtocol
{
    public interface IParser
    {
        IList<string> Parse(IList<string> values);

        IList<string> Parse(string value);

        void SetSuccessor(IParser nextParser);
    }
}
