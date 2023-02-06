using MessagingLibrary.Core.Messages;

namespace MessagingLibrary.Core.Serialization;

public interface IContractsProvider
{
    bool TryResolveContract(string typeName, out Type contract);
}

public class ContractsProvider : IContractsProvider
{
    private readonly Dictionary<string, Type> _supportedMessageTypes;
    
    public ContractsProvider()
    {
        var messageTypes = ScanMessageTypes();
        _supportedMessageTypes = new Dictionary<string, Type>(messageTypes);
    }

    public bool TryResolveContract(string typeName, out Type contract)
    {
        var resolved = _supportedMessageTypes.TryGetValue(typeName, out var type);
        contract = type;
        return resolved;
    }

    private static Dictionary<string, Type> ScanMessageTypes()
    {
        var contactType = typeof(IMessageContract);
        var messageTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(x => contactType.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
            .ToDictionary(k => k.AssemblyQualifiedName, v => v);
        return messageTypes;
    }
}