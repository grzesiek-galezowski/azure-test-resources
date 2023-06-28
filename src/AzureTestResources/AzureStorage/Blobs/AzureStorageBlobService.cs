using Azure;
using Azure.Storage.Blobs;
using AzureTestResources.Common;
using Microsoft.Extensions.Logging;

namespace AzureTestResources.AzureStorage.Blobs;

public class AzureStorageBlobService : IAzureService<StorageTestBlobContainer>
{
  private readonly BlobServiceClient _client;
  private readonly string _connectionString;
  private readonly ILogger _logger;
  private readonly CancellationToken _cancellationToken;

  public AzureStorageBlobService(
    BlobServiceClient client,
    string connectionString,
    ILogger logger,
    CancellationToken cancellationToken)
  {
    _client = client;
    _connectionString = connectionString;
    _logger = logger;
    _cancellationToken = cancellationToken;
  }

  public async Task<ICreateAzureResourceResponse<StorageTestBlobContainer>> CreateResourceInstance()
  {
    try
    {
      var resourceName = TestResourceNamingConvention.GenerateResourceId(
        "b" /* see https://learn.microsoft.com/en-us/rest/api/storageservices/naming-and-referencing-containers--blobs--and-metadata#resource-names */);
      
      _logger.Creating("blob container", resourceName);
      var response = new CreateAzureStorageBlobContainerResponse(
        await _client.CreateBlobContainerAsync(resourceName, cancellationToken: _cancellationToken),
        resourceName,
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