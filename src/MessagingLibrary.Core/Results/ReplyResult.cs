using MessagingLibrary.Core.Contexts;
using MessagingLibrary.Core.Messages;

namespace MessagingLibrary.Core.Results;

public class ReplyResult : SuccessfulResult
{
    public ReplyResult(IMessageResponse messageMessageResponse, IResponseContext responseContext)
    {
        MessageResponse = messageMessageResponse;
        ResponseContext = responseContext;
    }

    public IMessageResponse MessageResponse { get; }

    public IResponseContext ResponseContext { get; }
}