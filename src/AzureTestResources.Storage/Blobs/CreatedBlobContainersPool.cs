using Azure.Storage.Blobs;
using TddXt.AzureTestResources.Common;

namespace TddXt.AzureTestResources.Storage.Blobs;

public class CreatedBlobContainersPool : ICreatedResourcesPool
{
  private readonly BlobServiceClient _serviceClient;

  public CreatedBlobContainersPool(BlobServiceClient serviceClient)
  {
    _serviceClient = serviceClient;
  }

  public async Task<IEnumerable<ICreatedResource>> LoadResources()
  {
    return (await _serviceClient.GetBlobContainersAsync().ToListAsync().AsTask())
      .Select(i => new CreatedBlobContainer(i, _serviceClient));
  }
}