using MessagingLibrary.Core.Factory;
using MQTTnet;

namespace MqttPipe;

public class MqttTopicComparer : ITopicFilterComparer
{
    public bool IsMatch(string topic, string filter)
    {
        return MqttTopicFilterComparer.Compare(topic, filter) == MqttTopicFilterCompareResult.IsMatch;
    }
}