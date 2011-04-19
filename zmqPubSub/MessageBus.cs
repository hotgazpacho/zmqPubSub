using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZMQ;

namespace zmqPubSub
{
    /// <summary>
    /// An IMessageBus implementation using the Reactive Extensions for In-process messaging
    /// and ZeroMQ for inter-process messaging over a network 
    /// </summary>
    public class MessageBus : IMessageBus, IDisposable
    {
        readonly ISubject<object> _messages;
        readonly string _listenAddress;
        readonly string _publishAddress;
        readonly Encoding _encoding;

        readonly Context _context;
        readonly Socket _incoming;
        readonly Socket _outgoing;

        public MessageBus(string listenAddress, string publishAddress, Encoding encoding)
        {
            _listenAddress = listenAddress;
            _publishAddress = publishAddress;
            _encoding = encoding;

            _messages = new Subject<object>();
            _context = new Context(1);
            
            _outgoing = _context.Socket(SocketType.PUB);
            _outgoing.Connect(_publishAddress);
            
            _incoming = _context.Socket(SocketType.SUB);
            _incoming.Bind(_listenAddress);
            _incoming.Subscribe(string.Empty, _encoding);
        }

        public void Listen()
        {
            _messages.OnNext(new StartedListeningMessage(GetHashCode(), _listenAddress, _publishAddress, DateTime.UtcNow));

            while (true)
            {
                var messageBytes = _incoming.Recv(SendRecvOpt.NOBLOCK);
                if (messageBytes == null) continue;
                var message = _encoding.Deserialize(messageBytes);
                _messages.OnNext(message.Body);
            }
        }

        public void Publish<TMessage>(TMessage message)
        {
            _messages.OnNext(message);
            var m = message as Message ?? new Message {Body = message};
            var serialized = _encoding.Serialize(m);
            _outgoing.Send(serialized);
        }

        public IObservable<TMessage> GetMessages<TMessage>()
        {
            return _messages.AsObservable().OfType<TMessage>();
        }

        public void Dispose()
        {
            _incoming.Dispose();
            _outgoing.Dispose();
            _context.Dispose();
        }
    }
}