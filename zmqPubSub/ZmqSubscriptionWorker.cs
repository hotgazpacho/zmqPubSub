using System;
using System.ComponentModel;
using System.Text;
using Newtonsoft.Json;
using ZMQ;

namespace zmqPubSub
{
    /// <summary>
    /// A BackgroundWorker that listens in a loop for messages 
    /// coming in on a zmq SUB Socket  
    /// </summary>
    public class ZmqSubscriptionWorker : BackgroundWorker
    {
        Context MessagingContext { get; set; }
        public string ListenAddress { get; private set; }
        public Encoding MessageEncoding { get; private set; }

        protected ZmqSubscriptionWorker()
        {
            WorkerSupportsCancellation = true;
            WorkerReportsProgress = true;
        }

        public ZmqSubscriptionWorker(Context messagingContext, string listenAddress, Encoding messageEncoding) : this()
        {
            MessagingContext = messagingContext;
            ListenAddress = listenAddress;
            MessageEncoding = messageEncoding;
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            var context = e.Argument as Context;
            if (context == null) throw new ArgumentException("ZMQ Context required as the value for the Argument property of the DoWorkEventArgs", "e");

            using (var incoming = context.Socket(SocketType.SUB))
            {
                incoming.Subscribe("", MessageEncoding);
                incoming.Connect(ListenAddress);

                while (!CancellationPending)
                {
                    var messageBytes = incoming.Recv();
                    if(messageBytes == null) continue;
                    var jsonMessage = MessageEncoding.GetString(messageBytes);
                    var message = JsonConvert.DeserializeObject(jsonMessage);
                    ReportProgress(0, message);
                }

                incoming.Unsubscribe("", MessageEncoding);
            }
        }
    }
}