using MessagingLibrary.Core.Messages;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MessagingLibrary.Core.Serialization;

public interface IMessageSerializer
{
    string Serialize(IMessageContract messageContract);
    
    IMessageContract Deserialize(string payload);
}

public class MessageSerializer : IMessageSerializer
{
    private static readonly JsonSerializerSettings _serializerSettings = new()
    {
        ContractResolver = new CamelCasePropertyNamesContractResolver(),
        TypeNameHandling = TypeNameHandling.Objects
    };

    public string Serialize(IMessageContract messageContract)
    {
        return JsonConvert.SerializeObject(messageContract, messageContract.GetType(), _serializerSettings);
    }

    public IMessageContract Deserialize(string payload)
    {
        return (IMessageContract)JsonConvert.DeserializeObject(payload, _serializerSettings);
    }
}