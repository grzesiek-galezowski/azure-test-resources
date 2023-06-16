using Azure.Storage.Queues;

namespace AzureTestResources.AzureStorage;

public class StorageTestQueue : IAsyncDisposable
{
  private readonly QueueClient _client;
  private readonly CancellationToken _ct;
  
  public string ConnectionString { get; }
  public string Name { get; }

  public StorageTestQueue(
    QueueClient client,
    string name,
    CancellationToken ct,
    string connectionString)
  {
    _client = client;
    _ct = ct;
    ConnectionString = connectionString;
    Name = name;
  }

  public async ValueTask DisposeAsync()
  {
    await _client.DeleteAsync(_ct);
  }

  //bug rename?
}
//bug service bus queue