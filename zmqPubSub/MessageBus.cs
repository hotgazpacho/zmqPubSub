using System;
using System.Collections.Generic;
using System.Linq;

namespace zmqPubSub
{
    /// <summary>
    /// An IMessageBus implementation using the Reactive Extensions for In-process messaging
    /// and ZeroMQ for inter-process messaging over a network 
    /// </summary>
    public class MessageBus : IMessageBus
    {
        readonly ISubject<object> _messages;
        readonly IReceiveMessages _messageReceiver;
        readonly ISendMessages _messageSender;
        IDisposable _unsubscribe;

        public MessageBus(IReceiveMessages messageReceiver, ISendMessages messageSender)
        {
            _messages = new Subject<object>();
            _messageReceiver = messageReceiver;
            _messageSender = messageSender;
        }

        public bool IsListening { get { return _messageReceiver.IsListening; } }

        public void Start()
        {
            if(!IsListening)
                _unsubscribe = _messageReceiver.Subscribe(_messages);
        }

        public void Stop()
        {
            if (IsListening && _unsubscribe != null)
                _unsubscribe.Dispose();
            _unsubscribe = null;
        }

        public void Publish<TMessage>(TMessage message)
        {
            _messageSender.SendMessage(message);
        }

        public IObservable<TMessage> GetMessages<TMessage>()
        {
            return _messages.AsObservable().OfType<TMessage>();
        }
    }
}