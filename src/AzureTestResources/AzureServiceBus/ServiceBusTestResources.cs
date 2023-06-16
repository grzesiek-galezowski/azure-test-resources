using Azure;
using Azure.Messaging.ServiceBus.Administration;
using Microsoft.Extensions.Logging;

namespace AzureTestResources.AzureServiceBus;

public static class ServiceBusTestResources
{
  private static readonly TimeSpan AutoDeleteOnIdle = TimeSpan.FromMinutes(5);

  public static async Task<ServiceBusTestTopic> CreateTopic(
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
            AutoDeleteOnIdle = AutoDeleteOnIdle,
          }, cancellationToken);

        AssertValidResponse(response, topicName);

        return response;
      });

    return new ServiceBusTestTopic(
      serviceBusClient, 
      response.Value.Name, 
      connectionString,
      cancellationToken);
  }

  public static async Task<ServiceBusTestQueue> CreateQueue(
    string connectionString, 
    string namePrefix,
    ILogger logger,
    CancellationToken cancellationToken)
  {
    var serviceBusClient = new ServiceBusAdministrationClient(connectionString);

    var response = await AzureServiceBusRequestPolicyFactory.CreateCreateResourcePolicy(logger)
      .ExecuteAsync(async () =>
      {
        var queueName = TestResourceNamingConvention.GenerateResourceId(namePrefix);
        var response = await serviceBusClient.CreateQueueAsync(
          new CreateQueueOptions(queueName)
          {
            AutoDeleteOnIdle = AutoDeleteOnIdle,
          }, cancellationToken);

        AssertValidResponse(response, queueName);

        return response;
      });

    return new ServiceBusTestQueue(
      serviceBusClient, 
      response.Value.Name, 
      connectionString,
      cancellationToken);
  }

  private static void AssertValidResponse(NullableResponse<QueueProperties> response, string resourceName)
  {
    if (!response.HasValue)
    {
      throw new InvalidOperationException("Could not create a subscription " + resourceName);
    }

    if (resourceName != response.Value.Name)
    {
      throw new InvalidOperationException("Naming mismatch");
    }
  }

  private static void AssertValidResponse(NullableResponse<TopicProperties> response, string topicName)
  {
    if (!response.HasValue)
    {
      throw new InvalidOperationException("Could not create a subscription " + topicName);
    }

    if (topicName != response.Value.Name)
    {
      throw new InvalidOperationException("Naming mismatch");
    }
  }
}