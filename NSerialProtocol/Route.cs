using System;

namespace NSerialProtocol
{
    public interface IRoute
    {
        Type FrameType { get; set; }
        Action<ISerialFrame> Action { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Route : IRoute
    {
        public Type FrameType { get; set; }

        public Action<ISerialFrame> Action { get; set; } = default;

        public Route(Type frameType, Action<ISerialFrame> routeAction = default)
        {
            FrameType = frameType;
            Action = routeAction;
        }

        public Route(Type frameType)
            : this(frameType, default)
        {
            // Empty
        }
    }
}
