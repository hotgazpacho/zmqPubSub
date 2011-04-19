using System;
using System.Text;
using Exception = System.Exception;

namespace zmqPubSub.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "zmq Pub Sub Server";
            const string listenAddress = "tcp://127.0.0.1:54321";
            const string publishAddress = "tcp://127.0.0.1:12345";
            var messageEncoding = Encoding.Unicode;

            IMessageBus messageBus = new MessageBus(listenAddress, publishAddress, messageEncoding);
            messageBus.Subscribe<object>(Console.WriteLine);                
            messageBus.Listen();
        }
    }
}
