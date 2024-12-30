using Azure.Storage.Queues;
using Extensions.Logging.NUnit;
using FluentAssertions;
using TddXt.AzureTestResources.Storage;
using TddXt.AzureTestResources.Storage.Queues;
using Testcontainers.Azurite;

namespace TddXt.AzureTestResourcesSpecification;

public class AzureStorageQueuesSpecification
{
  private AzuriteContainer _container;
  private Lazy<Task> _deleteAllQueues;

  [OneTimeSetUp]
  public async Task SetUpEmulator()
  {
    _container = await DockerContainersForTests.StartAzuriteContainer();

    _deleteAllQueues = new(() => ZombieStorageQueueCleanup.DeleteZombieQueues(_container.GetConnectionString(),
      new NUnitLogger("storagequeue")));
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
  public async Task ShouldCreateAzureStorageQueue(int testNo)
  {
    await _deleteAllQueues.Value;

    var messageText = "lol";
    var cancellationToken = new CancellationTokenSource().Token;
    await using var queue = await new AzureStorageResources(_container.GetConnectionString()).CreateQueue(
      new NUnitLogger("storagequeue"), //bug consider one logger for the resources object
      cancellationToken);
    var queueClient = new QueueClient(queue.ConnectionString, queue.Name);
    await queueClient.SendMessageAsync(messageText, cancellationToken);

    var message = await queueClient.ReceiveMessageAsync(cancellationToken: cancellationToken);
    message.Value.MessageText.Should().Be(messageText);
  }
}