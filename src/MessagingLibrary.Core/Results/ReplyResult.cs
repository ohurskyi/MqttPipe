using MessagingLibrary.Core.Messages;

namespace MessagingLibrary.Core.Results;

public class ReplyResult : SuccessfulResult
{
    public ReplyResult(IMessageResponse messagePayload, IResponseContext responseContext)
    {
        Payload = messagePayload;
        ResponseContext = responseContext;
    }

    public IMessageResponse Payload { get; }

    public IResponseContext ResponseContext { get; }
}