using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;
using TddXt.AzureTestResources.Common;

namespace TddXt.AzureTestResources.Storage.Blobs;

public static class ZombieBlobContainerCleanup
{
  public static async Task DeleteZombieContainers(ILogger logger)
    => await DeleteZombieContainers(
      AzureStorageResources.AzuriteConnectionString,
      AzureResources.DefaultZombieToleranceForEmulator,
      logger);

  public static async Task DeleteZombieContainers(string connectionString, TimeSpan tolerance, ILogger logger)
  {
    var serviceClient = new BlobServiceClient(connectionString);

    await new ZombieResourceCleanupLoop(
      new CreatedBlobContainersPool(serviceClient),
      tolerance,
      logger).Execute();
  }
}