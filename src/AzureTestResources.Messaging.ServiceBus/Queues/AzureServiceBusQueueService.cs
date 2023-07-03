using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using AzureTestResources.Common;
using Microsoft.Extensions.Logging;

namespace AzureTestResources.Messaging.ServiceBus.Queues;

public class AzureServiceBusQueueService : IAzureService<ServiceBusTestQueue>
{
  private readonly ServiceBusAdministrationClient _serviceBusClient;
  private readonly CancellationToken _cancellationToken;
  private readonly string _namePrefix;
  private readonly string _connectionString;
  private readonly TimeSpan _autoDeleteOnIdle;
  private readonly ILogger _logger;

  public AzureServiceBusQueueService(
    string connectionString,
    string namePrefix,
    ServiceBusAdministrationClient serviceBusClient,
    TimeSpan autoDeleteOnIdle,
    ILogger logger,
    CancellationToken cancellationToken)
  {
    _connectionString = connectionString;
    _namePrefix = namePrefix;
    _cancellationToken = cancellationToken;
    _serviceBusClient = serviceBusClient;
    _autoDeleteOnIdle = autoDeleteOnIdle;
    _logger = logger;
  }

  public async Task<ICreateAzureResourceResponse<ServiceBusTestQueue>> CreateResourceInstance()
  {
    try
    {
      var queueName = TestResourceNamingConvention.GenerateResourceId(_namePrefix);

      _logger.Creating("service bus queue", queueName);

      var sdkResponse = await _serviceBusClient.CreateQueueAsync(
        new CreateQueueOptions(queueName)
        {
          AutoDeleteOnIdle = _autoDeleteOnIdle
        }, _cancellationToken);
      var response = new CreateAzureServiceBusQueueResponse(
        _connectionString,
        _serviceBusClient,
        sdkResponse,
        queueName,
        _logger,
        _cancellationToken);

      return response;
    }
    catch (ServiceBusException ex) when (RetryConditions.RequiresRetry(ex))
    {
      return new ResourceCouldNotBeCreatedResponse<ServiceBusTestQueue>(ex, true);
    }
  }
}