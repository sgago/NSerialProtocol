﻿using System;
using System.Linq;

namespace NSerialProtocol
{
    public static class ProtocolExtensions
    {
        public static EventRouter OnFrameReceived(this SerialProtocol protocol, Type frameType)
        {
            protocol.FrameReceivedEventRouter.AddRoute(frameType);

            return protocol.FrameReceivedEventRouter;
        }

        public static EventRouter OnFrameReceived<TFrame>(this SerialProtocol protocol) where TFrame : ISerialFrame
        {
            return OnFrameReceived(protocol, typeof(TFrame));
        }

        public static EventRouter Do(this EventRouter router, Action<ISerialFrame> action)
        {
            router.Routes.Last().Action += action;

            return router;
        }

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

            router.Routes.Last().Action += ifThenAction;

            return router;
        }

        public static Action<ISerialFrame> IfElse(this Action<ISerialFrame> prevAction,
            Predicate<ISerialFrame> ifPredicate,
            Action<ISerialFrame> doAction,
            Action<ISerialFrame> elseAction)
        {
            return prevAction +=
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
        }

        public static EventRouter WriteFrame(this EventRouter router, SerialFrame serialFrame)
        {
            router.Protocol.WriteFrame(serialFrame);

            return router;
        }
    }
}
