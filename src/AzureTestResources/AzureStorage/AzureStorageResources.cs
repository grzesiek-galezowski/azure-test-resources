using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using AzureTestResources.AzureStorage.Blobs;
using AzureTestResources.AzureStorage.Common;
using AzureTestResources.AzureStorage.Queues;
using Microsoft.Extensions.Logging;
using Polly;

namespace AzureTestResources.AzureStorage;

public static class AzureStorageResources
{
  public static async Task<StorageTestQueue> CreateQueue(ILogger logger, CancellationToken cancellationToken)
  {
    return await CreateQueue(
      "DefaultEndpointsProtocol=https;" +
      "AccountName=devstoreaccount1;" +
      "AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;" +
      "QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;", 
      logger, 
      cancellationToken);
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

    return await AzureResources.CreateApiToUnderlyingResource(service, logger);
  }

  public static async Task<StorageTestBlobContainer> CreateBlobContainer(ILogger logger, CancellationToken cancellationToken)
  {
    return await CreateBlobContainer(
      //bug correct connection string
      "DefaultEndpointsProtocol=https;" +
      "AccountName=devstoreaccount1;" +
      "AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;" +
      "QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;", 
      logger, 
      cancellationToken);
  }

  public static async Task<StorageTestBlobContainer> CreateBlobContainer(
    string connectionString,
    ILogger logger,
    CancellationToken ct)
  {
    var service = new AzureStorageBlobService(
      new BlobServiceClient(
        connectionString
      ), connectionString, ct);

    return await AzureResources.CreateApiToUnderlyingResource(service, logger);
  }
}

//bug zombie cleanup (for all resources)