using System.Net;
using Azure;
using Azure.Storage.Blobs;
using AzureTestResources.Common;
using Microsoft.Extensions.Logging;

namespace AzureTestResources.AzureStorage.Blobs;

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

    await foreach (var blobContainerItem in serviceClient.GetBlobContainersAsync())
    {
      try
      {
        logger.LogInformation($"Evaluating {blobContainerItem.Name} for zombie cleanup.");
        if (TestResourceNamingConvention.IsAZombieResource(tolerance, blobContainerItem.Name))
        {
          logger.LogInformation($"{blobContainerItem.Name} identified as a zombie resource. Trying to delete...");
          await serviceClient.GetBlobContainerClient(blobContainerItem.Name).DeleteIfExistsAsync();
          logger.LogInformation($"{blobContainerItem.Name} deleted successfully.");
        }
      }
      catch (RequestFailedException e) when (e.Status == (int)HttpStatusCode.NotFound)
      {
        logger.LogInformation($"{blobContainerItem.Name} could not be deleted. Most probably was already deleted by another process.");
      }
    }
  }
}