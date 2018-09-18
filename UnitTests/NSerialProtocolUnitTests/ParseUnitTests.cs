/*
The MIT License (MIT)

Copyright (c) 2016 Steven Gago

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

// Naming standard = [UnitOfWork_StateUnderTest_ExpectedBehavior]
// TODO: TestCases for switching EndWith, StartWith, FixedLength, and Dynamic lengths on and off!!!
// TODO: We need test cases when the EndWith and StartWith use the same character.
// TODO: All dynamic tests need to try lengths less than the actual size of the message
// TODO: All dynamic length tests need test negative startindexes
// TODO: May have to try making input buffer overflow...
// StartWithDynamicLength, EndWithDynamicLength, StartWithEndWithFixedLength, StartWithEndWithDynamicLength

namespace NSerialProtocolUnitTests
{
    using NSerialProtocol;
    using NSerialProtocol.EventArgs;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Contains unit tests for SerialPacketPrototype class.
    /// </summary>
    //[TestFixture]
    public class ParseUnitTests
    {
        /// <summary>
        /// The minimum character decimal value.
        /// </summary>
        protected const int MinDelimiter = 0;

        /// <summary>
        /// The maximum character decimal value.
        /// </summary>
        // True maximum value is ushort.MaxValue (65535); however, this is quite machine intensive.
        // However, 255 includes white space, regex special characters, and the soft hypen which are good to check.
        protected const int MaxDelimiter = 255;

        //[TestCase]
        //public void DynamicLength_NegativeLength_ThrowsArgumentOutOfRangeException()
        //{
        //    int index = 1;
        //    int length = -2;
        //    ISerialPackets serialPackets = new SerialPacketPrototype();

        //    Assert.Throws(typeof(ArgumentOutOfRangeException),
        //        () => { serialPackets.DynamicLength(length, index); });
        //}

        protected IEnumerable<ISerialPacket> ParseSerialPackets(ISerialPacketPrototype serialPackets,
            StringBuilder inputBuffer)
        {
            IEnumerable<ISerialPacket> parsedSerialPackets = null;

            List<SerialPacketReceivedEventArgs> serialPacketReceivedEventArgs =
                new List<SerialPacketReceivedEventArgs>();

            serialPackets.SerialPacketReceived += (sender, serialPacketEventArgs) =>
            {
                serialPacketReceivedEventArgs.Add(serialPacketEventArgs);
            };

            serialPackets.Parse(inputBuffer);

            parsedSerialPackets = serialPacketReceivedEventArgs.Select(x => x.SerialPacket);

            return parsedSerialPackets;
        }

        protected IEnumerable<ISerialPacket> ParseSerialPacketErrors(ISerialPacketPrototype serialPackets,
            StringBuilder inputBuffer, FrameError packetError)
        {
            List<FrameErrorReceivedEventArgs> serialPacketErrorReceivedEventArgs =
                new List<FrameErrorReceivedEventArgs>();

            serialPackets.SerialPacketError += (sender, serialPacketEventArgs) =>
            {
                serialPacketErrorReceivedEventArgs.Add(serialPacketEventArgs);
            };

            serialPackets.Parse(inputBuffer);

            return serialPacketErrorReceivedEventArgs
                    .Where(x => x.FrameError == packetError)
                    .Select(x => x.SerialPacket);
        }


        //public static IEnumerable GetSerialPacket_StartWith_TestCases
        //{
        //    get
        //    {
        //        yield return new TestCaseData("!", "").Returns("!");
        //        yield return new TestCaseData("!", "a").Returns("!a");
        //    }
        //}

        //[TestCaseSource(nameof(GetSerialPacket_StartWith_TestCases))]
        //public string GetSerialPacket_StartWith_Test(string startDelimiter, string text)
        //{
        //    ISerialPackets serialPackets = new SerialPacketPrototype()
        //        .StartFlag(startDelimiter);

        //    return serialPackets.CreateSerialPacket(text);
        //}

        //public static IEnumerable GetSerialPacket_EndWith_TestCases
        //{
        //    get
        //    {
        //        yield return new TestCaseData("!", "").Returns("!");
        //        yield return new TestCaseData("!", "a").Returns("a!");
        //    }
        //}

        //[TestCaseSource(nameof(GetSerialPacket_EndWith_TestCases))]
        //public string GetSerialPacket_EndWith_Test(string endDelimiter, string text)
        //{
        //    ISerialPackets serialPackets = new SerialPacketPrototype()
        //        .EndFlag(endDelimiter);

        //    return serialPackets.CreateSerialPacket(text);
        //}


        //public static IEnumerable GetSerialPacket_StartWithEndWith_TestCases
        //{
        //    get
        //    {
        //        yield return new TestCaseData("!", ".", "").Returns("!.");
        //        yield return new TestCaseData("!", ".", "a").Returns("!a.");
        //    }
        //}

        //[TestCaseSource(nameof(GetSerialPacket_StartWithEndWith_TestCases))]
        //public string GetSerialPacket_StartWithEndWith_Test(string startDelimiter, string endDelimiter, string text)
        //{
        //    ISerialPackets serialPackets = new SerialPacketPrototype()
        //        .StartFlag(startDelimiter)
        //        .EndFlag(endDelimiter);

        //    return serialPackets.CreateSerialPacket(text);
        //}


    }
}
