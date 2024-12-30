using System.Net;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using TddXt.AzureTestResources.Common;

namespace TddXt.AzureTestResources.Storage.Blobs;

public class CreatedBlobContainer(BlobContainerItem blobContainerItem, BlobServiceClient blobServiceClient)
  : ICreatedResource
{
  public string Name => blobContainerItem.Name;

  public async Task DeleteAsync()
  {
    try
    {
      await blobServiceClient.GetBlobContainerClient(Name).DeleteIfExistsAsync();
    }
    catch (RequestFailedException e) when (e.Status == (int)HttpStatusCode.NotFound)
    {
      throw new ResourceCouldNotBeDeletedException(e);
    }
  }
}