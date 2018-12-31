using NSerialProtocol.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NSerialProtocol
{
    /// <summary>
    /// Routes events to the appropriate handlers.
    /// </summary>
    public interface IEventRouter
    {
        IRoute AddRoute(ISerialProtocol serialProtocol, Type frameType,
            Predicate<ISerialFrame> canExecute);
        IRoute AddRoute(ISerialProtocol serialProtocol, Type frameType);

        IRoute AddRoute<TFrame>(ISerialProtocol serialProtocol,
            Predicate<ISerialFrame> canExecute);
        IRoute AddRoute<TFrame>(ISerialProtocol serialProtocol);
    }

    /// <summary>
    /// Routes events to the appropriate handlers.
    /// </summary>
    public abstract class EventRouter : IEventRouter
    {
        protected ISerialProtocol Protocol { get; set; }

        protected IList<IRoute> Routes { get; set; } = new List<IRoute>();

        /// <summary>
        /// Initializes a new instance of an EventRouter
        /// </summary>
        /// <param name="protocol"></param>
        public EventRouter(ISerialProtocol protocol)
        {
            Protocol = protocol;
        }

        public IRoute AddRoute(ISerialProtocol serialProtocol, Type frameType,
            Predicate<ISerialFrame> canExecute)
        {
            Action<ISerialFrame> newAction = new Action<ISerialFrame>((sf) => { });

            Routes.Add(new Route(serialProtocol, frameType));

            return Routes.Last();
        }

        public IRoute AddRoute(ISerialProtocol serialProtocol, Type frameType)
        {
            return AddRoute(serialProtocol, frameType, canExecute => true);
        }

        public IRoute AddRoute<TFrame>(ISerialProtocol serialProtocol, Predicate<ISerialFrame> canExecute)
        {
            Action<ISerialFrame> newAction = new Action<ISerialFrame>((sf) => { });

            Routes.Add(new Route(serialProtocol, typeof(TFrame)));

            return Routes.Last();
        }

        public IRoute AddRoute<TFrame>(ISerialProtocol serialProtocol)
        {
            return AddRoute(serialProtocol, typeof(TFrame), canExecute => true);
        }
    }

    public class FrameReceivedEventRouter : EventRouter
    {
        public FrameReceivedEventRouter(ISerialProtocol protocol)
            : base(protocol)
        {
            protocol.OnFrameReceived += Protocol_SerialFrameReceived;
        }

        private void Protocol_SerialFrameReceived(object sender, SerialFrameReceivedEventArgs e)
        {
            ISerialFrame receivedFrame;

            foreach (IRoute route in Routes)
            {
                if (e.SerialFrame.GetType() == route.FrameType)
                {
                    receivedFrame = e.SerialFrame as ISerialFrame;

                    if (route.CanExecute(receivedFrame))
                    {
                        route.Action?.Invoke(receivedFrame);
                    }
                }
            }
        }
    }



}