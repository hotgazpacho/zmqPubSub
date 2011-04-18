using System;

namespace zmqPubSub.Messages
{
    public sealed class StartListeningMessage
    {
        public DateTime When { get; private set; }

        internal StartListeningMessage(DateTime when)
        {
            When = when;
        }
    }
}