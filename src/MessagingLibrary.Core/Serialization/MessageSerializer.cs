using MessagingLibrary.Core.Messages;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MessagingLibrary.Core.Serialization;

public class MessageSerializerTest : IMessageSerializer
{
    private static readonly JsonSerializerSettings SerializerSettings = new()
    {
        ContractResolver = new CamelCasePropertyNamesContractResolver(),
        TypeNameHandling = TypeNameHandling.None
    };

    private readonly IContractsProvider _contractsProvider;

    public MessageSerializerTest(IContractsProvider contractsProvider)
    {
        _contractsProvider = contractsProvider;
    }

    public string Serialize(IMessageContract msg)
    {
        var messageWrapper = new SerializedMessage
        {
            MessageType = msg.GetType().AssemblyQualifiedName,
            Body = JsonConvert.SerializeObject(msg, SerializerSettings)
        };
        
        var json = JsonConvert.SerializeObject(messageWrapper, SerializerSettings);

        return json;
    }

    public IMessageContract Deserialize(string payload)
    {
        var serializedMessage = JsonConvert.DeserializeObject<SerializedMessage>(payload, SerializerSettings);

        if (!_contractsProvider.TryResolveContract(serializedMessage.MessageType, out var type)) return null;
        
        var messageContract = (IMessageContract)JsonConvert.DeserializeObject(serializedMessage.Body, type, SerializerSettings);

        return messageContract;

    }
}