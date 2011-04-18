using System;
using System.ComponentModel;
using System.Disposables;
using System.Text;
using Newtonsoft.Json;
using ZMQ;

namespace zmqPubSub
{
    /// <summary>
    /// A BackgroundWorker that listens in a loop for messages 
    /// coming in on a zmq SUB Socket  
    /// </summary>
    public class ZmqMessageReceiver : BackgroundWorker, IReceiveMessages
    {
        readonly Context _messagingContext;
        IObserver<object> _messageObserver;

        public string ListenAddress { get; private set; }
        public Encoding MessageEncoding { get; private set; }

        Socket _incoming;

        protected ZmqMessageReceiver()
        {
            WorkerSupportsCancellation = true;
            WorkerReportsProgress = true;
        }

        public ZmqMessageReceiver(Context messagingContext, string listenAddress, Encoding messageEncoding) : this()
        {
            _messagingContext = messagingContext;
            ListenAddress = listenAddress;
            MessageEncoding = messageEncoding;
            _incoming = _messagingContext.Socket(SocketType.SUB);
            _incoming.Subscribe("", MessageEncoding);
            _incoming.Connect(ListenAddress);
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            base.OnDoWork(e);

            while (!CancellationPending)
            {
                var messageBytes = _incoming.Recv();
                if (messageBytes == null) continue;
                var jsonMessage = MessageEncoding.GetString(messageBytes);
                var message = JsonConvert.DeserializeObject(jsonMessage);
                ReportProgress(0, message);
            }

            _incoming.Unsubscribe("", MessageEncoding);
            e.Cancel = true;
        }

        protected override void OnProgressChanged(ProgressChangedEventArgs e)
        {
            base.OnProgressChanged(e);
            if(_messageObserver != null)
                _messageObserver.OnNext(e.UserState);
        }

        protected override void OnRunWorkerCompleted(RunWorkerCompletedEventArgs e)
        {
            base.OnRunWorkerCompleted(e);
            _messageObserver = null;
        }

        public IDisposable Subscribe(IObserver<object> observer)
        {
            _messageObserver = observer;
            RunWorkerAsync();
            return Disposable.Create(CancelAsync);
        }

        public bool IsListening
        {
            get { return IsBusy; }
        }

        protected override void Dispose(bool disposing)
        {
            _incoming.Dispose();
            base.Dispose(disposing);
        }
    }
}