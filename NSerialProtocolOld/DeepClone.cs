namespace NSerialProtocol
{
    internal interface IDeepCloneable
    {
        object DeepClone();
    }

    internal interface IDeepCloneable<T> : IDeepCloneable
    {
        new T DeepClone();
    }
}
