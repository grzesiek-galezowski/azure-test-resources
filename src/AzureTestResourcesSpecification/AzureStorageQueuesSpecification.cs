using Azure.Storage.Queues;
using AzureTestResources.AzureStorage;
using AzureTestResources.AzureStorage.Queues;
using Extensions.Logging.NUnit;

namespace AzureTestResourcesSpecification;

public class AzureStorageQueuesSpecification
{
  private readonly Lazy<Task> _deleteAllQueues 
    = new(ZombieStorageQueueCleanup.DeleteZombieQueues);

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
    await using var queue = await AzureStorageResources.CreateQueue(
      new NUnitLogger("storagequeue"), new CancellationToken());
    var queueClient = new QueueClient(queue.ConnectionString, queue.Name);
    await queueClient.SendMessageAsync(messageText);

    var message = await queueClient.ReceiveMessageAsync();
    Assert.AreEqual(messageText, message.Value.MessageText);
  }
}