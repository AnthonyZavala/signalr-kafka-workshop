#define NEVER
#if ALWAYS

using System.Threading.Tasks;
using Confluent.Kafka;

public class KafkaProducer
{
    private readonly IProducer<string, string> _producer;
    private readonly string _topic;

    public KafkaProducer()
    {
        var pConfig = new ProducerConfig
        {
            BootstrapServers = "<ccloud bootstrap servers>",
            BrokerVersionFallback = "0.10.0.0",
            ApiVersionFallbackMs = 0,
            SaslMechanism = SaslMechanism.Plain,
            SecurityProtocol = SecurityProtocol.SaslSsl,
            // Note: If your root CA certificates are in an unusual location you
            // may need to specify this using the SslCaLocation property.
            SaslUsername = "<ccloud key>",
            SaslPassword = "<ccloud secret>"
        };

        _producer = new ProducerBuilder<string, string>(pConfig).Build();
        _topic = "<Kafka topic>";
    }

    public async Task Produce(string username, string message)
    {
        await _producer.ProduceAsync(_topic, new Message<string, string> { Key = username, Value = message });
    }
}

#endif
