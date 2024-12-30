using Microsoft.Extensions.Logging;

namespace TddXt.AzureTestResources.Common;

public class ZombieResourceCleanupLoop(ICreatedResourcesPool createdResourcesPool, TimeSpan tolerance, ILogger logger)
{
  public async Task Execute()
  {
    //bug parallelize this?
    foreach (var resource in await createdResourcesPool.LoadResources())
    {
      try
      {
        logger.LogInformation($"Evaluating {resource.Name} for zombie cleanup.");
        if (TestResourceNamingConvention.IsAZombieResource(tolerance, resource.Name))
        {
          logger.LogInformation($"{resource.Name} identified as a zombie resource. Trying to delete...");
          await resource.DeleteAsync();
          logger.LogInformation($"{resource.Name} deleted successfully.");
        }
        else
        {
          logger.LogInformation($"{resource.Name} is not a zombie resource. Skipped.");
        }
      }
      catch (ResourceCouldNotBeDeletedException)
      {
        logger.LogInformation(
          $"{resource.Name} could not be deleted. Most probably was already deleted by another process.");
      }
    }
  }
}