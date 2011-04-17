using System;
using System.Text;

namespace zmqPubSub.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new ZmqPubSubServer("tcp://0.0.0.0:12345", "tcp://0.0.0.0:54321", Encoding.Unicode);
            server.Start();
        }
    }
}
