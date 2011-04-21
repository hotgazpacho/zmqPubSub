using Newtonsoft.Json;

namespace zmqPubSub
{
    public class Message
    {
        [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
        public object Body { get; set; }
    }
}
