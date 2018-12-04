using NSerialProtocol.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;


namespace NSerialProtocol
{
    public interface IEventRouter
    {
        //IList<IRoute> Routes { get; set; }

        IRoute Add(Type type);
    }

    public abstract class EventRouter : IEventRouter
    {
        protected IList<IRoute> Routes { get; set; } = new List<IRoute>();

        public IRoute Add(Type frameType)
        {
            Action<ISerialFrame> newAction = new Action<ISerialFrame>((sf) => { });

            Routes.Add(new Route(frameType));

            return Routes.Last();
        }
    }

    public class FrameReceivedEventRouter : EventRouter
    {
        public FrameReceivedEventRouter(ISerialProtocol protocol)
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