using System.Net;
using Azure;
using Azure.Storage.Queues;
using AzureTestResources.Common;

namespace AzureTestResources.AzureStorage.Queues;

public class CreateAzureStorageQueueResponse : ICreateAzureResourceResponse<StorageTestQueue>
{
  private readonly Response<QueueClient> _response;
  private readonly string _resourceName;
  private readonly QueueServiceClient _client;
  private readonly string _connectionString;
  private readonly CancellationToken _cancellationToken;

  public CreateAzureStorageQueueResponse(
    Response<QueueClient> response,
    string resourceName,
    QueueServiceClient client,
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
    var resourceType = "queue";
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

  public StorageTestQueue CreateResourceApi()
  {
    return new StorageTestQueue(
      _client,
      _response.Value.Name,
      _connectionString,
      _cancellationToken);
  }
}