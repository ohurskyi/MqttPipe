namespace MessagingLibrary.Core.Contexts;

public interface IResponseContext
{
    string ReplyTopic { get; }
    Guid CorrelationId { get; }
}