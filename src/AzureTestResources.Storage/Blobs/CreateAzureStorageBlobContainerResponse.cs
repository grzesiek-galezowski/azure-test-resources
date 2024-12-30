using System.Net;
using Azure;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;
using TddXt.AzureTestResources.Common;

namespace TddXt.AzureTestResources.Storage.Blobs;

public class CreateAzureStorageBlobContainerResponse(
  Response<BlobContainerClient> response,
  string resourceName,
  BlobServiceClient client,
  string connectionString,
  ILogger logger,
  CancellationToken cancellationToken)
  : ICreateAzureResourceResponse<StorageTestBlobContainer>
{
  public void AssertResourceCreated()
  {
    var resourceType = "blob container";
    Assertions.AssertNotNull(response, resourceType, resourceName);
    Assertions.AssertNamesMatch(resourceName, response.Value.Name);
    Assertions.AssertIsHttpCreated(response, resourceType);
  }

  public bool ShouldBeRetried()
  {
    return response.GetRawResponse().Status == (int)HttpStatusCode.NoContent;
  }

  public string GetReasonForRetry()
  {
    return $"status code {response.GetRawResponse().Status} and error {response.GetRawResponse().ReasonPhrase}";
  }

  public StorageTestBlobContainer CreateResourceApi()
  {
    return new StorageTestBlobContainer(
      client,
      response.Value.Name,
      connectionString,
      logger,
      cancellationToken);
  }
}