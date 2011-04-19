using System;
using System.ComponentModel;

namespace zmqPubSub
{
    public interface IMessageBus
    {
        void Publish<TMessage>(TMessage message);
        IObservable<TMessage> GetMessages<TMessage>();
        void Listen();
    }

}