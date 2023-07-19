using Azure;
using Azure.Messaging.ServiceBus.Administration;
using Microsoft.Extensions.Logging;
using TddXt.AzureTestResources.Common;

namespace TddXt.AzureTestResources.Messaging.ServiceBus.Topics;

public class CreateAzureServiceBusTopicResponse : ICreateAzureResourceResponse<ServiceBusTestTopic>
{
  private readonly Response<TopicProperties> _response;
  private readonly string _topicName;
  private readonly ServiceBusAdministrationClient _serviceBusClient;
  private readonly CancellationToken _cancellationToken;
  private readonly string _connectionString;
  private readonly ILogger _logger;

  public CreateAzureServiceBusTopicResponse(
    string connectionString,
    ServiceBusAdministrationClient serviceBusClient,
    Response<TopicProperties> response,
    string topicName,
    ILogger logger,
    CancellationToken cancellationToken)
  {
    _connectionString = connectionString;
    _cancellationToken = cancellationToken;
    _serviceBusClient = serviceBusClient;
    _response = response;
    _topicName = topicName;
    _logger = logger;
  }

  public void AssertResourceCreated()
  {
    Assertions.AssertIsHttpCreated(_response, "topic");
    Assertions.AssertNotNull(_response, "topic", _topicName);
    Assertions.AssertNamesMatch(_topicName, _response.Value.Name);
  }

  public bool ShouldBeRetried() => false;
  public string GetReasonForRetry() => "None";
  public ServiceBusTestTopic CreateResourceApi()
  {
    return new ServiceBusTestTopic(
      _serviceBusClient,
      _response.Value.Name,
      _connectionString,
      _logger,
      _cancellationToken);
  }
}