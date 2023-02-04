namespace MessagingLibrary.Core.Messages;

public interface IMessageContract
{
    
}

public class MessageWrapper
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string MessageType { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
}