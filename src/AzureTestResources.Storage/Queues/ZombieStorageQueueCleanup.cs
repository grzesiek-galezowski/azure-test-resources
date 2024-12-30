using System.Net;
using Azure;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Logging;
using TddXt.AzureTestResources.Common;

namespace TddXt.AzureTestResources.Storage.Queues;

public class ZombieStorageQueueCleanup
{
  public static async Task DeleteZombieQueues(string connectionString, ILogger logger)
    => await DeleteZombieQueues(
      connectionString,
      AzureResources.DefaultZombieToleranceForEmulator, logger);

  public static async Task DeleteZombieQueues(string connectionString, TimeSpan tolerance, ILogger logger)
  {
    await new ZombieResourceCleanupLoop(new CreatedStorageQueuePool(connectionString), tolerance, logger)
      .Execute();
  }
}

public class CreatedStorageQueuePool(string connectionString) : ICreatedResourcesPool
{
  private readonly QueueServiceClient _serviceClient = new(connectionString);

  public async Task<IEnumerable<ICreatedResource>> LoadResources()
  {
    return (await _serviceClient.GetQueuesAsync().ToListAsync()).Select(r =>
      new CreatedStorageQueue(r, _serviceClient));
  }
}

public class CreatedStorageQueue(QueueItem queueItem, QueueServiceClient serviceClient) : ICreatedResource
{
  public string Name => queueItem.Name;

  public async Task DeleteAsync()
  {
    try
    {
      await serviceClient.GetQueueClient(queueItem.Name).DeleteIfExistsAsync();
    }
    catch (RequestFailedException e) when (e.Status == (int)HttpStatusCode.NotFound)
    {
      throw new ResourceCouldNotBeDeletedException(e);
    }
  }
}