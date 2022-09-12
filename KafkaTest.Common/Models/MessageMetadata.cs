using NodaTime;

namespace KafkaTest.Common.Models;

public record MessageMetadata(Instant Timestamp);
