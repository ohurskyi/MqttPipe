using MessagingLibrary.Core.Messages;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MessagingLibrary.Core.Serialization;

public class MessageSerializer : IMessageSerializer
{
    private static readonly JsonSerializerSettings SerializerSettings = new()
    {
        ContractResolver = new CamelCasePropertyNamesContractResolver(),
        TypeNameHandling = TypeNameHandling.None
    };

    private readonly IMessageTypesResolver _messageTypesResolver;

    public MessageSerializer(IMessageTypesResolver messageTypesResolver)
    {
        _messageTypesResolver = messageTypesResolver;
    }

    public string Serialize(IMessageContract msg)
    {
        var messageWrapper = new SerializedMessage
        {
            MessageType = _messageTypesResolver.ResolveContractName(msg),
            Body = JsonConvert.SerializeObject(msg, SerializerSettings)
        };
        
        var json = JsonConvert.SerializeObject(messageWrapper, SerializerSettings);

        return json;
    }

    public (IMessageContract msg, string messageType) Deserialize(string payload)
    {
        var serializedMessage = JsonConvert.DeserializeObject<SerializedMessage>(payload, SerializerSettings);

        var type = _messageTypesResolver.ResolveContractType(serializedMessage.MessageType);

        if (type == null) return (null, serializedMessage.MessageType);
        
        var messageContract = (IMessageContract)JsonConvert.DeserializeObject(serializedMessage.Body, type, SerializerSettings);

        return (messageContract, serializedMessage.MessageType);

    }
}