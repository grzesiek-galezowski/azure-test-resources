using Azure;
using Azure.Messaging.ServiceBus.Administration;
using Microsoft.Extensions.Logging;
using TddXt.AzureTestResources.Common;

namespace TddXt.AzureTestResources.Messaging.ServiceBus.Topics;

public class CreateAzureServiceBusTopicResponse(
  string connectionString,
  ServiceBusAdministrationClient serviceBusClient,
  Response<TopicProperties> response,
  string topicName,
  ILogger logger,
  CancellationToken cancellationToken)
  : ICreateAzureResourceResponse<ServiceBusTestTopic>
{
  public void AssertResourceCreated()
  {
    Assertions.AssertIsHttpCreated(response, "topic");
    Assertions.AssertNotNull(response, "topic", topicName);
    Assertions.AssertNamesMatch(topicName, response.Value.Name);
  }

  public bool ShouldBeRetried() => false;
  public string GetReasonForRetry() => "None";
  public ServiceBusTestTopic CreateResourceApi()
  {
    return new ServiceBusTestTopic(
      serviceBusClient,
      response.Value.Name,
      connectionString,
      logger,
      cancellationToken);
  }
}