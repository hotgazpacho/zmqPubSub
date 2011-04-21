using System.Text;

namespace zmqPubSub.Codec
{
    public interface IMessageCodec
    {
        Encoding Encoding { get; }
        byte[] Serialize(Message message);
        Message Deserialize(byte[] messageBytes);
    }
}