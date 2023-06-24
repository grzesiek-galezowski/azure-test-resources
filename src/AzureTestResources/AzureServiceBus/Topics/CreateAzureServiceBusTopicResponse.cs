using Azure;
using Azure.Messaging.ServiceBus.Administration;
using AzureTestResources.Common;

namespace AzureTestResources.AzureServiceBus.Topics;

public class CreateAzureServiceBusTopicResponse : ICreateAzureResourceResponse<ServiceBusTestTopic>
{
    private readonly Response<TopicProperties> _response;
    private readonly string _topicName;
    private readonly ServiceBusAdministrationClient _serviceBusClient;
    private readonly CancellationToken _cancellationToken;
    private readonly string _connectionString;

    public CreateAzureServiceBusTopicResponse(
      string connectionString,
      CancellationToken cancellationToken,
      ServiceBusAdministrationClient serviceBusClient,
      Response<TopicProperties> response,
      string topicName)
    {
        _connectionString = connectionString;
        _cancellationToken = cancellationToken;
        _serviceBusClient = serviceBusClient;
        _response = response;
        _topicName = topicName;
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
          _cancellationToken);
    }
}