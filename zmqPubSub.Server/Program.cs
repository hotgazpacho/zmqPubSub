using System;
using System.ComponentModel;
using System.Text;

namespace zmqPubSub.Server
{
    class Program
    {
        static ZmqPubSubServer _server;

        static void Main(string[] args)
        {
            Console.CancelKeyPress += (s, e) => _server.Stop();
            try
            {
                _server = new ZmqPubSubServer("tcp://127.0.0.1:12345",
                                                 "tcp://127.0.0.1:54321",
                                                 Encoding.Unicode);
                _server.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format("ERROR: {0}", e));
            }
        }
    }
}
