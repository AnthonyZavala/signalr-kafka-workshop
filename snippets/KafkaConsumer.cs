#define NEVER
#if ALWAYS

using System;
using System.Threading.Tasks;
using Confluent.Kafka;

public class KafkaConsumer
{
    public KafkaConsumer(IHubContext<ChatHub> hub)
    {
        Task messageReceived(string username, string message) => hub.Clients.All.SendAsync("messageReceived", username, message);
        
        Task.Run(async () =>
        {
            var cConfig = new ConsumerConfig
            {
                BootstrapServers = "<confluent cloud bootstrap servers>",
                BrokerVersionFallback = "0.10.0.0",
                ApiVersionFallbackMs = 0,
                SaslMechanism = SaslMechanism.Plain,
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslUsername = "<confluent cloud key>",
                SaslPassword = "<confluent cloud secret>",
                GroupId = Guid.NewGuid().ToString(),
                AutoOffsetReset = AutoOffsetReset.Latest
            };

            using (var consumer = new ConsumerBuilder<string, string>(cConfig).Build())
            {
                consumer.Subscribe("<Kafka topic>");

                while (true)
                {
                    try
                    {
                        var consumeResult = consumer.Consume();
                        await messageReceived(consumeResult.Key, consumeResult.Value);
                        Console.WriteLine($"consumed: {consumeResult.Value}");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"consume error: {e.Message}");
                    }
                }
            }
        });
    }
}

#endif
