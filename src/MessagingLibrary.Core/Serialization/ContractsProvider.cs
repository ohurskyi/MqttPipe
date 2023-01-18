using System.Reflection;
using MessagingLibrary.Core.Messages;

namespace MessagingLibrary.Core.Serialization;

public interface IContractsProvider
{
    bool TryResolveContract(string typeName, out Type contract);
    bool IsSupportedType(Type contract);
}

public class ContractsProvider : IContractsProvider
{
    private readonly Dictionary<string, Type> _knownTypes;
    
    public ContractsProvider()
    {
        var contactType = typeof(IMessageContract);
        var messageTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(x => contactType.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
            .ToDictionary(k => k.Name, v => v);
        _knownTypes = new Dictionary<string, Type>(messageTypes);
    }

    public bool TryResolveContract(string typeName, out Type contract)
    {
        var resolved = _knownTypes.TryGetValue(typeName, out var type);
        contract = type;
        return resolved;
    }

    public bool IsSupportedType(Type contract)
    {
        return _knownTypes.ContainsValue(contract);
    }
}