using Azure.Messaging.ServiceBus.Administration;
using AzureTestResources.AzureServiceBus.Queues;
using AzureTestResources.AzureServiceBus.Topics;
using AzureTestResources.Common;
using Microsoft.Extensions.Logging;

namespace AzureTestResources.AzureServiceBus;

public static class ServiceBusTestResources
{
  private static readonly TimeSpan AutoDeleteOnIdle = TimeSpan.FromMinutes(5);

  public static async Task<ServiceBusTestTopic> CreateTopic(
    string connectionString,
    string namePrefix,
    ILogger logger,
    CancellationToken cancellationToken)
  {
    var service = new AzureServiceBusTopicService(
      connectionString,
      namePrefix,
      cancellationToken,
      new ServiceBusAdministrationClient(connectionString), 
      AutoDeleteOnIdle);

    var instance = await AzureResources.CreateApiToUnderlyingResource(service, logger);

    return instance;
  }

  public static async Task<ServiceBusTestQueue> CreateQueue(
    string connectionString,
    string namePrefix,
    ILogger logger,
    CancellationToken cancellationToken)
  {
    var service = new AzureServiceBusQueueService(
      connectionString,
      namePrefix,
      cancellationToken,
      new ServiceBusAdministrationClient(connectionString), 
      AutoDeleteOnIdle);

    var instance = await AzureResources.CreateApiToUnderlyingResource(service, logger);

    return instance;
  }
}