using System;

namespace zmqPubSub
{
    public interface IMessageBus
    {
        bool IsListening { get; }
        void Start();
        void Stop();
        void Publish<TMessage>(TMessage message);
        IObservable<TMessage> GetMessages<TMessage>();
    }
}