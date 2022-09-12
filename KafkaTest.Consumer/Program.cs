using KafkaTest.Common;
using KafkaTest.Consumer;
using KafkaTest.Models;
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
        .AddSingleton(new ProgramOptions(configuration["Kafka:TopicName"]))
        .AddKafka(configuration)
        .AddKafkaMessagePayloadTypes(typeof(UserActivity).Assembly)
        .AddHostedService<ConsumerHostedService>();
    })
  .Build();

host.Run();
