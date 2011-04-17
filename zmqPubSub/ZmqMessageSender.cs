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
    public class ZmqMessageSender : BackgroundWorker, ISendMessages
    {
        Context _messagingContext;

        public string PublishAddress { get; private set; }
        public Encoding MessageEncoding { get; private set; }

        protected ZmqMessageSender()
        {
            WorkerSupportsCancellation = false;
            WorkerReportsProgress = false;
        }

        public ZmqMessageSender(Context messagingContext, string publishAddress, Encoding messageEncoding)
        {
            _messagingContext = messagingContext;
            PublishAddress = publishAddress;
            MessageEncoding = messageEncoding;
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            SendMessage(e.Argument);
        }

        public void SendMessage(object message)
        {
            if (message == null) throw new ArgumentNullException("Cannot publish null message.");

            var serializedMessage = JsonConvert.SerializeObject(message);

            using (var outgoing = _messagingContext.Socket(SocketType.PUB))
            {
                outgoing.Connect(PublishAddress);
                outgoing.Send(serializedMessage, MessageEncoding);
            }
        }
    }
}