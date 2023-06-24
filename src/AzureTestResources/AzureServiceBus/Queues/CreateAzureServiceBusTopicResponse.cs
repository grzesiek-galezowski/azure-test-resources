using Azure;
using Azure.Messaging.ServiceBus.Administration;
using AzureTestResources.Common;

namespace AzureTestResources.AzureServiceBus.Queues;

public class CreateAzureServiceBusQueueResponse : ICreateAzureResourceResponse<ServiceBusTestQueue>
{
    private readonly Response<QueueProperties> _response;
    private readonly string _topicName;
    private readonly ServiceBusAdministrationClient _serviceBusClient;
    private readonly CancellationToken _cancellationToken;
    private readonly string _connectionString;

    public CreateAzureServiceBusQueueResponse(
      string connectionString,
      CancellationToken cancellationToken,
      ServiceBusAdministrationClient serviceBusClient,
      Response<QueueProperties> response,
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
        Assertions.AssertIsHttpCreated(_response, "queue");
        Assertions.AssertNotNull(_response, "queue", _topicName);
        Assertions.AssertNamesMatch(_topicName, _response.Value.Name);
    }

    public bool ShouldBeRetried() => false;
    public string GetReasonForRetry() => "None";
    public ServiceBusTestQueue CreateResourceApi()
    {
        return new ServiceBusTestQueue(
          _serviceBusClient,
          _response.Value.Name,
          _connectionString,
          _cancellationToken);
    }
}