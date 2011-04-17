using System;
using System.ComponentModel;
using System.Text;
using Newtonsoft.Json;
using ZMQ;

namespace zmqPubSub
{
    /// <summary>
    /// A BackgroundWorker for sending messages
    /// on a zmq PUB Socket
    /// </summary>
    public class ZmqPublishingWorker : BackgroundWorker
    {
        Context MessagingContext { get; set; }
        public string PublishAddress { get; private set; }
        public Encoding MessageEncoding { get; private set; }

        protected ZmqPublishingWorker()
        {
            WorkerSupportsCancellation = false;
            WorkerReportsProgress = false;
        }

        public ZmqPublishingWorker(Context messagingContext, string publishAddress, Encoding messageEncoding)
        {
            MessagingContext = messagingContext;
            PublishAddress = publishAddress;
            MessageEncoding = messageEncoding;
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            if (e.Argument == null) throw new ArgumentNullException("Cannot publish null message.");

            var message = JsonConvert.SerializeObject(e.Argument);

            using (var outgoing = MessagingContext.Socket(SocketType.PUB))
            {
                outgoing.Connect(PublishAddress);
                outgoing.Send(message, MessageEncoding);
            }
        }
    }
}