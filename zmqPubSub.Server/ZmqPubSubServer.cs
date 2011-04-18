using System;
using ZMQ;
using System.Text;

namespace zmqPubSub.Server
{
    public class ZmqPubSubServer
    {
        public string PublishAddress { get; private set; }
        public string ListenAddress { get; private set; }
        public Encoding MessageEncoding { get; private set; }

        Context _context;

        IMessageBus _messageBus;

        public ZmqPubSubServer(string publishAddress, string listenAddress, Encoding messageEncoding)
        {
            PublishAddress = publishAddress;
            ListenAddress = listenAddress;
            MessageEncoding = messageEncoding;
        }

        public void Start()
        {
            Console.WriteLine("Starting Zmq Pub/Sub Server...");
            Console.WriteLine(string.Format("- Messages will be encoded as {0}", MessageEncoding.EncodingName));
            _context = new Context(1);

            var receiver = new ZmqMessageReceiver(_context, ListenAddress, MessageEncoding);
            var sender = new ZmqMessageSender(_context, PublishAddress, MessageEncoding);
            _messageBus = new MessageBus(receiver, sender);
            _messageBus.Start();
        }

        public void Stop()
        {
            Console.WriteLine("Zmq Pub/Sub Server Shutting down.");
            _messageBus.Stop();
            _context.Dispose();
        }
    }
}