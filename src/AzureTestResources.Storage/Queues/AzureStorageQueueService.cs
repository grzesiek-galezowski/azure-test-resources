using Azure.Storage.Queues;
using TddXt.AzureTestResources.Common;

namespace TddXt.AzureTestResources.Storage.Queues;

public class AzureStorageQueueService(
  QueueServiceClient client,
  string connectionString,
  CancellationToken cancellationToken)
  : IAzureService<StorageTestQueue>
{
  public async Task<ICreateAzureResourceResponse<StorageTestQueue>> CreateResourceInstance()
  {
    var resourceId = TestResourceNamingConvention.GenerateResourceId(
      "q" /* see https://learn.microsoft.com/en-us/rest/api/storageservices/naming-queues-and-metadata#queue-names */);
    var response = new CreateAzureStorageQueueResponse(
      await client.CreateQueueAsync(resourceId, cancellationToken: cancellationToken),
      resourceId,
      client,
      connectionString,
      cancellationToken);
    return response;
  }
}