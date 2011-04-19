using System;

namespace zmqPubSub
{
    public class StartedListeningMessage
    {
        public int Id { get; private set; }
        public DateTime When { get; private set; }
        public string ListeningOn { get; private set; }
        public string PublishingOn { get; private set; }

        public StartedListeningMessage(int id, string listeningOn, string publishingOn, DateTime when)
        {
            Id = id;
            When = when;
            ListeningOn = listeningOn;
            PublishingOn = publishingOn;
        }

        public override string ToString()
        {
            return string.Format("zmq Pub Sub Server started at {0}{1}-- Listening on {2}{1}-- Publishing on {3}",
                                 When, Environment.NewLine, ListeningOn, PublishingOn);
        }
    }
}