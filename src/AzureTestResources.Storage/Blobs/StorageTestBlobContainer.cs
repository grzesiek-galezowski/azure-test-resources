using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;
using TddXt.AzureTestResources.Common;

namespace TddXt.AzureTestResources.Storage.Blobs;

public class StorageTestBlobContainer : IAzureResourceApi
{
  private readonly BlobServiceClient _client;
  private readonly ILogger _logger;
  private readonly CancellationToken _ct;

  public string ConnectionString { get; }
  public string Name { get; }

  public StorageTestBlobContainer(
    BlobServiceClient client,
    string name,
    string connectionString,
    ILogger logger,
    CancellationToken ct)
  {
    _client = client;
    _logger = logger;
    _ct = ct;
    ConnectionString = connectionString;
    Name = name;
  }

  public async ValueTask DisposeAsync()
  {
    _logger.Deleting("blob container", Name);
    await _client.DeleteBlobContainerAsync(Name, cancellationToken: _ct);
    _logger.Deleted("blob container", Name);
  }
}