using System;

namespace zmqPubSub
{
    public interface IReceiveMessages
    {
        void ListenForMessages(IObserver<object> messageObserver);
        void StopListeningForMessages();
        bool IsListening { get; }
    }
}