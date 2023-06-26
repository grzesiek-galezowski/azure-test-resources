using System.Net;
using Azure;
using Azure.Storage.Blobs;
using AzureTestResources.Common;

namespace AzureTestResources.AzureStorage.Blobs;

public class ZombieBlobContainerCleanup
{
  public static async Task DeleteZombieContainers()
    => await DeleteZombieContainers(
      AzureStorageResources.AzuriteConnectionString, 
      AzureResources.DefaultZombieToleranceForEmulator);

  public static async Task DeleteZombieContainers(string connectionString, TimeSpan tolerance)
  {
    var serviceClient = new BlobServiceClient(connectionString);

    await foreach (var blobContainerItem in serviceClient.GetBlobContainersAsync())
    {
      try
      {
        if (TestResourceNamingConvention.IsAZombieResource(tolerance, blobContainerItem.Name))
        {
          await serviceClient.GetBlobContainerClient(blobContainerItem.Name).DeleteIfExistsAsync();
        }
      }
      catch (RequestFailedException e) when (e.Status == (int)HttpStatusCode.NotFound)
      {
        //silently ignore (todo: add more logging)
      }
    }
  }
}