using System.Collections.Immutable;
using Bogus;

namespace KafkaTest.Producer;

public static class StaticData
{
  public static readonly IReadOnlyList<(string UserId, string UserName)> Users =
    Enumerable.Range(0, 256).Select(_ => (Guid.NewGuid().ToString(), new Faker().Person.FullName)).ToImmutableArray();

  public static readonly IReadOnlyList<string> ActivityDescriptions = ImmutableList.Create(
    "Viewed an article.",
    "Liked an article.",
    "Shared an article.",
    "Wrote an article.",
    "Followed an author.");
}
