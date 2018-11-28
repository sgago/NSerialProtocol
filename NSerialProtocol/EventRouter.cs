using System;
using System.Collections.Generic;

namespace NSerialProtocol
{
    using FrameActionKvp = KeyValuePair<Type, Action<ISerialFrame>>;

    public class EventRouter
    {
        public NSerialProtocol Protocol { get; set; }
        public List<FrameActionKvp> Actions { get; set; } = new List<FrameActionKvp>();

        public EventRouter(NSerialProtocol protocol)
        {
            Protocol = protocol;
            protocol.SerialFrameReceived += Protocol_SerialFrameReceived;
        }

        private void Protocol_SerialFrameReceived(object sender, EventArgs.SerialFrameReceivedEventArgs e)
        {
            foreach (FrameActionKvp frameActionKvp in Actions)
            {
                if (e.SerialFrame.GetType() == frameActionKvp.Key)
                {
                    frameActionKvp.Value?.Invoke((ISerialFrame)e.SerialFrame);
                }
            }
        }

        public Action<ISerialFrame> Add(Type type)
        {
            Action<ISerialFrame> newAction = new Action<ISerialFrame>((sf) => { });

            Actions.Add(new FrameActionKvp(type, newAction));

            return Actions[Actions.Count - 1].Value;
        }
    }
}
