using System.Text.Json;
using NodaTime;
using NodaTime.Serialization.SystemTextJson;

namespace KafkaTest.Common.Serialization;

public static class DefaultJsonSerializerOptions
{
  public static readonly JsonSerializerOptions Value =
    new JsonSerializerOptions().ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
}
