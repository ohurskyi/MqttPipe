using MessagingLibrary.Core.Messages;

namespace MessagingLibrary.Core.Results
{
    public class IntegrationEventResult : SuccessfulResult
    {
        public string Topic { get; }
        public IMessageContract Contract { get; }

        public IntegrationEventResult(IMessageContract contract, string topic)
        {
            Topic = topic;
            Contract = contract;
        }
    }
}