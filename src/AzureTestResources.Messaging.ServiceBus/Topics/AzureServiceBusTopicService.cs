using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using AzureTestResources.Common;
using Microsoft.Extensions.Logging;

namespace AzureTestResources.Messaging.ServiceBus.Topics;

public class AzureServiceBusTopicService : IAzureService<ServiceBusTestTopic>
{
  private readonly ServiceBusAdministrationClient _serviceBusClient;
  private readonly CancellationToken _cancellationToken;
  private readonly string _namePrefix;
  private readonly string _connectionString;
  private readonly TimeSpan _autoDeleteOnIdle;
  private readonly ILogger _logger;

  public AzureServiceBusTopicService(
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

  public async Task<ICreateAzureResourceResponse<ServiceBusTestTopic>> CreateResourceInstance()
  {
    try
    {
      var topicName = TestResourceNamingConvention.GenerateResourceId(_namePrefix);
      _logger.Creating("topic", topicName);
      var sdkResponse = await _serviceBusClient.CreateTopicAsync(
        new CreateTopicOptions(topicName)
        {
          AutoDeleteOnIdle = _autoDeleteOnIdle
        }, _cancellationToken);
      var response = new CreateAzureServiceBusTopicResponse(
        _connectionString,
        _serviceBusClient,
        sdkResponse,
        topicName,
        _logger,
        _cancellationToken);
      return response;
    }
    catch (ServiceBusException ex) when (RetryConditions.RequiresRetry(ex))
    {
      return new ResourceCouldNotBeCreatedResponse<ServiceBusTestTopic>(ex, true);
    }
  }
}