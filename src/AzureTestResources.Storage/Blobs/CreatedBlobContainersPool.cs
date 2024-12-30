using Azure.Storage.Blobs;
using TddXt.AzureTestResources.Common;

namespace TddXt.AzureTestResources.Storage.Blobs;

public class CreatedBlobContainersPool(BlobServiceClient serviceClient) : ICreatedResourcesPool
{
  public async Task<IEnumerable<ICreatedResource>> LoadResources()
  {
    return (await serviceClient.GetBlobContainersAsync().ToListAsync().AsTask())
      .Select(i => new CreatedBlobContainer(i, serviceClient));
  }
}