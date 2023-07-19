using Azure.Messaging.ServiceBus.Administration;
using Microsoft.Extensions.Logging;
using TddXt.AzureTestResources.Common;

namespace TddXt.AzureTestResources.Messaging.ServiceBus.Topics;

public class ServiceBusTestTopic : IAzureResourceApi
{
  private readonly ServiceBusAdministrationClient _serviceBusClient;
  private readonly CancellationToken _cancellationToken;
  private readonly ILogger _logger;
  public string ConnectionString { get; }
  public string Name { get; }

  public ServiceBusTestTopic(
    ServiceBusAdministrationClient serviceBusClient,
    string topicName,
    string connectionString,
    ILogger logger,
    CancellationToken cancellationToken)
  {
    ConnectionString = connectionString;
    _serviceBusClient = serviceBusClient;
    Name = topicName;
    _cancellationToken = cancellationToken;
    _logger = logger;
  }

  public async Task CreateSubscription(string name)
  {
    var response = await _serviceBusClient.CreateSubscriptionAsync(
      Name,
      cancellationToken: _cancellationToken,
      subscriptionName: name);

    if (!response.HasValue)
    {
      throw new InvalidOperationException("Could not create a subscription " + name);
    }

    if (Name != response.Value.TopicName)
    {
      throw new InvalidOperationException("Naming mismatch");
    }
  }

  public async ValueTask DisposeAsync()
  {
    _logger.Deleting("topic", Name);
    await _serviceBusClient.DeleteTopicAsync(Name, cancellationToken: _cancellationToken);
    _logger.Deleted("topic", Name);
  }
}