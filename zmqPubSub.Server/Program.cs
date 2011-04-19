using System;
using System.ComponentModel;
using System.Linq;
using System.Text;

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
            messageBus.GetMessages<StartedListeningMessage>()
                .Where(m => m.Id == messageBus.GetHashCode())
                .Subscribe(x => Console.WriteLine("Press ESC to cancel, or any other key to send a message."));
            var worker = new BackgroundWorker();
            worker.DoWork += (s, e) => messageBus.Listen();
            worker.RunWorkerAsync();

            bool loop = true;
            int i = 1;
            while (loop)
            {
                var key = Console.ReadKey(false);
                if (key.Key == ConsoleKey.Escape)
                {
                    loop = false;
                    continue;
                }
                string message = string.Format("Message {0:d2} from Server", i);
                messageBus.Publish(message);
                i++;
            }
        }
    }
}
