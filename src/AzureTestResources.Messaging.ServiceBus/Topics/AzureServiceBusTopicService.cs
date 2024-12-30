using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Microsoft.Extensions.Logging;
using TddXt.AzureTestResources.Common;

namespace TddXt.AzureTestResources.Messaging.ServiceBus.Topics;

public class AzureServiceBusTopicService(
  string connectionString,
  string namePrefix,
  ServiceBusAdministrationClient serviceBusClient,
  TimeSpan autoDeleteOnIdle,
  ILogger logger,
  CancellationToken cancellationToken)
  : IAzureService<ServiceBusTestTopic>
{
  public async Task<ICreateAzureResourceResponse<ServiceBusTestTopic>> CreateResourceInstance()
  {
    try
    {
      var topicName = TestResourceNamingConvention.GenerateResourceId(namePrefix);
      logger.Creating("topic", topicName);
      var sdkResponse = await serviceBusClient.CreateTopicAsync(
        new CreateTopicOptions(topicName)
        {
          AutoDeleteOnIdle = autoDeleteOnIdle
        }, cancellationToken);
      var response = new CreateAzureServiceBusTopicResponse(
        connectionString,
        serviceBusClient,
        sdkResponse,
        topicName,
        logger,
        cancellationToken);
      return response;
    }
    catch (ServiceBusException ex) when (RetryConditions.RequiresRetry(ex))
    {
      return new ResourceCouldNotBeCreatedResponse<ServiceBusTestTopic>(ex, true);
    }
  }
}