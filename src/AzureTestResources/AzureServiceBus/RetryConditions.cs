using Azure.Messaging.ServiceBus;

namespace AzureTestResources.AzureServiceBus;

internal static class RetryConditions
{
  public static bool RequiresRetry(ServiceBusException ex)
  {
    return ex.Reason == ServiceBusFailureReason.MessagingEntityAlreadyExists
           || ex.Message.Contains("SubCode=40901.");
  }
}