using Azure.Messaging.ServiceBus.Administration;
using Microsoft.Extensions.Logging;
using TddXt.AzureTestResources.Common;

namespace TddXt.AzureTestResources.Messaging.ServiceBus.Queues;

public class ServiceBusTestQueue(
  ServiceBusAdministrationClient serviceBusClient,
  string queueName,
  string connectionString,
  ILogger logger,
  CancellationToken cancellationToken)
  : IAzureResourceApi
{
  public string ConnectionString { get; } = connectionString;
  public string Name { get; } = queueName;

  public async ValueTask DisposeAsync()
  {
    logger.Deleting("queue", Name);
    await serviceBusClient.DeleteQueueAsync(Name, cancellationToken: cancellationToken);
    logger.Deleted("Queue", Name);
  }
}