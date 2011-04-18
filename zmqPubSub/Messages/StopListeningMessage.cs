using System;

namespace zmqPubSub.Messages
{
    public sealed class StopListeningMessage
    {
        public DateTime When { get; private set; }

        internal StopListeningMessage(DateTime when)
        {
            When = when;
        }
    }
}
