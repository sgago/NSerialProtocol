using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSerialProtocol
{
    public enum ParserError
    {

    }

    /// <summary>
    /// 
    /// </summary>
    /*
    /// Flags are parsed first as their values
    /// are constant.
    /// 
    /// What comes Next: ByteStuffing or Length?
    /// Length poses a fun problem.  A length can be any value;
    /// therefore, this could result in transmission errors
    /// if the length is the same as one of the flags.
    /// 
    /// However, what if the programmer is using length to
    /// demarcate the start and end of a serial frame?
    /// This would allow us to ignore flag "collisions"
    /// because we now know how many bytes to read.
    /// 
    /// Solution:
    /// Let's let the programmer decide.  If they want the length
    /// Include another attribute value where programmers
    /// can check yes I want this value byte stuffed and
    /// check yes I want this included in the CRC calculation.
    /// 
    /// STX LENGTH DATA ETX
    /// STX 3 ETX ETX ETX ETX // Payload length
    /// STX 5 ETX ETX ETX ETX // Frame length
    /// 
    /// Issues:
    /// 1. Parser order: Flags then length
    /// STX 3 ETX ETX ETX ETX =
    /// STX 3 ETC (Error, bad length), Garbage ETX, Garbage ETX, Garbage ETX
    /// 
    /// 2. Parser order: Length then flags
    /// STX 5 ETX ETX ETX ETX = Good
    /// STX 3 ETX ETX ETX ETX = Good
    /// 
    /// 5 ETX ETX ETX ETX STX = Good but later bad
    /// 
    /// STX 5 ETX ETX ETX STX ETX = Good but later bad
    /// 
    */
    public enum ParserType
    {
        /// <summary>
        /// 
        /// </summary>
        Flags = 0,

        /// <summary>
        /// 
        /// </summary>
        Length = 100,

        /// <summary>
        /// 
        /// </summary>
        ByteStuff = 200,

        /// <summary>
        /// 
        /// </summary>
        Fec = 300,
    }

    public class FrameParserChain
    {


        public FrameParserChain()
        {

        }
    }
}
