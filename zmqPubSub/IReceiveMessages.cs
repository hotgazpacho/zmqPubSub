using System.Collections.Generic;

namespace zmqPubSub
{
    public interface IReceiveMessages
    {
        void ListenForMessages(ISubject<object> messages);
        void StopListeningForMessages();
        bool IsListening { get; }
    }
}