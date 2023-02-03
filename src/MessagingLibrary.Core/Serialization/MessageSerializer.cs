using System.Runtime.Serialization;
using MessagingLibrary.Core.Messages;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MessagingLibrary.Core.Serialization;

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