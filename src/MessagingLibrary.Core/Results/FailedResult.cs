namespace MessagingLibrary.Core.Results
{
    public class FailedResult : IErrorResult
    {
        private FailedResult(string failureReason): this(failureReason, null)
        {
        }

        private FailedResult(Exception ex) : this(string.Empty, ex)
        {
        }

        private FailedResult(string failureReason, Exception ex)
        {
            FailureReason = failureReason;
            Exception = ex;
        }
        
        public string FailureReason { get; }
        
        public Exception Exception { get; }
        
        public bool Success => false;

        public static FailedResult Create(string failureReason)
        {
            return new FailedResult(failureReason);
        }
        
        public static FailedResult Create(Exception exception)
        {
            return new FailedResult(exception);
        }

        public static FailedResult Create(string failureReason, Exception ex)
        {
            return new FailedResult(failureReason, ex);
        }
    }
}