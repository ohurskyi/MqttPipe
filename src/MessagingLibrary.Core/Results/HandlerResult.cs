namespace MessagingLibrary.Core.Results;

public class HandlerResult
{
    public List<IExecutionResult> ExecutionResults { get; } = new();
    public bool Success => ExecutionResults.All(x => x.Success);
    public void AddResult(IExecutionResult executionResult)
    {
        ExecutionResults.Add(executionResult);
    }
}