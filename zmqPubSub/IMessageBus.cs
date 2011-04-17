using System;
using System.Concurrency;

namespace zmqPubSub
{
    public interface IMessageBus
    {
        bool IsActive { get; }
        void Start();
        void Stop();
        void Publish<TMessage>(TMessage message);
        IDisposable Subscribe<TMessage>(Action<TMessage> onNext);
        IDisposable Subscribe<TMessage>(Action<TMessage> onNext, Action<Exception> onError);
        IDisposable SubscribeOn<TMessage>(IScheduler scheduler, Action<TMessage> onNext);
        IDisposable SubscribeOn<TMessage>(IScheduler scheduler, Action<TMessage> onNext, Action<Exception> onError);
    }
}