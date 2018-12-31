using System;

namespace NSerialProtocol.Extensions
{
    /// <summary>
    /// Extends the Route class with methods that provide straightforward methods
    /// for adding more elements to a Route's invocation list via MulticastDelegates.
    /// </summary>
    public static class RouteExtensions
    {
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

        public static IRoute IfElse(this IRoute route,
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
