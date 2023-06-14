using Azure.Messaging.ServiceBus;
using AzureTestResources.AzureServiceBusTopics;
using Extensions.Logging.NUnit;

namespace AzureTestResourcesSpecification;

public class AzureServiceBusTopicsSpecification
{
  [Test]
  public async Task ShouldWHAT()
  {
    //GIVEN
    var ct = new CancellationToken();

    await using var topic = await ServiceBusTestTopic.Create(
      await File.ReadAllTextAsync("C:\\Users\\HYPERBOOK\\.secrets\\service-bus-connection-string.txt", ct),
      "test-topic",
      new NUnitLogger("servicebus"),
      ct);

    await topic.CreateSubscription("MyMessages");

    var client = new ServiceBusClient(topic.ConnectionString);

    var serviceBusSender = client.CreateSender(topic.Name);
    await serviceBusSender.SendMessageAsync(new ServiceBusMessage("lol"), ct);

    var receiver = client.CreateReceiver(topic.Name, "MyMessages");
    var receivedMessage = await receiver.ReceiveMessageAsync(cancellationToken: ct);

    Assert.AreEqual("lol", receivedMessage.Body.ToString());
  }
}