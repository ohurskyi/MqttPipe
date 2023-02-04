using System.Runtime.Serialization;
using MessagingLibrary.Core.Messages;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MessagingLibrary.Core.Serialization;

public class MessageSerializerTest : IMessageSerializer
{
    private static readonly JsonSerializerSettings _serializerSettings =  new()
    {
        ContractResolver = new CamelCasePropertyNamesContractResolver(),
        TypeNameHandling = TypeNameHandling.None
    };

    public string Serialize(IMessageContract msg)
    {
        var messageWrapper = new MessageWrapper
        {
            MessageType = msg.GetType().AssemblyQualifiedName,
            Body = JsonConvert.SerializeObject(msg, _serializerSettings)
        };
        
        var json = JsonConvert.SerializeObject(messageWrapper, _serializerSettings);

        return json;
    }

    public IMessageContract Deserialize(string payload)
    {
        var messageWrapper = JsonConvert.DeserializeObject<MessageWrapper>(payload, _serializerSettings);

        var msgType = Type.GetType(messageWrapper.MessageType);
        
        var messageContract = (IMessageContract)JsonConvert.DeserializeObject(messageWrapper.Body, msgType, _serializerSettings);

        return messageContract;
    }
}

public class MessageSerializer : IMessageSerializer
{
    private readonly JsonSerializerSettings _serializerSettings;

    public MessageSerializer(IContractsProvider contractsProvider)
    {
        _serializerSettings = new()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            TypeNameHandling = TypeNameHandling.Auto,
            SerializationBinder = new KnownMessageTypesAssemblyBinder(contractsProvider)
        };
    }

    public string Serialize(IMessageContract messageContract)
    {
        var result =  JsonConvert.SerializeObject(messageContract, typeof(IMessageContract), _serializerSettings);
        return result;
    }

    public IMessageContract Deserialize(string payload)
    {
        try
        {
            var contract = (IMessageContract)JsonConvert.DeserializeObject(payload, _serializerSettings);
            return contract;
        }
        catch (JsonSerializationException ex)
        {
            throw new SerializationException($"Message can not be deserialized. Payload: {payload}", ex);
        }
    }
}