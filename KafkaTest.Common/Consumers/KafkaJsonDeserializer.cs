using System.Text.Json;
using Confluent.Kafka;
using KafkaTest.Common.Models;
using KafkaTest.Common.Serialization;

namespace KafkaTest.Common.Consumers;

internal class KafkaJsonDeserializer
  : IDeserializer<IntermediateMessageEnvelope>
{
  public IntermediateMessageEnvelope Deserialize(
    ReadOnlySpan<byte> data,
    bool isNull,
    SerializationContext context) =>
    JsonSerializer.Deserialize<IntermediateMessageEnvelope>(data, DefaultJsonSerializerOptions.Value) ??
    throw new InvalidOperationException("Could not deserialize.");
}
