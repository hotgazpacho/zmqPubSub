using System;

namespace zmqPubSub
{
    public interface IReceiveMessages : IObservable<object>
    {
        bool IsListening { get; }
    }
}