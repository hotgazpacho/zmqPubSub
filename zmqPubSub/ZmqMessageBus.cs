using System;
using System.Collections.Generic;
using System.Concurrency;
using System.Linq;
using System.Text;
using ZMQ;
using Exception = System.Exception;

namespace zmqPubSub
{
    /// <summary>
    /// An IMessageBus implementation using the Reactive Extensions for In-process messaging
    /// and ZeroMQ for inter-process messaging over a network 
    /// </summary>
    public class ZmqMessageBus : IMessageBus, IDisposable
    {
        readonly ISubject<object> _messages;
        readonly Context _messagingContext;
        readonly ZmqSubscriptionWorker _zmqSubscriptionWorker;
        readonly ZmqPublishingWorker _zmqPublishingWorker;

        public string IncomingAddress { get; private set; }
        public string OutgoingAddress { get; private set; }
        public Encoding MessageEncoding { get; private set; }

        public ZmqMessageBus(string incomingAddress, string outgoingAddress, Encoding messageEncoding)
        {
            IncomingAddress = incomingAddress;
            OutgoingAddress = outgoingAddress;
            MessageEncoding = messageEncoding;

            _messages = new Subject<object>();

            _messagingContext = new Context(1);

            _zmqSubscriptionWorker = new ZmqSubscriptionWorker(_messagingContext, IncomingAddress, MessageEncoding);
            _zmqSubscriptionWorker.ProgressChanged += (sender, args) => Publish(args.UserState);

            _zmqPublishingWorker = new ZmqPublishingWorker(_messagingContext, OutgoingAddress, MessageEncoding);
        }

        public bool IsActive { get { return _zmqSubscriptionWorker.IsBusy; } }

        public void Start()
        {
            if(!IsActive)
                _zmqSubscriptionWorker.RunWorkerAsync();
        }

        public void Stop()
        {
            if(IsActive)
                _zmqSubscriptionWorker.CancelAsync();
        }

        public void Publish<TMessage>(TMessage message)
        {
            _zmqPublishingWorker.RunWorkerAsync(message);
            _messages.OnNext(message);
        }

        IObservable<TMessage> GetMessage<TMessage>()
        {
            return _messages.AsObservable().OfType<TMessage>();
        }

        public IDisposable Subscribe<TMessage>(Action<TMessage> onNext)
        {
            return GetMessage<TMessage>().Subscribe(onNext);
        }

        public IDisposable Subscribe<TMessage>(Action<TMessage> onNext, Action<Exception> onError)
        {
            return GetMessage<TMessage>().Subscribe(onNext, onError);
        }

        public IDisposable SubscribeOn<TMessage>(IScheduler scheduler, Action<TMessage> onNext)
        {
            return GetMessage<TMessage>().SubscribeOn(scheduler).Subscribe(onNext);
        }

        public IDisposable SubscribeOn<TMessage>(IScheduler scheduler, Action<TMessage> onNext, Action<Exception> onError)
        {
            return GetMessage<TMessage>().SubscribeOn(scheduler).Subscribe(onNext, onError);
        }

        public void Dispose()
        {
            _messagingContext.Dispose();
        }
    }
}