using System.Net;
using Azure;
using Azure.Storage.Queues;
using TddXt.AzureTestResources.Common;

namespace TddXt.AzureTestResources.Storage.Queues;

public class CreateAzureStorageQueueResponse(
  Response<QueueClient> response,
  string resourceName,
  QueueServiceClient client,
  string connectionString,
  CancellationToken cancellationToken)
  : ICreateAzureResourceResponse<StorageTestQueue>
{
  public void AssertResourceCreated()
  {
    var resourceType = "queue";
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

  public StorageTestQueue CreateResourceApi()
  {
    return new StorageTestQueue(
      client,
      response.Value.Name,
      connectionString,
      cancellationToken);
  }
}