using System;
using System.Text;

namespace zmqPubSub.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new ZmqPubSubServer("tcp://127.0.0.1:12345", "tcp://127.0.0.1:54321", Encoding.Unicode);
            Console.CancelKeyPress += (sender, e) => server.Stop(); 
            try
            {
                server.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format("ERROR: {0}", e));
            }
        }

    }
}
