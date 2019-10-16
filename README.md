# signalr-kafka-workshop
Fun with SignalR and Kafka

This is a tutorial for producing and consuming a Kafka topic, with some pre-built SignalR fun.

This tutorial is based off this [Microsoft tutorial](https://docs.microsoft.com/en-us/aspnet/core/tutorials/signalr-typescript-webpack?view=aspnetcore-2.2&tabs=visual-studio-code).
Additionally, the Kafka portions are based off of this [Confluent example](https://github.com/confluentinc/confluent-kafka-dotnet/blob/master/examples/ConfluentCloud/Program.cs)

## Prerequisites
- [Visual Studio Code](https://code.visualstudio.com/download) (or anything that can be used for dotnet development)
- [.NET Core SDK 2.2 or later](https://www.microsoft.com/net/download/all)
- [C# for Visual Studio Code version 1.17.1 or later](https://marketplace.visualstudio.com/items?itemName=ms-vscode.csharp)
- [Node.js](https://nodejs.org/) with [npm](https://www.npmjs.com/)

## Steps
### Clone this repo and test that the app works
- Build the Typescript ```npm run release```
- Run the dotnet app ```dotnet run```

You will be able to open multiple browser windows and chat locally.
![Chat](https://docs.microsoft.com/en-us/aspnet/core/tutorials/signalr-typescript-webpack/_static/browsers-message-broadcast.png?view=aspnetcore-2.2)
### Produce Kafka Messages
- Copy the KafkaProducer.cs class snippet and remove the annotations that prevent it from compiling
```
#define NEVER
#if ALWAYS
...
#endif
```
- Set the Kafka Servers, Username, Password, and Topic.
```
BootstrapServers = "<ccloud bootstrap servers>"
SaslUsername = "<ccloud key>"
SaslPassword = "<ccloud secret>"
_topic = "<Kafka topic>"
```
- Update the ChatHub constructor to include the KafkaProducer, and call produce with the username and message.
```
private readonly KafkaProducer _kafkaProducer;

public ChatHub(KafkaProducer kafkaProducer)
{
    _kafkaProducer = kafkaProducer;
}

public async Task NewMessage(string username, string message)
{
    await _kafkaProducer.Produce(username, message);
    await Clients.All.SendAsync("messageReceived", username, message);
}
```
- Uncomment the the dependency injection line that creates the KafkaProducer: ```services.AddSingleton<KafkaProducer>();```
- Messages will now be send to the Kafka topic as well.
### Consume Kafka Messages
- Copy the KafkaConsumer.cs class snippet and remove the annotations that prevent it from compiling
```
#define NEVER
#if ALWAYS
...
#endif
```
- Set the Kafka Servers, Username, Password, and Topic.
```
BootstrapServers = "<ccloud bootstrap servers>"
SaslUsername = "<ccloud key>"
SaslPassword = "<ccloud secret>"
consumer.Subscribe("<Kafka topic>");
```
- Update the ChatHub constructor to include the KafkaConsumer, this is a little bit of magic and we don't actually need to use the consumer here.
```
public ChatHub(KafkaProducer kafkaProducer, KafkaConsumer _)
```
- The SignalR SendAsync message can be remove now.
```
public async Task NewMessage(string username, string message)
{
    await _kafkaProducer.Produce(username, message);
}
```
- Uncomment the the dependency injection line that creates the KafkaConsumer: ```services.AddSingleton<KafkaConsumer>();```
- Messages will now be produced and consumed from Kafka, and you can chat with other users.
