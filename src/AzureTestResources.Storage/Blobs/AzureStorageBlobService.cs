using Azure;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;
using TddXt.AzureTestResources.Common;

namespace TddXt.AzureTestResources.Storage.Blobs;

public class AzureStorageBlobService(
  BlobServiceClient client,
  string connectionString,
  ILogger logger,
  CancellationToken cancellationToken)
  : IAzureService<StorageTestBlobContainer>
{
  public async Task<ICreateAzureResourceResponse<StorageTestBlobContainer>> CreateResourceInstance()
  {
    try
    {
      var resourceName = TestResourceNamingConvention.GenerateResourceId(
        "b" /* see https://learn.microsoft.com/en-us/rest/api/storageservices/naming-and-referencing-containers--blobs--and-metadata#resource-names */);

      logger.Creating("blob container", resourceName);
      var response = new CreateAzureStorageBlobContainerResponse(
        await client.CreateBlobContainerAsync(resourceName, cancellationToken: cancellationToken),
        resourceName,
        client,
        connectionString,
        logger,
        cancellationToken);
      return response;
    }
    catch (RequestFailedException ex)
    {
      return new ResourceCouldNotBeCreatedResponse<StorageTestBlobContainer>(
        ex,
        ex.ErrorCode == "ContainerAlreadyExists");
    }
  }
}