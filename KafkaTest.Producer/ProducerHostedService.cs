using KafkaTest.Common.AdminClient;
using KafkaTest.Common.Models;
using KafkaTest.Common.Producers;
using KafkaTest.Models;
using Microsoft.Extensions.Hosting;

namespace KafkaTest.Producer;

public class ProducerHostedService : IHostedService
{
  private readonly KafkaAdminClient _kafkaAdminClient;
  private readonly MessageProducer _producer;
  private readonly ProgramOptions _programOptions;
  private readonly Random _random = new();

  public ProducerHostedService(
    KafkaAdminClient kafkaAdminClient,
    MessageProducer producer,
    ProgramOptions programOptions)
  {
    _kafkaAdminClient = kafkaAdminClient;
    _producer = producer;
    _programOptions = programOptions;
  }

  public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

  public async Task StartAsync(CancellationToken cancellationToken)
  {
    await _kafkaAdminClient.EnsureTopicExists(_programOptions.TopicName, 64);

    var i = 0L;

    Console.Clear();
    while (!cancellationToken.IsCancellationRequested)
    {
      var (userId, userName) = GetRandomUserData();

      var userActivity = new UserActivity(userId, userName, i, GetRandomActivityDescription());
      var message = MessageEnvelope.Create(userId, userActivity);

      await _producer.ProduceAsync(_programOptions.TopicName, message);
      Console.WriteLine(
        $"UserActivity sent for {$"{userActivity.UserName}:".PadRight(20)} \"{userActivity.Description}\".");

      i++;

      await Task.Delay(_programOptions.MessagesDelay, CancellationToken.None);
    }
  }

  private (string UserId, string UserName) GetRandomUserData() =>
    StaticData.Users[_random.Next(StaticData.Users.Count)];

  private string GetRandomActivityDescription() =>
    StaticData.ActivityDescriptions[_random.Next(StaticData.ActivityDescriptions.Count)];
}
