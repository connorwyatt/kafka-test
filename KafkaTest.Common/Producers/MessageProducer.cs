using Confluent.Kafka;
using KafkaTest.Common.Models;

namespace KafkaTest.Common.Producers;

public class MessageProducer : IDisposable
{
  private readonly IProducer<string, IntermediateMessageEnvelope> _producer;

  public MessageProducer(ProducerConfig producerConfig) => _producer = CreateProducer(producerConfig);

  public void Dispose()
  {
    _producer.Dispose();
  }

  public async Task ProduceAsync(string topic, MessageEnvelope messageEnvelope)
  {
    await _producer.ProduceAsync(
      topic,
      new Message<string, IntermediateMessageEnvelope>
      {
        Key = messageEnvelope.PartitionKey,
        Value = IntermediateMessageEnvelope.FromMessageEnvelope(messageEnvelope),
      });
  }

  private static IProducer<string, IntermediateMessageEnvelope> CreateProducer(ProducerConfig producerConfig) =>
    new ProducerBuilder<string, IntermediateMessageEnvelope>(producerConfig)
      .SetValueSerializer(new KafkaJsonSerializer())
      .Build();
}
