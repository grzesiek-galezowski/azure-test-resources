using System.Net;
using Azure;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using AzureTestResources.Common;
using Microsoft.Extensions.Logging;

namespace AzureTestResources.Storage.Queues;

public class ZombieStorageQueueCleanup
{
  public static async Task DeleteZombieQueues(ILogger logger)
    => await DeleteZombieQueues(
      AzureStorageResources.AzuriteConnectionString,
      AzureResources.DefaultZombieToleranceForEmulator, logger);

  public static async Task DeleteZombieQueues(string connectionString, TimeSpan tolerance, ILogger logger)
  {
    await new ZombieResourceCleanupLoop(new CreatedStorageQueuePool(connectionString), tolerance, logger)
      .Execute();
  }
}

public class CreatedStorageQueuePool : ICreatedResourcesPool
{
  private readonly QueueServiceClient _serviceClient;

  public CreatedStorageQueuePool(string connectionString)
  {
    _serviceClient = new QueueServiceClient(connectionString);
  }

  public async Task<IEnumerable<ICreatedResource>> LoadResources()
  {
    return (await _serviceClient.GetQueuesAsync().ToListAsync()).Select(r =>
      new CreatedStorageQueue(r, _serviceClient));
  }
}

public class CreatedStorageQueue : ICreatedResource
{
  private readonly QueueItem _queueItem;
  private readonly QueueServiceClient _serviceClient;

  public CreatedStorageQueue(QueueItem queueItem, QueueServiceClient serviceClient)
  {
    _queueItem = queueItem;
    _serviceClient = serviceClient;
  }

  public string Name => _queueItem.Name;

  public async Task DeleteAsync()
  {
    try
    {
      await _serviceClient.GetQueueClient(_queueItem.Name).DeleteIfExistsAsync();
    }
    catch (RequestFailedException e) when (e.Status == (int)HttpStatusCode.NotFound)
    {
      throw new ResourceCouldNotBeDeletedException(e);
    }
  }
}