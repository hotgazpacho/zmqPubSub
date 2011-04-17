using System.ComponentModel;
using System.Text;
using ZMQ;

namespace zmqPubSub
{
    /// <summary>
    /// A BackgroundWorker that incapsulates receiving messages
    /// on a zmq SUB Scoket and sending them on a zmq PUB Socket
    /// </summary>
    public class ZmqPubSubWorker : BackgroundWorker
    {
        public string IncomingAddress { get; private set; }
        public string OutgoingAddress { get; private set; }
        public Encoding MessageEncoding { get; private set; }

        protected ZmqPubSubWorker()
        {
            WorkerSupportsCancellation = true;
            WorkerReportsProgress = true;
        }

        public ZmqPubSubWorker(string incomingAddress, string outgoingAddress, Encoding messageEncoding) : this()
        {
            IncomingAddress = incomingAddress;
            OutgoingAddress = outgoingAddress;
            MessageEncoding = messageEncoding;
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            using (var context = new Context(1))
            using (Socket incoming = context.Socket(SocketType.SUB),
                          outgoing = context.Socket(SocketType.PUB))
            {
                outgoing.Bind(OutgoingAddress);
                incoming.Subscribe("", MessageEncoding);
                incoming.Connect(IncomingAddress);

                while (!CancellationPending)
                {
                    var messageBytes = incoming.Recv();
                    if(messageBytes == null) continue;
                    outgoing.Send(messageBytes);
                    var message = MessageEncoding.GetString(messageBytes);
                    ReportProgress(0, message);
                }
            }
        }
    }
}
