using System;
using System.Collections.Generic;
using System.Linq;

namespace NSerialProtocol.EventRouters
{
    /// <summary>
    /// Routes events to the appropriate handlers.
    /// </summary>
    public interface IEventRouter<T>
    {
        IRoute<T> AddRoute(ISerialProtocol serialProtocol, Type frameType,  Predicate<T> canExecute);

        IRoute<T> AddRoute(ISerialProtocol serialProtocol, Type frameType);

        IRoute<T> AddRoute<TFrame>(ISerialProtocol serialProtocol, Predicate<T> canExecute);

        IRoute<T> AddRoute<TFrame>(ISerialProtocol serialProtocol);
    }

    /// <summary>
    /// Routes events to the appropriate handlers.
    /// </summary>
    public abstract class EventRouter<T> : IEventRouter<T>
    {
        protected ISerialProtocol Protocol { get; set; }

        protected IList<IRoute<T>> Routes { get; set; } = new List<IRoute<T>>();

        /// <summary>
        /// Initializes a new instance of an EventRouter
        /// </summary>
        /// <param name="protocol"></param>
        public EventRouter(ISerialProtocol protocol)
        {
            Protocol = protocol;
        }

        public IRoute<T> AddRoute(ISerialProtocol serialProtocol, Type frameType, Predicate<T> canExecute)
        {
            Action<T> newAction = new Action<T>((sf) => { });

            Routes.Add(new Route<T>(serialProtocol, frameType));

            return Routes.Last();
        }

        public IRoute<T> AddRoute(ISerialProtocol serialProtocol, Type frameType)
        {
            return AddRoute(serialProtocol, frameType, canExecute => true);
        }

        public IRoute<T> AddRoute<TFrame>(ISerialProtocol serialProtocol, Predicate<T> canExecute)
        {
            Action<T> newAction = new Action<T> ((sf) => { });

            Routes.Add(new Route<T>(serialProtocol, typeof(TFrame)));

            return Routes.Last();
        }

        public IRoute<T> AddRoute<TFrame>(ISerialProtocol serialProtocol)
        {
            return AddRoute(serialProtocol, typeof(TFrame), canExecute => true);
        }
    }
}