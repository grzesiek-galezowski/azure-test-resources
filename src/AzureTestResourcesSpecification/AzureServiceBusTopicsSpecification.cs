using Azure.Messaging.ServiceBus;
using DotNet.Testcontainers.Containers;
using Extensions.Logging.NUnit;
using FluentAssertions;
using TddXt.AzureTestResources.Messaging.ServiceBus;

namespace TddXt.AzureTestResourcesSpecification;

public class AzureServiceBusTopicsSpecification
{
  // will not work yet, because the emulator does not support admin sdk
  private IContainer _container;

  [OneTimeSetUp]
  public async Task SetUpEmulator()
  {
    _container = await DockerContainersForTests.StartServiceBusContainer2();
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
  public async Task ShouldCreateAzureServiceBusTopic(int testNo)
  {
    //GIVEN
    var ct = new CancellationTokenSource().Token;


    await using var topic = await ServiceBusTestResources.CreateTopic(
      "Endpoint=sb://default.default.default.localhost.localsandbox.sh:5672;SharedAccessKeyName=1234;SharedAccessKey=password;UseDevelopmentEmulator=true",
      "testTopic",
      new NUnitLogger("servicebus"),
      ct);

    await topic.CreateSubscription("MyMessages");

    var client = new ServiceBusClient(topic.ConnectionString);

    var serviceBusSender = client.CreateSender(topic.Name);
    await serviceBusSender.SendMessageAsync(new ServiceBusMessage("lol"), ct);

    var receiver = client.CreateReceiver(topic.Name, "MyMessages");
    var receivedMessage = await receiver.ReceiveMessageAsync(cancellationToken: ct);

    receivedMessage.Body.ToString().Should().Be("lol");
  }
}