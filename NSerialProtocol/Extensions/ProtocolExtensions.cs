using System;
using System.Linq;

namespace NSerialProtocol
{
    public static class ProtocolExtensions
    {
        // TODO: OnFrameReceived should probably return a route instead
        // TODO: OnFrameReceived should probably accept a predicate - an easy check for the coder

        public static IRoute OnFrameReceived(this ISerialProtocol protocol, Type frameType)
        {
            return protocol.FrameReceivedEventRouter.AddRoute(protocol, frameType);
        }

        public static IRoute OnFrameReceived<TFrame>(this ISerialProtocol protocol)
            where TFrame : ISerialFrame
        {
            return OnFrameReceived(protocol, typeof(TFrame));
        }

        public static IRoute Do(this IRoute route, Action<ISerialFrame> action)
        {
            route.Action += action;

            return route;
        }

        public static IRoute If(this IRoute route,
            Predicate<ISerialFrame> ifPredicate,
            Action<ISerialFrame> doAction)
        {
            route.Action +=
                new Action<ISerialFrame>((sf) =>
                {
                    if (ifPredicate(sf))
                    {
                        doAction(sf);
                    }
                });

            return route;
        }

        public static IRoute IfElse(
            this IRoute route,
            Predicate<ISerialFrame> ifPredicate,
            Action<ISerialFrame> doAction,
            Action<ISerialFrame> elseAction)
        {
            route.Action +=
                new Action<ISerialFrame>((sf) =>
                {
                    if (ifPredicate(sf))
                    {
                        doAction(sf);
                    }
                    else
                    {
                        elseAction(sf);
                    }
                });

            return route;
        }

        public static IRoute WriteFrame(this IRoute route, ISerialFrame serialFrame)
        {
            route.SerialProtocol.WriteFrame(serialFrame);

            return route;
        }
    }
}
