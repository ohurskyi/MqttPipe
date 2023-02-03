using MessagingLibrary.Core.Messages;

namespace MessagingLibrary.Core.Serialization;

public interface IMessageSerializer
{
    string Serialize(IMessageContract messageContract);
    IMessageContract Deserialize(string payload);
}