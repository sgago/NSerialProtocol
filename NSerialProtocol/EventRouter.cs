using NSerialProtocol.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;


namespace NSerialProtocol
{
    public interface IEventRouter
    {
        IList<IRoute> Routes { get; set; }

        IRoute AddRoute(ISerialProtocol serialProtocol, Type frameType, Predicate<ISerialFrame> canExecute);
        IRoute AddRoute(ISerialProtocol serialProtocol, Type frameType);

        IRoute AddRoute<TFrame>(ISerialProtocol serialProtocol, Predicate<ISerialFrame> canExecute);
        IRoute AddRoute<TFrame>(ISerialProtocol serialProtocol);
    }

    public abstract class EventRouter : IEventRouter
    {
        public ISerialProtocol Protocol { get; set; }

        public IList<IRoute> Routes { get; set; } = new List<IRoute>();

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
            foreach (IRoute route in Routes)
            {
                if (e.SerialFrame.GetType() == route.FrameType)
                {
                    route.Action?.Invoke((ISerialFrame)e.SerialFrame);
                }
            }
        }
    }



}