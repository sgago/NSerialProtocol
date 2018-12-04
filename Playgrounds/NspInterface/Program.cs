using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NspInterface
{
    using NSerialProtocol;

    class Program
    {
        static void Main(string[] args)
        {
            NSerialProtocol protocol = new NSerialProtocol();

            Predicate<ISerialFrame> isAckFrame = new Predicate<ISerialFrame>((sf) => true );
            Action<ISerialFrame> thenDoAction = new Action<ISerialFrame>((sf) => Console.WriteLine("some action"));

            protocol.OnFrameReceived<ISerialFrame>()
                .If(isAckFrame, thenDoAction)
                .Do((sf) => Console.WriteLine("Do something"));
        }
    }
}
