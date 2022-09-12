using Confluent.Kafka;
using KafkaTest.Common.Models;
using KafkaTest.Common.Serialization;

namespace KafkaTest.Common.Consumers;

public class MessageConsumer : IDisposable
{
  private readonly ConsumerConfig _consumerConfig;
  private readonly MessagePayloadTypes _messagePayloadTypes;
  private IConsumer<string, IntermediateMessageEnvelope>? _consumer;

  public MessageConsumer(ConsumerConfig consumerConfig, MessagePayloadTypes messagePayloadTypes)
  {
    _consumerConfig = consumerConfig;
    _messagePayloadTypes = messagePayloadTypes;
  }

  public void Dispose()
  {
    try
    {
      _consumer?.Close();
      _consumer?.Dispose();
    }
    catch
    {
    }
  }

  public async Task ConsumeAsync(
    string topic,
    Func<MessageEnvelope, Partition, Task> handler,
    CancellationToken cancellationToken)
  {
    _consumer = CreateConsumer();

    _consumer.Subscribe(topic);

    Console.WriteLine($"Subscribed to {topic}");

    await Task.Run(
      async () =>
      {
        while (!cancellationToken.IsCancellationRequested)
        {
          try
          {
            var consumeResult = _consumer?.Consume(cancellationToken);

            if (consumeResult == null)
            {
              continue;
            }

            await handler.Invoke(
              consumeResult.Message.Value.ToMessageEnvelope(_messagePayloadTypes),
              consumeResult.Partition);
          }
          catch (OperationCanceledException)
          {
          }
        }

        _consumer?.Unsubscribe();
      },
      cancellationToken);
  }

  private IConsumer<string, IntermediateMessageEnvelope> CreateConsumer() =>
    new ConsumerBuilder<string, IntermediateMessageEnvelope>(_consumerConfig)
      .SetValueDeserializer(new KafkaJsonDeserializer())
      .Build();
}
