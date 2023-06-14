using Azure.Messaging.ServiceBus.Administration;
using Microsoft.Extensions.Logging;

namespace AzureTestResources.AzureServiceBusTopics;

public class ServiceBusTestTopic : IAsyncDisposable
{
  private readonly ServiceBusAdministrationClient _serviceBusClient;
  private readonly string _topicName;
  private readonly CancellationToken _cancellationToken;
  public string ConnectionString { get; }
  public string Name => _topicName;

  public static async Task<ServiceBusTestTopic> Create(
    string connectionString, 
    string namePrefix,
    ILogger logger,
    CancellationToken cancellationToken)
  {
    var serviceBusClient = new ServiceBusAdministrationClient(connectionString);

    var response = await AzureServiceBusRequestPolicyFactory.CreateCreateResourcePolicy(logger)
      .ExecuteAsync(async () =>
      {
        var topicName = TestResourceNamingConvention.GenerateResourceId(namePrefix);
        var response = await serviceBusClient.CreateTopicAsync(
          new CreateTopicOptions(topicName)
          {
            AutoDeleteOnIdle = TimeSpan.FromMinutes(5),
          }, cancellationToken);

        if (!response.HasValue)
        {
          throw new InvalidOperationException("Could not create a subscription " + topicName);
        }

        if (topicName != response.Value.Name)
        {
          throw new InvalidOperationException("Naming mismatch");
        }

        return response;
      });

    return new ServiceBusTestTopic(
      serviceBusClient, 
      response.Value.Name, 
      connectionString,
      cancellationToken);
  }


  private ServiceBusTestTopic(ServiceBusAdministrationClient serviceBusClient,
    string topicName,
    string connectionString,
    CancellationToken cancellationToken)
  {
    ConnectionString = connectionString;
    _serviceBusClient = serviceBusClient;
    _topicName = topicName;
    _cancellationToken = cancellationToken;
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
    await _serviceBusClient.DeleteTopicAsync(_topicName, cancellationToken: _cancellationToken);
  }
}