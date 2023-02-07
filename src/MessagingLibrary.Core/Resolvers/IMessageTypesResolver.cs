using MessagingLibrary.Core.Messages;

namespace MessagingLibrary.Core.Resolvers;

public interface IMessageTypesResolver
{
    Type ResolveContractType(string typeName);
    string ResolveContractName(IMessageContract messageContract);
}