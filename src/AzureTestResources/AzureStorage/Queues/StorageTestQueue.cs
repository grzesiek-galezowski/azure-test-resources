using Azure.Storage.Queues;
using AzureTestResources.Common;

namespace AzureTestResources.AzureStorage.Queues;

public class StorageTestQueue : IAzureResourceApi
{
  private readonly QueueServiceClient _client;
  private readonly CancellationToken _ct;

  public string ConnectionString { get; }
  public string Name { get; }

  public StorageTestQueue(QueueServiceClient client,
    string name,
    string connectionString,
    CancellationToken ct)
  {
    _client = client;
    _ct = ct;
    ConnectionString = connectionString;
    Name = name;
  }

  public async ValueTask DisposeAsync()
  {
    await _client.DeleteQueueAsync(Name, _ct);
  }
}