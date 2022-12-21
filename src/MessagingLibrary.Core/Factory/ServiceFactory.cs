namespace MessagingLibrary.Core.Factory;

public delegate object ServiceFactory(Type handlerType);

public static class ServiceFactoryExtensions
{
    public static T GetInstance<T>(this ServiceFactory serviceFactory, Type type) => (T)serviceFactory(type);
    public static IEnumerable<T> GetInstances<T>(this ServiceFactory serviceFactory) => (IEnumerable<T>)serviceFactory(typeof(IEnumerable<T>));

}