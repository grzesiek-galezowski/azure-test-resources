using Azure.Messaging.ServiceBus;
using Extensions.Logging.NUnit;
using FluentAssertions;
using TddXt.AzureTestResources.Messaging.ServiceBus;
using Testcontainers.ServiceBus;

namespace TddXt.AzureTestResourcesSpecification;

public class AzureServiceBusQueuesSpecification
{
  // will not work yet, because the emulator does not support admin sdk
  private ServiceBusContainer _container;

  [OneTimeSetUp]
  public async Task SetUpEmulator()
  {
    _container = await DockerContainersForTests.StartServiceBusContainer();
  }

  [OneTimeTearDown]
  public async Task TearDownEmulator()
  {
    await _container.DisposeAsync();
  }


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
    var ct = new CancellationTokenSource().Token;

    await using var queue = await ServiceBusTestResources.CreateQueue(
      _container.GetConnectionString(),
      "testqueue",
      new NUnitLogger("servicebus"),
      ct);

    var client = new ServiceBusClient(queue.ConnectionString);

    var serviceBusSender = client.CreateSender(queue.Name);
    await serviceBusSender.SendMessageAsync(new ServiceBusMessage("lol"), ct);

    var receiver = client.CreateReceiver(queue.Name);
    var receivedMessage = await receiver.ReceiveMessageAsync(cancellationToken: ct);

    receivedMessage.Body.ToString().Should().Be("lol");
  }
}