using System;

namespace NSerialProtocol
{
    /// <summary>
    /// Extends the SerialProtocol class with methods for routing packet events
    /// to the appropriate handler.
    /// </summary>
    public static class ProtocolExtensions
    {
        /// <summary>
        /// OnFrameReceived is called when a completed serial frame has been
        /// successfully received and parsed.
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="frameType"></param>
        /// <param name="canExecute"></param>
        /// <returns></returns>
        public static IRoute<ISerialFrame> OnFrameReceived(this ISerialProtocol protocol, Type frameType,
            Predicate<ISerialFrame> canExecute)
        {
            return protocol.FrameReceivedEventRouter.AddRoute(protocol, frameType);
        }

        /// <summary>
        /// OnFrameReceived is called when a completed serial frame has been
        /// successfully received and parsed.
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="frameType"></param>
        /// <returns></returns>
        public static IRoute<ISerialFrame> OnFrameReceived(this ISerialProtocol protocol, Type frameType)
        {
            return OnFrameReceived(protocol, frameType, canExecute => true);
        }

        /// <summary>
        /// OnFrameReceived is called when a completed serial frame has been
        /// successfully received and parsed.
        /// </summary>
        /// <typeparam name="TFrame"></typeparam>
        /// <param name="protocol"></param>
        /// <returns></returns>
        public static IRoute<ISerialFrame> OnFrameReceived<TFrame>(this ISerialProtocol protocol)
            where TFrame : ISerialFrame
        {
            return OnFrameReceived(protocol, typeof(TFrame));
        }

        /// <summary>
        /// OnFrameReceived is called when a completed serial frame has been
        /// successfully received and parsed.
        /// </summary>
        /// <typeparam name="TFrame"></typeparam>
        /// <param name="protocol"></param>
        /// <param name="canExecute"></param>
        /// <returns></returns>
        public static IRoute<ISerialFrame> OnFrameReceived<TFrame>(this ISerialProtocol protocol,
            Predicate<ISerialFrame> canExecute)
            where TFrame : ISerialFrame
        {
            return OnFrameReceived(protocol, typeof(TFrame), canExecute);
        }
    }
}
