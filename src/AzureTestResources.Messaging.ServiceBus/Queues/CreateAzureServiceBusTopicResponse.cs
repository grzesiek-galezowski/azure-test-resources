using Azure;
using Azure.Messaging.ServiceBus.Administration;
using Microsoft.Extensions.Logging;
using TddXt.AzureTestResources.Common;

namespace TddXt.AzureTestResources.Messaging.ServiceBus.Queues;

public class CreateAzureServiceBusQueueResponse(
  string connectionString,
  ServiceBusAdministrationClient serviceBusClient,
  Response<QueueProperties> response,
  string topicName,
  ILogger logger,
  CancellationToken cancellationToken)
  : ICreateAzureResourceResponse<ServiceBusTestQueue>
{
  public void AssertResourceCreated()
  {
    Assertions.AssertIsHttpCreated(response, "queue");
    Assertions.AssertNotNull(response, "queue", topicName);
    Assertions.AssertNamesMatch(topicName, response.Value.Name);
  }

  public bool ShouldBeRetried() => false;
  public string GetReasonForRetry() => "None";
  public ServiceBusTestQueue CreateResourceApi()
  {
    return new ServiceBusTestQueue(
      serviceBusClient,
      response.Value.Name,
      connectionString,
      logger,
      cancellationToken);
  }
}