using Azure.Messaging.ServiceBus.Administration;
using AzureTestResources.Common;

namespace AzureTestResources.AzureServiceBus.Queues;

public class ServiceBusTestQueue : IAzureResourceApi
{
    private readonly ServiceBusAdministrationClient _serviceBusClient;
    private readonly string _queueName;
    private readonly CancellationToken _cancellationToken;
    public string ConnectionString { get; }
    public string Name => _queueName;

    public ServiceBusTestQueue(
      ServiceBusAdministrationClient serviceBusClient,
      string queueName,
      string connectionString,
      CancellationToken cancellationToken)
    {
        ConnectionString = connectionString;
        _serviceBusClient = serviceBusClient;
        _queueName = queueName;
        _cancellationToken = cancellationToken;
    }

    public async ValueTask DisposeAsync()
    {
        await _serviceBusClient.DeleteQueueAsync(_queueName, cancellationToken: _cancellationToken);
    }

}