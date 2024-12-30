using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Microsoft.Extensions.Logging;
using TddXt.AzureTestResources.Common;

namespace TddXt.AzureTestResources.Messaging.ServiceBus.Queues;

public class AzureServiceBusQueueService(
  string connectionString,
  string namePrefix,
  ServiceBusAdministrationClient serviceBusClient,
  TimeSpan autoDeleteOnIdle,
  ILogger logger,
  CancellationToken cancellationToken)
  : IAzureService<ServiceBusTestQueue>
{
  public async Task<ICreateAzureResourceResponse<ServiceBusTestQueue>> CreateResourceInstance()
  {
    try
    {
      var queueName = TestResourceNamingConvention.GenerateResourceId(namePrefix);

      logger.Creating("service bus queue", queueName);

      var sdkResponse = await serviceBusClient.CreateQueueAsync(
        new CreateQueueOptions(queueName)
        {
          AutoDeleteOnIdle = autoDeleteOnIdle
        }, cancellationToken);
      var response = new CreateAzureServiceBusQueueResponse(
        connectionString,
        serviceBusClient,
        sdkResponse,
        queueName,
        logger,
        cancellationToken);

      return response;
    }
    catch (ServiceBusException ex) when (RetryConditions.RequiresRetry(ex))
    {
      return new ResourceCouldNotBeCreatedResponse<ServiceBusTestQueue>(ex, true);
    }
  }
}