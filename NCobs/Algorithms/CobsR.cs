namespace NByteStuff.Algorithms
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    // 1. Add an overhead byte which is 1 plus the number of non-zero bytes that follow
    // 2. If we encounter a zero in the data, replace it with the number of non-zero bytes that follow
    // 3. If the last data byte is greater than the usual length byte (that would replace a zero),
    //      replace the last zero with the last data byte (decoder can figure this out because
    //      the last length byte is greater than the remaining length of the packet
    public class CobsR : IByteStuff
    {
        public string Stuff(string text)
        {
            throw new NotImplementedException();
        }

        public string Unstuff(string text)
        {
            throw new NotImplementedException();
        }
    }
}
