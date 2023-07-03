using Azure.Messaging.ServiceBus;

namespace AzureTestResources.Messaging.ServiceBus;

internal static class RetryConditions
{
  public static bool RequiresRetry(ServiceBusException ex)
  {
    return ex.Reason == ServiceBusFailureReason.MessagingEntityAlreadyExists
           || ex.Message.Contains("SubCode=40901.");
  }
}