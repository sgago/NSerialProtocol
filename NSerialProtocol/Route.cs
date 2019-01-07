using System;

namespace NSerialProtocol
{
    public interface IRoute<T>
    {
        ISerialProtocol SerialProtocol { get; }
        Type Type { get; set; }
        Predicate<T> CanExecute { get; set; }
        Action<T> Action { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Route<T> : IRoute<T>
    {
        public Type Type { get; set; }

        public Predicate<T> CanExecute { get; set; }

        public Action<T> Action { get; set; } = default;

        public ISerialProtocol SerialProtocol { get; }

        public Route(ISerialProtocol serialProtocol, Type type,
            Action<T> routeAction, Predicate<T> canExecute)
        {
            SerialProtocol = serialProtocol;
            Type = type;
            Action = routeAction;
            CanExecute = canExecute;
        }

        public Route(ISerialProtocol serialProtocol, Type type, Action<T> routeAction)
            : this(serialProtocol, type, routeAction, canExecute => true)
        {
            // Empty
        }

        public Route(ISerialProtocol serialProtocol, Type type)
            : this(serialProtocol, type, default, canExecute => true)
        {
            // Empty
        }
    }
}
