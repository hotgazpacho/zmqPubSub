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

        public ZmqPubSubServer(string publishAddress, string listenAddress, Encoding messageEncoding)
        {
            PublishAddress = publishAddress;
            ListenAddress = listenAddress;
            MessageEncoding = messageEncoding;
        }

        public void Start()
        {
            Console.WriteLine("Starting Zmq Pub/Sub Server...");
            using (var context = new Context(1))
            {
                var receiver = new ZmqMessageReceiver(context, ListenAddress, MessageEncoding);
                var sender = new ZmqMessageSender(context, PublishAddress, MessageEncoding);
                var messageBus = new MessageBus(receiver, sender);
                Console.WriteLine(string.Format("- Messages will be encoded as {0}", MessageEncoding.EncodingName));
                Console.WriteLine(string.Format("-- Listening for messages on {0}", ListenAddress));
                Console.WriteLine(string.Format("-- Publishing messages on {0}", PublishAddress));
                messageBus.Start();
                while (messageBus.IsListening)
                {
                    continue;
                }
            }
            Console.WriteLine("Zmq Pub/Sub Server Shutting down.");
        }
    }
}