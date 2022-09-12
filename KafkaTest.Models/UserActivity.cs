using KafkaTest.Common.Models;

namespace KafkaTest.Models;

public record UserActivity(string UserId, string UserName, long Index, string Description) : IMessagePayload;
