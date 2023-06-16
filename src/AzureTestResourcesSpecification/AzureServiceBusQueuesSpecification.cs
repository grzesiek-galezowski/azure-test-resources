using Azure.Messaging.ServiceBus;
using AzureTestResources.AzureServiceBus;
using Extensions.Logging.NUnit;

namespace AzureTestResourcesSpecification;

public class AzureServiceBusQueuesSpecification
{
  [TestCase(1)]
  [TestCase(2)]
  [TestCase(3)]
  [TestCase(4)]
  [TestCase(5)]
  [TestCase(6)]
  [TestCase(7)]
  [TestCase(8)]
  [TestCase(9)]
  [TestCase(10)]
  [TestCase(11)]
  [TestCase(12)]
  [TestCase(13)]
  [TestCase(14)]
  [TestCase(15)]
  [TestCase(16)]
  [TestCase(21)]
  [TestCase(22)]
  [TestCase(23)]
  [TestCase(24)]
  [TestCase(25)]
  [TestCase(26)]
  [TestCase(27)]
  [TestCase(28)]
  [TestCase(29)]
  [TestCase(30)]
  [TestCase(31)]
  [TestCase(32)]
  [TestCase(33)]
  [TestCase(34)]
  [TestCase(35)]
  [TestCase(36)]
  public async Task ShouldCreateAzureServiceBusQueue(int testNo)
  {
    //GIVEN
    var ct = new CancellationToken();

    await using var queue = await ServiceBusTestResources.CreateQueue(
      await File.ReadAllTextAsync("C:\\Users\\HYPERBOOK\\.secrets\\service-bus-connection-string.txt", ct),
      "testqueue",
      new NUnitLogger("servicebus"),
      ct);

    var client = new ServiceBusClient(queue.ConnectionString);

    var serviceBusSender = client.CreateSender(queue.Name);
    await serviceBusSender.SendMessageAsync(new ServiceBusMessage("lol"), ct);

    var receiver = client.CreateReceiver(queue.Name);
    var receivedMessage = await receiver.ReceiveMessageAsync(cancellationToken: ct);

    Assert.AreEqual("lol", receivedMessage.Body.ToString());
  }
}