using MessagingLibrary.Core.Messages;

namespace MessagingLibrary.Core.Serialization;

public interface IMessageSerializer
{
    string Serialize(IMessageContract messageContract);
    (IMessageContract msg, string messageType) Deserialize(string payload);
}