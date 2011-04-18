using System;
using System.ComponentModel;
using System.Text;
using Newtonsoft.Json;
using ZMQ;

namespace zmqPubSub
{
    /// <summary>
    /// A BackgroundWorker for sending messages
    /// on a zmq PUB Socket
    /// </summary>
    public class ZmqMessageSender : ISendMessages, IDisposable
    {
        readonly Context _messagingContext;
        readonly Socket _outgoing;

        public string PublishAddress { get; private set; }
        public Encoding MessageEncoding { get; private set; }

        public ZmqMessageSender(Context messagingContext, string publishAddress, Encoding messageEncoding)
        {
            _messagingContext = messagingContext;
            PublishAddress = publishAddress;
            MessageEncoding = messageEncoding;
            _outgoing = _messagingContext.Socket(SocketType.PUB);
            _outgoing.Connect(PublishAddress);
        }

        public void SendMessage(object message)
        {
            if (message == null) throw new ArgumentNullException("Cannot publish null message.");

            var serializedMessage = JsonConvert.SerializeObject(message);
            
            _outgoing.Send(serializedMessage, MessageEncoding);
        }

        public void Dispose()
        {
            _outgoing.Dispose();
        }
    }
}