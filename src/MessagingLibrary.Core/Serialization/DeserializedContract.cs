using MessagingLibrary.Core.Messages;

namespace MessagingLibrary.Core.Serialization;

public class DeserializedContract
{
    public IMessageContract Message { get; set; }
    public string Type { get; set; }
}