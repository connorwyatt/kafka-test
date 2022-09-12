using KafkaTest.Common;
using KafkaTest.Models;
using KafkaTest.Producer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").AddCommandLine(args).Build();

using var host = Host.CreateDefaultBuilder(args)
  .ConfigureServices(
    services =>
    {
      services
        .Configure<HostOptions>(options => { options.ShutdownTimeout = TimeSpan.FromSeconds(30); })
        .AddSingleton(
          new ProgramOptions(
            configuration.GetValue<string>("Kafka:TopicName"),
            configuration.GetValue<int>("Kafka:MessagesDelay")))
        .AddKafka(configuration)
        .AddKafkaMessagePayloadTypes(typeof(UserActivity).Assembly)
        .AddHostedService<ProducerHostedService>();
    })
  .Build();

host.Run();
