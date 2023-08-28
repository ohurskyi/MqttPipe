using System.Collections.Concurrent;
using MessagingLibrary.Core.Messages;

namespace MessagingLibrary.Core.Resolvers;

public class MessageTypesResolver : IMessageTypesResolver
{
    private readonly ConcurrentDictionary<string, Type> _supportedMessageTypes;
    
    public MessageTypesResolver()
    {
        var messageTypes = ScanMessageTypes();
        _supportedMessageTypes = new ConcurrentDictionary<string, Type>(messageTypes);
    }

    public Type ResolveContractType(string typeName)
    {
        return _supportedMessageTypes.GetValueOrDefault(typeName);
    }

    public string ResolveContractName(IMessageContract messageContract)
    {
        return GenerateMessageTypeKey(messageContract.GetType());
    }

    private static string GenerateMessageTypeKey(Type messageContractType) =>
        messageContractType.AssemblyQualifiedName;

    private static Dictionary<string, Type> ScanMessageTypes()
    {
        var contactType = typeof(IMessageContract);
        var messageTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(x => contactType.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
            .ToDictionary(GenerateMessageTypeKey, v => v);
        return messageTypes;
    }
}