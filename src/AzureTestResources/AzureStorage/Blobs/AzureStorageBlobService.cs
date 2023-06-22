using Azure;
using Azure.Storage.Blobs;
using AzureTestResources.Common;

namespace AzureTestResources.AzureStorage.Blobs;

public class AzureStorageBlobService : IAzureService<StorageTestBlobContainer>
{
  private readonly BlobServiceClient _client;
  private readonly string _connectionString;
  private readonly CancellationToken _cancellationToken;

  public AzureStorageBlobService(BlobServiceClient client, string connectionString, CancellationToken cancellationToken)
  {
    _client = client;
    _connectionString = connectionString;
    _cancellationToken = cancellationToken;
  }

  public async Task<ICreateAzureResourceResponse<StorageTestBlobContainer>> CreateResourceInstance()
  {
    try
    {
      var resourceId = TestResourceNamingConvention.GenerateResourceId(
        "b" /* see https://learn.microsoft.com/en-us/rest/api/storageservices/naming-and-referencing-containers--blobs--and-metadata#resource-names */);
      var response = new CreateAzureStorageBlobContainerResponse(
        await _client.CreateBlobContainerAsync(resourceId, cancellationToken: _cancellationToken),
        resourceId,
        _client,
        _connectionString,
        _cancellationToken);
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