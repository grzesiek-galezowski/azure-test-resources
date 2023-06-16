using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace AzureTestResources.AzureServiceBus;

public static class AzureServiceBusRequestPolicyFactory
{
  public static AsyncRetryPolicy CreateCreateResourcePolicy(ILogger logger)
  {
    return Policy.Handle<ServiceBusException>(
        e => e.Reason == ServiceBusFailureReason.MessagingEntityAlreadyExists)
      .Or<ServiceBusException>(
        e => e.Message.Contains("SubCode=40901."))
      .WaitAndRetryAsync(30,
        _ => TimeSpan.FromSeconds(1),
        (exception, _, retryCount, _) =>
        {
          logger.LogWarning(
            $"Retry {retryCount} due to status code {((ServiceBusException)exception).Reason} and message {((ServiceBusException)exception).Message}");
        });
  }
}