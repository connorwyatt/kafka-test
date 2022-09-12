using System.Text;
using System.Text.Json;
using Confluent.Kafka;
using KafkaTest.Common.Models;
using KafkaTest.Common.Serialization;

namespace KafkaTest.Common.Producers;

internal class KafkaJsonSerializer
  : ISerializer<IntermediateMessageEnvelope>
{
  public byte[] Serialize(IntermediateMessageEnvelope data, SerializationContext context) =>
    Encoding.UTF8.GetBytes(JsonSerializer.Serialize(data, DefaultJsonSerializerOptions.Value));
}
