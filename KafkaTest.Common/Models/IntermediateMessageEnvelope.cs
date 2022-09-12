using KafkaTest.Common.Serialization;

namespace KafkaTest.Common.Models;

internal record IntermediateMessageEnvelope(
  string Id,
  string PartitionKey,
  IntermediateMessagePayload Payload,
  MessageMetadata Metadata)
{
  public static IntermediateMessageEnvelope FromMessageEnvelope(MessageEnvelope messageEnvelope) =>
    new(
      messageEnvelope.Id,
      messageEnvelope.PartitionKey,
      IntermediateMessagePayload.FromMessagePayload(messageEnvelope.Payload),
      messageEnvelope.Metadata);

  public MessageEnvelope ToMessageEnvelope(MessagePayloadTypes messagePayloadTypes) =>
    new(Id, PartitionKey, Payload.ToMessagePayload(messagePayloadTypes), Metadata);
}
