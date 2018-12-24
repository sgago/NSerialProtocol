using System;

namespace NSerialProtocol
{
    public interface IRoute
    {
        ISerialProtocol SerialProtocol { get; }
        Type FrameType { get; set; }
        Predicate<ISerialFrame> CanExecute { get; set; }
        Action<ISerialFrame> Action { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Route : IRoute
    {
        public Type FrameType { get; set; }

        public Predicate<ISerialFrame> CanExecute { get; set; }

        public Action<ISerialFrame> Action { get; set; } = default;

        public ISerialProtocol SerialProtocol { get; }

        public Route(ISerialProtocol serialProtocol, Type frameType,
            Action<ISerialFrame> routeAction, Predicate<ISerialFrame> canExecute)
        {
            SerialProtocol = serialProtocol;
            FrameType = frameType;
            Action = routeAction;
            CanExecute = canExecute;
        }

        public Route(ISerialProtocol serialProtocol, Type frameType, Action<ISerialFrame> routeAction)
            : this(serialProtocol, frameType, routeAction, canExecute => true)
        {
            // Empty
        }

        public Route(ISerialProtocol serialProtocol, Type frameType)
            : this(serialProtocol, frameType, default, canExecute => true)
        {
            // Empty
        }
    }
}
