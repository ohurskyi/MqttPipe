namespace MessagingLibrary.Core.Messages;

public interface IResponseContext
{
    string ReplyTopic { get; set; }
    Guid CorrelationId { get; set; }
}