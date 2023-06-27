using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using AzureTestResources.Common;

namespace AzureTestResources.AzureServiceBus.Topics;

public class AzureServiceBusTopicService : IAzureService<ServiceBusTestTopic>
{
  private readonly ServiceBusAdministrationClient _serviceBusClient;
  private readonly CancellationToken _cancellationToken;
  private readonly string _namePrefix;
  private readonly string _connectionString;
  private readonly TimeSpan _autoDeleteOnIdle;

  public AzureServiceBusTopicService(
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

  public async Task<ICreateAzureResourceResponse<ServiceBusTestTopic>> CreateResourceInstance()
  {
    try
    {
      var topicName = TestResourceNamingConvention.GenerateResourceId(_namePrefix);
      var response = new CreateAzureServiceBusTopicResponse(
        _connectionString,
        _cancellationToken,
        _serviceBusClient,
        await _serviceBusClient.CreateTopicAsync(
          new CreateTopicOptions(topicName)
          {
            AutoDeleteOnIdle = _autoDeleteOnIdle
          }, _cancellationToken),
        topicName);
      return response;
    }
    catch (ServiceBusException ex) when (RetryConditions.RequiresRetry(ex))
    {
      return new ResourceCouldNotBeCreatedResponse<ServiceBusTestTopic>(ex, true);
    }
  }
}