#define NEVER
#if ALWAYS

using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace SignalRWebPack.Hubs
{
    public class ChatHub : Hub
    {
        private readonly KafkaProducer _kafkaProducer;

        public ChatHub(KafkaProducer kafkaProducer, KafkaConsumer _)
        {
            _kafkaProducer = kafkaProducer;
        }

        public async Task NewMessage(string username, string message)
        {
            await _kafkaProducer.Produce(username, message);
        }
    }
}

#endif
