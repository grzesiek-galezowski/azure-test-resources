using System.Net;
using Azure;
using Azure.Storage.Blobs;
using AzureTestResources.Common;

namespace AzureTestResources.AzureStorage.Blobs;

public class CreateAzureStorageBlobContainerResponse : ICreateAzureResourceResponse<StorageTestBlobContainer>
{
  private readonly Response<BlobContainerClient> _response;
  private readonly string _resourceName;
  private readonly BlobServiceClient _client;
  private readonly string _connectionString;
  private readonly CancellationToken _cancellationToken;

  public CreateAzureStorageBlobContainerResponse(
    Response<BlobContainerClient> response,
    string resourceName,
    BlobServiceClient client,
    string connectionString,
    CancellationToken cancellationToken)
  {
    _response = response;
    _resourceName = resourceName;
    _client = client;
    _connectionString = connectionString;
    _cancellationToken = cancellationToken;
  }

  public void AssertResourceCreated()
  {
    var resourceType = "blob container";
    Assertions.AssertNotNull(_response, resourceType, _resourceName);
    Assertions.AssertNamesMatch(_resourceName, _response.Value.Name);
    Assertions.AssertIsHttpCreated(_response, resourceType);
  }

  public bool ShouldBeRetried()
  {
    return _response.GetRawResponse().Status == (int)HttpStatusCode.NoContent;
  }

  public string GetReasonForRetry()
  {
    return $"status code {_response.GetRawResponse().Status} and error {_response.GetRawResponse().ReasonPhrase}";
  }

  public StorageTestBlobContainer CreateResourceApi()
  {
    return new StorageTestBlobContainer(
      _client,
      _response.Value.Name,
      _cancellationToken,
      _connectionString);
  }
}