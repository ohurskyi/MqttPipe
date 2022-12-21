using MessagingLibrary.Core.Messages;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MessagingLibrary.Core.Extensions
{
    public static class MessageExtensions
    {
        public static string MessagePayloadToJson<T>(this T messagePayload) where T: IMessageContract
        {
            return JsonConvert.SerializeObject(messagePayload, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }
        
        public static T MessagePayloadFromJson<T>(this string messagePayload) where T: IMessageContract
        {
            return JsonConvert.DeserializeObject<T>(messagePayload, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }
    }
}