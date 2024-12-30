using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using Microsoft.Extensions.Logging;
using TddXt.AzureTestResources.Common;
using TddXt.AzureTestResources.Storage.Blobs;
using TddXt.AzureTestResources.Storage.Queues;

namespace TddXt.AzureTestResources.Storage;

public partial class AzureStorageResources(string connectionString)
{

  public async Task<StorageTestQueue> CreateQueue(ILogger logger, CancellationToken cancellationToken)
  {
    return await CreateQueue(connectionString, logger, cancellationToken);
  }

  public async Task<StorageTestQueue> CreateQueue(
    string connectionString,
    ILogger logger,
    CancellationToken ct)
  {
    var service = new AzureStorageQueueService(
      new QueueServiceClient(
        connectionString
      ), connectionString, ct);

    return await AzureResources.CreateApiToUnderlyingResource(service, "storage queue", logger);
  }
}

public partial class AzureStorageResources
{
  public async Task<StorageTestBlobContainer> CreateBlobContainer(ILogger logger, CancellationToken cancellationToken)
  {
    return await CreateBlobContainer(connectionString, logger, cancellationToken);
  }

  public async Task<StorageTestBlobContainer> CreateBlobContainer(
    string connectionString,
    ILogger logger,
    CancellationToken ct)
  {
    var service = new AzureStorageBlobService(
      new BlobServiceClient(connectionString),
      connectionString,
      logger,
      ct);

    return await AzureResources.CreateApiToUnderlyingResource(service, "blob container", logger);
  }
}
