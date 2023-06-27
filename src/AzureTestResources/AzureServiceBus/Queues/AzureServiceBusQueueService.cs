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
    ServiceBusAdministrationClient serviceBusClient,
    TimeSpan autoDeleteOnIdle,
    CancellationToken cancellationToken)
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
      var queueName = TestResourceNamingConvention.GenerateResourceId(_namePrefix);

      Console.WriteLine(SomeLogging.Creating($"service bus queue", queueName)); //bug
      var sdkResponse = await _serviceBusClient.CreateQueueAsync(
        new CreateQueueOptions(queueName)
        {
          AutoDeleteOnIdle = _autoDeleteOnIdle
        }, _cancellationToken);
      var response = new CreateAzureServiceBusQueueResponse(
        _connectionString,
        _cancellationToken,
        _serviceBusClient,
        sdkResponse,
        queueName);
      Console.WriteLine(SomeLogging.Created("Queue", queueName)); //bug
      return response;
    }
    catch (ServiceBusException ex) when (RetryConditions.RequiresRetry(ex))
    {
      return new ResourceCouldNotBeCreatedResponse<ServiceBusTestQueue>(ex, true);
    }
  }
}