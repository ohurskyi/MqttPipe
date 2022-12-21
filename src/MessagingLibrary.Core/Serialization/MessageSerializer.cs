using MessagingLibrary.Core.Messages;
using MessagingLibrary.Core.Resolvers;
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

    public DeserializedContract Deserialize(string payload)
    {
        var serializedMessage = JsonConvert.DeserializeObject<SerializedMessage>(payload, SerializerSettings);
        var messageType = serializedMessage.MessageType;
        var type = _messageTypesResolver.ResolveContractType(messageType);
        return type == null 
            ? new DeserializedContract { Message = null, Type = messageType }
            : new DeserializedContract
            {
                Message = (IMessageContract)JsonConvert.DeserializeObject(serializedMessage.Body, type, SerializerSettings), 
                Type = messageType
            };
    }
}