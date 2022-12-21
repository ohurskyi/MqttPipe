namespace MessagingLibrary.Processing.Listeners;

public interface IConsumerListener
{
    Task StartListening();
    Task StopListening();
}