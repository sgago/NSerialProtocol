using NSerialProtocol.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;


namespace NSerialProtocol
{
    public interface IEventRouter
    {
        IList<IRoute> Routes { get; set; }

        IRoute AddRoute(Type frameType);
    }

    public abstract class EventRouter : IEventRouter
    {
        public ISerialProtocol Protocol { get; set; }

        public IList<IRoute> Routes { get; set; } = new List<IRoute>();

        public EventRouter(ISerialProtocol protocol)
        {
            Protocol = protocol;
        }

        public IRoute AddRoute(Type frameType)
        {
            Action<ISerialFrame> newAction = new Action<ISerialFrame>((sf) => { });

            Routes.Add(new Route(frameType));

            return Routes.Last();
        }

        public IRoute AddRoute<TFrame>()
        {
            return AddRoute(typeof(TFrame));
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