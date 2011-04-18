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
            using (var context = new Context(1))
            {
                var receiver = new ZmqMessageReceiver(context, ListenAddress, MessageEncoding);
                receiver.DoWork += (s, e) =>
                    {
                        Console.WriteLine(string.Format("-- Listening for messages on {0}.", ListenAddress));
                        Console.WriteLine(string.Format("-- Messages will be published on {0}.", PublishAddress));
                    };
                receiver.RunWorkerCompleted += (s, e) =>
                    {
                        Console.WriteLine(string.Format("-- Stopped Publishing messages on {0}", PublishAddress));
                        Console.WriteLine(string.Format("-- Stopped Listening for messages on {0}", ListenAddress));
                        if(e.Error != null)
                            Console.WriteLine("ERROR: {0}", e.Error);
                    };

                var sender = new ZmqMessageSender(context, PublishAddress, MessageEncoding);
                sender.DoWork += (s, e) => Console.WriteLine(string.Format("-- Publishing message on {0}{1}{2}", PublishAddress, Environment.NewLine, e.Argument));

                _messageBus = new MessageBus(receiver, sender);
                _messageBus.Start();
            }
        }

        public void Stop()
        {
            Console.WriteLine("Zmq Pub/Sub Server Shutting down.");
            _messageBus.Stop();
        }
    }
}