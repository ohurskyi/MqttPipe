namespace MessagingLibrary.Core.Messages;

public interface IMessage
{
    string Topic { get; set; }
    string ReplyTopic { get; set; }
    string Payload { get; set; }
    Guid CorrelationId { get; set; }
}