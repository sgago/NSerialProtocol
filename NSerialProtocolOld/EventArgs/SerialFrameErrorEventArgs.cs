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
namespace NSerialProtocol.EventArgs
{
    using System;

    /// <summary>
    /// Specifies the type of framing error.
    /// </summary>
    public enum FrameError
    {
        Frame,
        Length,
        Ecc,
    }

    /// <summary>
    /// Provides data for the FrameError event.
    /// </summary>
    public class FrameErrorReceivedEventArgs : EventArgs
    {
        /// <summary>
        ///
        /// </summary>
        private ISerialPacket serialPacket;

        /// <summary>
        /// The type of frame error.
        /// </summary>
        private FrameError frameError;

        public ISerialPacket SerialPacket
        {
            get
            {
                return serialPacket;
            }

            protected set
            {
                serialPacket = value;
            }
        }

        /// <summary>
        /// Gets the type of frame error that occured.
        /// </summary>
        public FrameError FrameError
        {
            get
            {
                return frameError;
            }

            protected set
            {
                frameError = value;
            }
        }

        /// <summary>
        /// Initializes a new
        /// </summary>
        /// <param name="serialPacket"></param>
        /// <param name="frameError"></param>
        public FrameErrorReceivedEventArgs(ISerialPacket serialPacket, FrameError frameError)
        {
            SerialPacket = serialPacket;
            FrameError = frameError;
        }
    }
}
