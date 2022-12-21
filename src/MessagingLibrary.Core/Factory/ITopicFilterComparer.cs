namespace MessagingLibrary.Core.Factory;

public interface ITopicFilterComparer
{
    public bool IsMatch(string topic, string filter);
}