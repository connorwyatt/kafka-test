using NodaTime;

namespace KafkaTest.Common.Models;

public record MessageEnvelope(
  string Id,
  string PartitionKey,
  IMessagePayload Payload,
  MessageMetadata Metadata)
{
  public static MessageEnvelope Create(string partitionKey, IMessagePayload payload) =>
    new(
      Guid.NewGuid().ToString(),
      partitionKey,
      payload,
      new MessageMetadata(SystemClock.Instance.GetCurrentInstant()));
}
