using Microsoft.Extensions.Logging;

namespace TddXt.AzureTestResources.Common;

public class ZombieResourceCleanupLoop
{
  private readonly ICreatedResourcesPool _createdResourcesPool;
  private readonly ILogger _logger;
  private readonly TimeSpan _tolerance;

  public ZombieResourceCleanupLoop(ICreatedResourcesPool createdResourcesPool, TimeSpan tolerance, ILogger logger)
  {
    _tolerance = tolerance;
    _logger = logger;
    _createdResourcesPool = createdResourcesPool;
  }

  public async Task Execute()
  {
    //bug parallelize this?
    foreach (var resource in await _createdResourcesPool.LoadResources())
    {
      try
      {
        _logger.LogInformation($"Evaluating {resource.Name} for zombie cleanup.");
        if (TestResourceNamingConvention.IsAZombieResource(_tolerance, resource.Name))
        {
          _logger.LogInformation($"{resource.Name} identified as a zombie resource. Trying to delete...");
          await resource.DeleteAsync();
          _logger.LogInformation($"{resource.Name} deleted successfully.");
        }
        else
        {
          _logger.LogInformation($"{resource.Name} is not a zombie resource. Skipped.");
        }
      }
      catch (ResourceCouldNotBeDeletedException)
      {
        _logger.LogInformation(
          $"{resource.Name} could not be deleted. Most probably was already deleted by another process.");
      }
    }
  }
}