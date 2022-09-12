using System.Reflection;
using Confluent.Kafka;
using KafkaTest.Common.AdminClient;
using KafkaTest.Common.Consumers;
using KafkaTest.Common.Producers;
using KafkaTest.Common.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KafkaTest.Common;

public static class DependencyInjectionExtensions
{
  public static IServiceCollection AddKafka(this IServiceCollection services, IConfiguration configuration)
  {
    var bootstrapServers = configuration.GetValue<string>("Kafka:BootstrapServers");
    var groupId = configuration.GetValue<string>("Kafka:ConsumerGroupId");

    services.AddSingleton<KafkaAdminClient>();

    services.AddSingleton<IAdminClient>(
      _ => new AdminClientBuilder(new KeyValuePair<string, string>[] { new("bootstrap.servers", bootstrapServers), })
        .Build());

    services.AddSingleton(
      _ => new MessageProducer(
        new ProducerConfig
        {
          BootstrapServers = bootstrapServers,
        }));

    services.AddSingleton(
      serviceProvider => new MessageConsumer(
        new ConsumerConfig
        {
          BootstrapServers = bootstrapServers,
          GroupId = groupId,
          AutoOffsetReset = AutoOffsetReset.Earliest,
        },
        serviceProvider.GetRequiredService<MessagePayloadTypes>()));

    return services;
  }

  public static IServiceCollection AddKafkaMessagePayloadTypes(
    this IServiceCollection services,
    params Assembly[] assemblies)
  {
    return services.AddSingleton(_ => new MessagePayloadTypes(assemblies));
  }
}
