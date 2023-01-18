namespace MessagingLibrary.Core.Messages;

public interface IResponseContext
{
    string ReplyTopic { get; }
    Guid CorrelationId { get; }
}