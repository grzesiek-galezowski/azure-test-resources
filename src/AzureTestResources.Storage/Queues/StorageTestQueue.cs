using Azure.Storage.Queues;
using TddXt.AzureTestResources.Common;

namespace TddXt.AzureTestResources.Storage.Queues;

public class StorageTestQueue(
  QueueServiceClient client,
  string name,
  string connectionString,
  CancellationToken ct)
  : IAzureResourceApi
{
  public string ConnectionString { get; } = connectionString;
  public string Name { get; } = name;

  public async ValueTask DisposeAsync()
  {
    await client.DeleteQueueAsync(Name, ct);
  }
}