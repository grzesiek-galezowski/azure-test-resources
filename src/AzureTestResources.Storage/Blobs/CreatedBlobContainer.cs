using System.Net;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzureTestResources.Common;

namespace AzureTestResources.Storage.Blobs;

public class CreatedBlobContainer : ICreatedResource
{
  private readonly BlobContainerItem _blobContainerItem;
  private readonly BlobServiceClient _blobServiceClient;

  public CreatedBlobContainer(BlobContainerItem blobContainerItem, BlobServiceClient blobServiceClient)
  {
    _blobContainerItem = blobContainerItem;
    _blobServiceClient = blobServiceClient;
  }

  public string Name => _blobContainerItem.Name;

  public async Task DeleteAsync()
  {
    try
    {
      await _blobServiceClient.GetBlobContainerClient(Name).DeleteIfExistsAsync();
    }
    catch (RequestFailedException e) when (e.Status == (int)HttpStatusCode.NotFound)
    {
      throw new ResourceCouldNotBeDeletedException(e);
    }
  }
}