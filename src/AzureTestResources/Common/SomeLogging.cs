using Microsoft.Extensions.Logging;

namespace AzureTestResources.Common;

//bug promote higher in directories
public static class SomeLogging //bug change the name
{
    public static string Deleted(string resourceType, string resourceName)
    {
        return $"{resourceType} {resourceName} deleted successfully";
    }

    public static string Deleting(string resourceType, string resourceName)
    {
        return $"Deleting {resourceType} {resourceName}";
    }

    public static string Created(string resourceType, string queueName)
    {
        return resourceType + $" {queueName} created successfully.";
    }

    public static string Creating(string resourceType, string resourceName)
    {
        return $"Attempting to create a " + resourceType + $" {resourceName}";
    }

    public static void Deleted(this ILogger logger, string resourceType, string resourceName)
    {
        logger.LogInformation(Deleted(resourceType, resourceName));
    }

    public static void Deleting(this ILogger logger, string resourceType, string resourceName)
    {
        logger.LogInformation(Deleting(resourceType, resourceName));
    }

    public static void Created(this ILogger logger, string resourceType, string resourceName)
    {
        logger.LogInformation(Created(resourceType, resourceName));
    }

    public static void Creating(this ILogger logger, string resourceType, string resourceName)
    {
        logger.LogInformation(Creating(resourceType, resourceName));
    }


}