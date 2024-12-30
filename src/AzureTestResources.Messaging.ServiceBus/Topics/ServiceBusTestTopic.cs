using Azure.Messaging.ServiceBus.Administration;
using Microsoft.Extensions.Logging;
using TddXt.AzureTestResources.Common;

namespace TddXt.AzureTestResources.Messaging.ServiceBus.Topics;

public class ServiceBusTestTopic(
  ServiceBusAdministrationClient serviceBusClient,
  string topicName,
  string connectionString,
  ILogger logger,
  CancellationToken cancellationToken)
  : IAzureResourceApi
{
  public string ConnectionString { get; } = connectionString;
  public string Name { get; } = topicName;

  public async Task CreateSubscription(string name)
  {
    var response = await serviceBusClient.CreateSubscriptionAsync(
      Name,
      cancellationToken: cancellationToken,
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
    logger.Deleting("topic", Name);
    await serviceBusClient.DeleteTopicAsync(Name, cancellationToken: cancellationToken);
    logger.Deleted("topic", Name);
  }
}