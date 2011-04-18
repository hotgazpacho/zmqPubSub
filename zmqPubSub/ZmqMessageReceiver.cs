using System;
using System.Disposables;
using System.Text;
using Newtonsoft.Json;
using ZMQ;
using zmqPubSub.Messages;
using Exception = System.Exception;

namespace zmqPubSub
{
    /// <summary>
    /// Listens in a loop for messages coming in on a zmq SUB Socket  
    /// </summary>
    public class ZmqMessageReceiver : IReceiveMessages
    {
        readonly Context _messagingContext;
        IObserver<object> _messageObserver;

        public string ListenAddress { get; private set; }
        public Encoding MessageEncoding { get; private set; }

        public ZmqMessageReceiver(Context messagingContext, string listenAddress, Encoding messageEncoding)
        {
            _messagingContext = messagingContext;
            ListenAddress = listenAddress;
            MessageEncoding = messageEncoding;
        }

        void Start(StartListeningMessage value)
        {
            _messageObserver.OnNext(value);

            using (var incoming = _messagingContext.Socket(SocketType.SUB))
            {
                incoming.Subscribe("", MessageEncoding);
                incoming.Connect(ListenAddress);

                IsListening = true;

                while (IsListening)
                {
                    var messageBytes = incoming.Recv();
                    if (messageBytes == null) continue;
                    var jsonMessage = MessageEncoding.GetString(messageBytes);
                    var message = JsonConvert.DeserializeObject(jsonMessage);
                    _messageObserver.OnNext(message);
                }
            }
        }

        void Stop()
        {
            IsListening = false;
        }

        public IDisposable Subscribe(IObserver<object> observer)
        {
            _messageObserver = observer;
            return Disposable.Create(Stop);
        }

        public bool IsListening { get; private set; }

        public void OnNext(StartListeningMessage value)
        {
            if(!IsListening)
                Start(value);
        }

        public void OnError(Exception error)
        {
            Stop();
        }

        public void OnCompleted()
        {
            Stop();
        }
    }
}