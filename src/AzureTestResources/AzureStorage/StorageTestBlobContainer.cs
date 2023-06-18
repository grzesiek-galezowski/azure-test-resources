using Azure.Storage.Blobs;

namespace AzureTestResources.AzureStorage;

public class StorageTestBlobContainer : IAsyncDisposable
{
  private readonly BlobContainerClient _client;
  private readonly CancellationToken _ct;
  
  public string ConnectionString { get; }
  public string Name { get; }

  public StorageTestBlobContainer(
    BlobContainerClient client,
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
    await _client.DeleteAsync(cancellationToken: _ct);
  }
}