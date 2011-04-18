using System;
using zmqPubSub.Messages;

namespace zmqPubSub
{
    public interface IReceiveMessages : IObservable<object>, IObserver<StartListeningMessage>
    {
        bool IsListening { get; }
    }
}