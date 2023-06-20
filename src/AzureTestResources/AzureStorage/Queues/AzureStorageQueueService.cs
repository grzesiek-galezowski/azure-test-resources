using Azure.Storage.Queues;
using AzureTestResources.AzureStorage.Common;

namespace AzureTestResources.AzureStorage.Queues;

public class AzureStorageQueueService : IAzureService<StorageTestQueue>
{
  private readonly QueueServiceClient _client;
  private readonly string _connectionString;
  private readonly CancellationToken _cancellationToken;

  public AzureStorageQueueService(QueueServiceClient client, string connectionString,
    CancellationToken cancellationToken)
  {
    _client = client;
    _connectionString = connectionString;
    _cancellationToken = cancellationToken;
  }

  public async Task<ICreateAzureResourceResponse<StorageTestQueue>> CreateResourceInstance()
  {
    //var resourceId = TestResourceNamingConvention.GenerateResourceId(
    //  "q" /* see https://learn.microsoft.com/en-us/rest/api/storageservices/naming-queues-and-metadata#queue-names */);
    var resourceId = "q12345";
    var response = new CreateAzureStorageQueueResponse(
      await _client.CreateQueueAsync(resourceId, cancellationToken: _cancellationToken),
      resourceId,
      _client,
      _connectionString,
      _cancellationToken);
    return response;
  }
}