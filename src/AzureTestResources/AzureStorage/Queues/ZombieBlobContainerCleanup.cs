using System.Net;
using Azure;
using Azure.Storage.Queues;
using AzureTestResources.Common;

namespace AzureTestResources.AzureStorage.Queues;

public class ZombieStorageQueueCleanup
{
  //bug commonalize default tolerance, maybe call it emulator tolerance or sth.
  private static readonly TimeSpan DefaultTolerance = TimeSpan.FromMinutes(1);

  //bug make these parameterless versions for all zombie routines
  //and the other should accept config or both connection string+tolerance
  public static async Task DeleteZombieQueues()
    => await DeleteZombieQueues(AzureStorageResources.AzuriteConnectionString, DefaultTolerance);

  public static async Task DeleteZombieQueues(string connectionString, TimeSpan tolerance)
  {
    var serviceClient = new QueueServiceClient(connectionString);

    await foreach (var blobContainerItem in serviceClient.GetQueuesAsync())
    {
      try
      {
        if (TestResourceNamingConvention.IsAZombieResource(tolerance, blobContainerItem.Name))
        {
          await serviceClient.GetQueueClient(blobContainerItem.Name).DeleteIfExistsAsync();
        }
      }
      catch (RequestFailedException e) when (e.Status == (int)HttpStatusCode.NotFound)
      {
        //silently ignore (todo: add more logging)
      }
    }
  }
}