using MessagingLibrary.Core.Messages;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MessagingLibrary.Core.Serialization;

public class MessageWrapper
{
    public MessageWrapper(IMessageContract contract)
    {
        Contract = contract;
    }

    public IMessageContract Contract { get; }
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

    // add wrapper class
    public string Serialize(IMessageContract messageContract)
    {
        var result =  JsonConvert.SerializeObject(messageContract, typeof(IMessageContract), _serializerSettings);
        return result;
    }

    public IMessageContract Deserialize(string payload)
    {
        return (IMessageContract)JsonConvert.DeserializeObject(payload, _serializerSettings);
    }
    
    public T Deserialize<T>(string payload) where T: IMessageContract
    {
        return JsonConvert.DeserializeObject<T>(payload, _serializerSettings);
    }
}