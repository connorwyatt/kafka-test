using KafkaTest.Common.Consumers;
using KafkaTest.Models;
using Microsoft.Extensions.Hosting;

namespace KafkaTest.Consumer;

public class ConsumerHostedService : IHostedService
{
  private readonly MessageConsumer _consumer;
  private readonly ProgramOptions _programOptions;

  public ConsumerHostedService(MessageConsumer consumer, ProgramOptions programOptions)
  {
    _consumer = consumer;
    _programOptions = programOptions;
  }

  public async Task StartAsync(CancellationToken cancellationToken)
  {
    Console.Clear();
    await _consumer.ConsumeAsync(
      _programOptions.TopicName,
      (messageEnvelope, partition) =>
      {
        switch (messageEnvelope.Payload)
        {
          case UserActivity userActivity:
            Console.WriteLine(
              $"P{partition.Value.ToString().PadLeft(2)}: UserActivity ({userActivity.Index}) received for {$"{userActivity.UserName}:".PadRight(20)} \"{userActivity.Description}\".");
            break;
        }

        return Task.CompletedTask;
      },
      cancellationToken);
  }

  public Task StopAsync(CancellationToken cancellationToken)
  {
    _consumer.Dispose();
    return Task.CompletedTask;
  }
}
