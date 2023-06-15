using Azure.Messaging.ServiceBus;
using Azure.Storage.Queues;
using AzureTestResources.AzureServiceBusTopics;
using Extensions.Logging.NUnit;

namespace AzureTestResourcesSpecification;

public class AzureStorageQueuesSpecification
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
    var ct = new CancellationToken();
    var client = new QueueClient(
      "DefaultEndpointsProtocol=https;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;", "queue-name" + testNo
    );    
    // Create the queue if it doesn't already exist
    await client.CreateAsync(cancellationToken: ct);

    await client.DeleteAsync(ct);
  }
}