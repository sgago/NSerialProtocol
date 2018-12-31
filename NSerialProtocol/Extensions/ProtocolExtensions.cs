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
        /// received and parsed.
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="frameType"></param>
        /// <param name="canExecute"></param>
        /// <returns></returns>
        public static IRoute OnFrameReceived(this ISerialProtocol protocol, Type frameType,
            Predicate<ISerialFrame> canExecute)
        {
            return protocol.FrameReceivedEventRouter.AddRoute(protocol, frameType);
        }

        public static IRoute OnFrameReceived(this ISerialProtocol protocol, Type frameType)
        {
            return OnFrameReceived(protocol, frameType, canExecute => true);
        }

        public static IRoute OnFrameReceived<TFrame>(this ISerialProtocol protocol)
            where TFrame : ISerialFrame
        {
            return OnFrameReceived(protocol, typeof(TFrame));
        }

        public static IRoute OnFrameReceived<TFrame>(this ISerialProtocol protocol,
            Predicate<ISerialFrame> canExecute)
            where TFrame : ISerialFrame
        {
            return OnFrameReceived(protocol, typeof(TFrame), canExecute);
        }
    }
}
