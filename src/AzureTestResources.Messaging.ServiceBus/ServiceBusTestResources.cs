using Azure.Messaging.ServiceBus.Administration;
using Microsoft.Extensions.Logging;
using TddXt.AzureTestResources.Common;
using TddXt.AzureTestResources.Messaging.ServiceBus.Queues;
using TddXt.AzureTestResources.Messaging.ServiceBus.Topics;

namespace TddXt.AzureTestResources.Messaging.ServiceBus;

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
      new ServiceBusAdministrationClient(connectionString),
      AutoDeleteOnIdle,
      logger,
      cancellationToken);

    var instance = await AzureResources.CreateApiToUnderlyingResource(service, "topic", logger);

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
      new ServiceBusAdministrationClient(connectionString),
      AutoDeleteOnIdle,
      logger,
      cancellationToken);

    var instance = await AzureResources.CreateApiToUnderlyingResource(service, "queue", logger);

    return instance;
  }
}