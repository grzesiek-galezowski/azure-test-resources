using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using AzureTestResources.Common;

namespace AzureTestResources.AzureServiceBus.Queues;

public class AzureServiceBusQueueService : IAzureService<ServiceBusTestQueue>
{
  private readonly ServiceBusAdministrationClient _serviceBusClient;
  private readonly CancellationToken _cancellationToken;
  private readonly string _namePrefix;
  private readonly string _connectionString;
  private readonly TimeSpan _autoDeleteOnIdle;

  public AzureServiceBusQueueService(
    string connectionString,
    string namePrefix,
    CancellationToken cancellationToken,
    ServiceBusAdministrationClient serviceBusClient,
    TimeSpan autoDeleteOnIdle)
  {
    _connectionString = connectionString;
    _namePrefix = namePrefix;
    _cancellationToken = cancellationToken;
    _serviceBusClient = serviceBusClient;
    _autoDeleteOnIdle = autoDeleteOnIdle;
  }

  public async Task<ICreateAzureResourceResponse<ServiceBusTestQueue>> CreateResourceInstance()
  {
    try
    {
      var topicName = TestResourceNamingConvention.GenerateResourceId(_namePrefix);
      var response = new CreateAzureServiceBusQueueResponse(
        _connectionString,
        _cancellationToken,
        _serviceBusClient,
        await _serviceBusClient.CreateQueueAsync(
          new CreateQueueOptions(topicName)
          {
            AutoDeleteOnIdle = _autoDeleteOnIdle,
          }, _cancellationToken),
        topicName);
      return response;
    }
    catch (ServiceBusException ex) when (RetryConditions.RequiresRetry(ex))
    {
      return new ResourceCouldNotBeCreatedResponse<ServiceBusTestQueue>(ex, true);
    }
  }
}