using System.Text;
using Newtonsoft.Json;

namespace zmqPubSub.Codec
{
    public class JsonMessageCodec : IMessageCodec
    {
        public JsonMessageCodec(Encoding encoding)
        {
            Encoding = encoding;
        }

        public Encoding Encoding { get; private set; }
        
        public byte[] Serialize(Message message)
        {
            var json = JsonConvert.SerializeObject(message);
            var serialized = Encoding.GetBytes(json);
            return serialized;
        }

        public Message Deserialize(byte[] messageBytes)
        {
            var serialized = Encoding.GetString(messageBytes);
            Message deserialized;
            try
            {
                deserialized = JsonConvert.DeserializeObject<Message>(serialized);
            }
            catch (JsonReaderException)
            {
                deserialized = new Message { Body = serialized };
            }

            return deserialized;
        }
    }
}