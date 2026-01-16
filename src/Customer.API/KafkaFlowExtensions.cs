using KafkaFlow;
using KafkaFlow.Serializer;

namespace Customer.API;

public static class KafkaFlowExtensions
{
    public static void ConfigureKafkaFlow(this IServiceCollection services, IConfiguration configuration)
    {
        var kafkaConfig = configuration.GetSection("Kafka");
        var bootstrapServers = kafkaConfig.GetValue<string>("BootstrapServers") ?? "localhost:9092";
        var topic = kafkaConfig.GetValue<string>("Topic") ?? "customer-events";

        services.AddKafka(kafka => kafka
            .AddCluster(cluster => cluster
                .WithBrokers(new[] { bootstrapServers })
                .CreateTopicIfNotExists(topic, 1, 1)
                .AddProducer("customer-events", producer => producer
                    .DefaultTopic(topic)
                    .AddMiddlewares(middlewares => middlewares
                        .AddSerializer<JsonCoreSerializer>()
                    )
                )
            )
        );
    }
}
