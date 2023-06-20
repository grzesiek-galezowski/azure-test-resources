using Azure.Storage.Blobs;
using AzureTestResources.AzureStorage.Common;

namespace AzureTestResources.AzureStorage.Blobs;

public class StorageTestBlobContainer : IAzureResourceApi
{
  private readonly BlobServiceClient _client;
  private readonly CancellationToken _ct;

  public string ConnectionString { get; }
  public string Name { get; }

  public StorageTestBlobContainer(
    BlobServiceClient client,
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
    await _client.DeleteBlobContainerAsync(Name, cancellationToken: _ct);
  }
}