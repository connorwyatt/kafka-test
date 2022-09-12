using System.Text.Json;
using System.Text.Json.Nodes;
using KafkaTest.Common.Serialization;

namespace KafkaTest.Common.Models;

internal record IntermediateMessagePayload(string Type, JsonNode Value)
{
  public static IntermediateMessagePayload FromMessagePayload(IMessagePayload messagePayload)
  {
    var jsonNode = JsonSerializer.SerializeToNode(
        messagePayload,
        messagePayload.GetType(),
        DefaultJsonSerializerOptions.Value) ??
      throw new InvalidOperationException("Could not serialize payload.");

    return new IntermediateMessagePayload(messagePayload.GetType().Name, jsonNode);
  }

  public IMessagePayload ToMessagePayload(MessagePayloadTypes messagePayloadTypes)
  {
    var type = messagePayloadTypes.GetPayloadType(Type);

    var messagePayload = (IMessagePayload?)Value.Deserialize(type, DefaultJsonSerializerOptions.Value);
    return messagePayload ?? throw new InvalidOperationException("Could not deserialize payload.");
  }
}
