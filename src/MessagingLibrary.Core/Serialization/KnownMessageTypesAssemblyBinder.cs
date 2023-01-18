using System.Reflection;
using MessagingLibrary.Core.Messages;
using Newtonsoft.Json.Serialization;

namespace MessagingLibrary.Core.Serialization;

public class KnownMessageTypesAssemblyBinder : ISerializationBinder
{
    private readonly IContractsProvider _contractsProvider;
    
    public KnownMessageTypesAssemblyBinder(IContractsProvider contractsProvider)
    {
        _contractsProvider = contractsProvider;
    }
    
    public Type BindToType(string assemblyName, string typeName)
    {
        return _contractsProvider.TryResolveContract(typeName, out var typeInfo) ? typeInfo : null;
    }

    public void BindToName(Type serializedType, out string assemblyName, out string typeName)
    {
        if (!_contractsProvider.IsSupportedType(serializedType))
        {
            assemblyName = null;
            typeName = null;
            return;
        }
        
        assemblyName = null;
        typeName = serializedType.Name;   
    }
}