using Newtonsoft.Json;

namespace zmqPubSub
{
    internal class Message
    {
        [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
        public object Body { get; set; }
    }
}
