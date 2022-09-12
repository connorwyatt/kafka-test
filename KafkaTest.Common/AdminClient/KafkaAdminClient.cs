using Confluent.Kafka;
using Confluent.Kafka.Admin;

namespace KafkaTest.Common.AdminClient;

public class KafkaAdminClient
{
  private readonly IAdminClient _adminClient;

  public KafkaAdminClient(IAdminClient adminClient) => _adminClient = adminClient;

  public async Task EnsureTopicExists(string topicName, int partitions)
  {
    var topicMetadata = await GetTopicMetadata(topicName);

    if (topicMetadata != null)
    {
      if (topicMetadata.Partitions.Count != partitions)
      {
        throw new InvalidOperationException("Inconsistent number of partitions.");
      }

      return;
    }

    await CreateTopic(topicName, partitions);
  }

  public Task CreateTopic(string topicName, int partitions)
  {
    return _adminClient.CreateTopicsAsync(
      new[]
      {
        new TopicSpecification
        {
          Name = topicName,
          NumPartitions = partitions,
          Configs = new Dictionary<string, string>
          {
            { "retention.ms", "-1" },
          },
        },
      });
  }

  private Task<Metadata> GetMetadataAsync() => Task.Run(() => _adminClient.GetMetadata(TimeSpan.FromSeconds(30)));

  private async Task<TopicMetadata?> GetTopicMetadata(string topicName)
  {
    var metadata = await GetMetadataAsync();

    return metadata.Topics.SingleOrDefault(t => t.Topic == topicName);
  }
}
