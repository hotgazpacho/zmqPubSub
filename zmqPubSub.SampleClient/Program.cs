﻿using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using zmqPubSub.Codec;

namespace zmqPubSub.SampleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "zmq Pub Sub Sample Client";
            const string publishAddress = "tcp://127.0.0.1:54321";
            const string listenAddress = "tcp://127.0.0.1:12345";
            IMessageCodec messageCodec = new JsonMessageCodec(Encoding.Unicode);

            IMessageBus messageBus = new MessageBus(listenAddress, publishAddress, messageCodec);
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
                string message = string.Format("Message {0:d2} from Client", i);
                messageBus.Publish(message);
                i++;
            }
        }
    }
}
