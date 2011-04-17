using System;
using System.Concurrency;
using System.Linq;

namespace zmqPubSub
{
    public static class MessageBusExtensions
    {
        public static IDisposable Subscribe<TMessage>(this IMessageBus messageBus, Action<TMessage> onNext)
        {
            return messageBus.GetMessages<TMessage>().Subscribe(onNext);
        }

        public static IDisposable Subscribe<TMessage>(this IMessageBus messageBus, Action<TMessage> onNext, Action<Exception> onError)
        {
            return messageBus.GetMessages<TMessage>().Subscribe(onNext, onError);
        }

        public static IDisposable SubscribeOn<TMessage>(this IMessageBus messageBus, IScheduler scheduler, Action<TMessage> onNext)
        {
            return messageBus.GetMessages<TMessage>().SubscribeOn(scheduler).Subscribe(onNext);
        }

        public static IDisposable SubscribeOn<TMessage>(this IMessageBus messageBus, IScheduler scheduler, Action<TMessage> onNext, Action<Exception> onError)
        {
            return messageBus.GetMessages<TMessage>().SubscribeOn(scheduler).Subscribe(onNext, onError);
        }
    }
}