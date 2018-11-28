using System;
using System.Linq;

namespace NSerialProtocol
{
    public static class ProtocolExtensions
    {
        public static EventRouter OnFrameReceived(this NSerialProtocol protocol, Type type)
        {
            protocol.FrameReceivedEventRouter.Add(type);

            return protocol.FrameReceivedEventRouter;
        }

        public static EventRouter OnFrameReceived<TFrame>(this NSerialProtocol protocol) where TFrame : ISerialFrame
        {
            return OnFrameReceived(protocol, typeof(TFrame));
        }

        //public static Action<ISerialFrame> Do(this Action<ISerialFrame> prevAction, Action<ISerialFrame> action)
        //{
        //    prevAction += action;

        //    prevAction = prevAction + action;

        //    return prevAction;
        //}

        public static EventRouter Do(this EventRouter router, Action<ISerialFrame> action)
        {
            // TODO: Create my own KVP class so I can do this:
            //router.Actions.Last().Value += action;

            // TOOO: Until then, I'm doing this ugly, ugly thing:
            router.Actions[router.Actions.Count - 1] = new System.Collections.Generic.KeyValuePair<Type, Action<ISerialFrame>>
                (
                    router.Actions[router.Actions.Count - 1].Key,
                    router.Actions[router.Actions.Count - 1].Value + action
                );

            return router;
        }

        //public static Action<ISerialFrame> If(this Action<ISerialFrame> prevAction,
        //    Predicate<ISerialFrame> ifPredicate,
        //    Action<ISerialFrame> doAction)
        //{
        //    prevAction +=
        //        new Action<ISerialFrame>((sf) =>
        //        {
        //            if (ifPredicate(sf))
        //            {
        //                doAction(sf);
        //            }
        //        });

        //    return prevAction;
        //}

        public static EventRouter If(this EventRouter router,
            Predicate<ISerialFrame> ifPredicate,
            Action<ISerialFrame> doAction)
        {
            Action<ISerialFrame> ifThenAction =
                new Action<ISerialFrame>((sf) =>
                {
                    if (ifPredicate(sf))
                    {
                        doAction(sf);
                    }
                });

            router.Actions[router.Actions.Count - 1] = new System.Collections.Generic.KeyValuePair<Type, Action<ISerialFrame>>
                (
                    router.Actions[router.Actions.Count - 1].Key,
                    router.Actions[router.Actions.Count - 1].Value + ifThenAction
                );
            return router;
        }

        public static EventRouter WriteFrame(this EventRouter router, SerialFrame serialFrame)
        {
            router.Protocol.WriteFrame(serialFrame);

            return router;
        }

        //public static Action<ISerialFrame> IfElse(this Action<ISerialFrame> prevAction,
        //    Predicate<ISerialFrame> ifPredicate,
        //    Action<ISerialFrame> doAction,
        //    Action<ISerialFrame> elseAction)
        //{
        //    return prevAction +=
        //        new Action<ISerialFrame>((sf) =>
        //        {
        //            if (ifPredicate(sf))
        //            {
        //                doAction(sf);
        //            }
        //            else
        //            {
        //                elseAction(sf);
        //            }
        //        });
        //}
    }
}
