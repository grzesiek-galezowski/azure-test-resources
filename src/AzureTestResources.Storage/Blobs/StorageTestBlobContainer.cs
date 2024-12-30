using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;
using TddXt.AzureTestResources.Common;

namespace TddXt.AzureTestResources.Storage.Blobs;

public class StorageTestBlobContainer(
  BlobServiceClient client,
  string name,
  string connectionString,
  ILogger logger,
  CancellationToken ct)
  : IAzureResourceApi
{
  public string ConnectionString { get; } = connectionString;
  public string Name { get; } = name;

  public async ValueTask DisposeAsync()
  {
    logger.Deleting("blob container", Name);
    await client.DeleteBlobContainerAsync(Name, cancellationToken: ct);
    logger.Deleted("blob container", Name);
  }
}