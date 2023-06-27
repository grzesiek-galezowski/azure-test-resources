using Azure.Messaging.ServiceBus.Administration;
using AzureTestResources.Common;
using Microsoft.Extensions.Logging;

namespace AzureTestResources.AzureServiceBus.Topics;

public class ServiceBusTestTopic : IAzureResourceApi
{
  private readonly ServiceBusAdministrationClient _serviceBusClient;
  private readonly string _topicName;
  private readonly CancellationToken _cancellationToken;
  private readonly ILogger _logger;
  public string ConnectionString { get; }
  public string Name => _topicName;

  public ServiceBusTestTopic(
    ServiceBusAdministrationClient serviceBusClient,
    string topicName,
    string connectionString,
    ILogger logger,
    CancellationToken cancellationToken)
  {
    ConnectionString = connectionString;
    _serviceBusClient = serviceBusClient;
    _topicName = topicName;
    _cancellationToken = cancellationToken;
    _logger = logger;
  }

  public async Task CreateSubscription(string name)
  {
    var response = await _serviceBusClient.CreateSubscriptionAsync(
      _topicName,
      cancellationToken: _cancellationToken,
      subscriptionName: name);

    if (!response.HasValue)
    {
      throw new InvalidOperationException("Could not create a subscription " + name);
    }

    if (_topicName != response.Value.TopicName)
    {
      throw new InvalidOperationException("Naming mismatch");
    }
  }

  public async ValueTask DisposeAsync()
  {
    _logger.Deleting("topic", Name);
    await _serviceBusClient.DeleteTopicAsync(_topicName, cancellationToken: _cancellationToken);
    _logger.Deleted("topic", Name);
  }
}