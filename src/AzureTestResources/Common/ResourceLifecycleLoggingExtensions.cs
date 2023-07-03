using Microsoft.Extensions.Logging;

namespace AzureTestResources.Common;

public static class ResourceLifecycleLoggingExtensions
{
  public static void Deleted(this ILogger logger, string resourceType, string resourceName)
  {
    logger.LogInformation($"{resourceType} {resourceName} deleted successfully");
  }

  public static void Deleting(this ILogger logger, string resourceType, string resourceName)
  {
    logger.LogInformation($"Deleting {resourceType} {resourceName}");
  }

  public static void Created(this ILogger logger, string resourceType, string resourceName)
  {
    logger.LogInformation($"{resourceType} {resourceName} created successfully.");
  }

  public static void Creating(this ILogger logger, string resourceType, string resourceName)
  {
    logger.LogInformation($"Attempting to create a {resourceType} {resourceName}");
  }
}