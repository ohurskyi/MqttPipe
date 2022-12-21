namespace MessagingLibrary.Core.Messages;

public class Message: IMessage
{
    public string Topic { get; set; }
    public string ReplyTopic { get; set; }
    public string Payload { get; set; }
    public Guid CorrelationId { get; set; }
}