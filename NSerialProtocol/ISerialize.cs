using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSerialProtocol
{
    public interface ISerialize
    {
        byte[] Serialize();

        T Deserialize<T>(byte[] bytes);
    }
}
