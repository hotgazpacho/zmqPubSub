using System;
using System.Text;

namespace zmqPubSub.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new ZmqPubSubServer("tcp://localhost", "tcp://localhost:4321", Encoding.Unicode); 
            try
            {
                server.Start();
                Console.ReadLine();
                server.Stop();
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format("ERROR: {0}", e));
            }
        }
    }
}
