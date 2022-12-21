namespace MessagingLibrary.Core.Results;

public interface IErrorResult : IExecutionResult
{
    public string FailureReason { get; }
        
    public Exception Exception { get; }
}