using Azure.Messaging.ServiceBus.Administration;
using Microsoft.Extensions.Logging;
using TddXt.AzureTestResources.Common;

namespace TddXt.AzureTestResources.Messaging.ServiceBus.Queues;

public class ServiceBusTestQueue : IAzureResourceApi
{
  private readonly ServiceBusAdministrationClient _serviceBusClient;
  private readonly ILogger _logger;
  private readonly CancellationToken _cancellationToken;
  public string ConnectionString { get; }
  public string Name { get; }

  public ServiceBusTestQueue(
    ServiceBusAdministrationClient serviceBusClient,
    string queueName,
    string connectionString,
    ILogger logger,
    CancellationToken cancellationToken)
  {
    ConnectionString = connectionString;
    _serviceBusClient = serviceBusClient;
    Name = queueName;
    _logger = logger;
    _cancellationToken = cancellationToken;
  }

  public async ValueTask DisposeAsync()
  {
    _logger.Deleting("queue", Name);
    await _serviceBusClient.DeleteQueueAsync(Name, cancellationToken: _cancellationToken);
    _logger.Deleted("Queue", Name);
  }
}