using System.Text;
using Newtonsoft.Json;

namespace zmqPubSub
{
    internal static class EncodingExtensions
    {
        internal static Message Deserialize(this Encoding encoding, byte[] messageBytes)
        {
            var serialized = encoding.GetString(messageBytes);
            Message deserialized;
            try
            {
                deserialized = JsonConvert.DeserializeObject<Message>(serialized);
            }
            catch (JsonReaderException)
            {
                deserialized = new Message {Body = serialized};
            }
            
            return deserialized;
        }

        internal static byte[] Serialize(this Encoding encoding, Message message)
        {
            var json = JsonConvert.SerializeObject(message);
            var serialized = encoding.GetBytes(json);
            return serialized;
        }
    }
}
