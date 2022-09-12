# Kafka Test

Run the `docker-compose.yml` file in the root to get **Zookeeper**, **Kafka**, and **Kafdrop** (Admin UI) running. (Kafdrop will be exposed on port 9000)

To run the producer application, run:

```shell
dotnet run --project KafkaTest.Producer/KafkaTest.Producer.csproj --Kafka:TopicName={TOPIC_NAME} --Kafka:MessagesDelay={MESSAGES_DELAY}
```

Where `TOPIC_NAME` (`string`) is the name of the topic to send messages to, and `MESSAGES_DELAY` (`int`) is the gap between messages in milliseconds.

To run the consumer application, run:

```shell
dotnet run --project KafkaTest.Consumer/KafkaTest.Consumer.csproj --Kafka:TopicName={TOPIC_NAME} --Kafka:ConsumerGroupId={CONSUMER_GROUP_ID}
```

Where `TOPIC_NAME` (`string`) is the name of the topic to receive messages from, and `CONSUMER_GROUP_ID` (`string`) is the ID of the consumer group to register the consumer in.
