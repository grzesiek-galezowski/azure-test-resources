using System.Net;
using Azure;
using Azure.Storage.Blobs;
using AzureTestResources.Common;

namespace AzureTestResources.AzureStorage.Blobs;

public class ZombieBlobContainerCleanup
{
  //bug commonalize default tolerance, maybe call it emulator tolerance or sth.
  private static readonly TimeSpan DefaultTolerance = TimeSpan.FromMinutes(1);

  //bug make these parameterless versions for all zombie routines
  //and the other should accept config or both connection string+tolerance
  public static async Task DeleteZombieContainers()
    => await DeleteZombieContainers(AzureStorageResources.AzuriteConnectionString, DefaultTolerance);

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