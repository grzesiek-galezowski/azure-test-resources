using Azure;
using Azure.Messaging.ServiceBus.Administration;
using AzureTestResources.Common;
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

        AssertValidResponseTopic(response, topicName);

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

        AssertValidResponseQueue(response, queueName);

        return response;
      });

    return new ServiceBusTestQueue(
      serviceBusClient,
      response.Value.Name,
      connectionString,
      cancellationToken);
  }

  private static void AssertValidResponseQueue(NullableResponse<QueueProperties> response, string resourceName)
  {
    Assertions.AssertIsHttpCreated(response, "queue");
    Assertions.AssertNotNull(response, "queue", resourceName);
    Assertions.AssertNamesMatch(resourceName, response.Value.Name);
  }

  private static void AssertValidResponseTopic(NullableResponse<TopicProperties> response, string topicName)
  {
    Assertions.AssertIsHttpCreated(response, "topic");
    Assertions.AssertNotNull(response, "topic", topicName);
    Assertions.AssertNamesMatch(topicName, response.Value.Name);
  }
}