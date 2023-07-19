using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using Microsoft.Extensions.Logging;
using TddXt.AzureTestResources.Common;
using TddXt.AzureTestResources.Storage.Blobs;
using TddXt.AzureTestResources.Storage.Queues;

namespace TddXt.AzureTestResources.Storage;

public static partial class AzureStorageResources
{
  public const string AzuriteConnectionString = "DefaultEndpointsProtocol=http;" +
                                               "AccountName=devstoreaccount1;" +
                                               "AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;" +
                                               "BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;" +
                                               "QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;" +
                                               "TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;";
}

public static partial class AzureStorageResources
{

  public static async Task<StorageTestQueue> CreateQueue(ILogger logger, CancellationToken cancellationToken)
  {
    return await CreateQueue(AzuriteConnectionString, logger, cancellationToken);
  }

  public static async Task<StorageTestQueue> CreateQueue(
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

public static partial class AzureStorageResources
{
  public static async Task<StorageTestBlobContainer> CreateBlobContainer(ILogger logger, CancellationToken cancellationToken)
  {
    return await CreateBlobContainer(AzuriteConnectionString, logger, cancellationToken);
  }

  public static async Task<StorageTestBlobContainer> CreateBlobContainer(
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
