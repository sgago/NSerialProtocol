using System;

namespace NSerialProtocol.Extensions
{
    /// <summary>
    /// Extends the Route class with straightforward methods that provide
    /// adding more elements to a Route's invocation list via MulticastDelegates.
    /// </summary>
    public static class RouteExtensions
    {
        /// <summary>
        /// Performs a given action when this route's delegates are invoked.
        /// </summary>
        /// <param name="route">The route that will be invoked.</param>
        /// <param name="action">The action or callback to perform when this route is invoked.</param>
        /// <returns></returns>
        public static IRoute<T> Do<T>(this IRoute<T> route, Action<T> action)
        {
            route.Action += action;

            return route;
        }

        public static IRoute<string> Do(this IRoute<string> route, Action<string> action)
        {
            return Do<string>(route, action);
        }

        public static IRoute<ISerialFrame> Do(this IRoute<ISerialFrame> route, Action<ISerialFrame> action)
        {
            return Do<ISerialFrame>(route, action);
        }

        public static IRoute<ISerialPacket> Do(this IRoute<ISerialPacket> route, Action<ISerialPacket> action)
        {
            return Do<ISerialPacket>(route, action);
        }

        public static IRoute<T> If<T>(this IRoute<T> route,
            Predicate<T> ifPredicate,
            Action<T> doAction)
        {
            route.Action +=
                new Action<T>((sf) =>
                {
                    if (ifPredicate(sf))
                    {
                        doAction(sf);
                    }
                });

            return route;
        }

        public static IRoute<T> IfElse<T>(this IRoute<T> route,
            Predicate<T> ifPredicate,
            Action<T> doAction,
            Action<T> elseAction)
        {
            route.Action +=
                new Action<T>((sf) =>
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

        public static IRoute<T> WriteFrame<T>(this IRoute<T> route, ISerialFrame serialFrame)
        {
            route.SerialProtocol.WriteFrame(serialFrame);

            return route;
        }
    }
}
