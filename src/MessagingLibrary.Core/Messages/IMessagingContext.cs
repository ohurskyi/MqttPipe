namespace MessagingLibrary.Core.Messages;

public interface IMessagingContext<T> : IResponseContext
    where T: IMessageContract
{
    string Topic { get; set; }
    T Payload { get; set; }
}