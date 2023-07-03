using System.Net;
using Azure;
using Azure.Data.Tables;
using AzureTestResources.Common;
using Microsoft.Extensions.Logging;

namespace AzureTestResources.Data.Tables;

public static class ZombieTableCleanup
{
  public static async Task DeleteZombieTables(CosmosTestTableConfig config, ILogger logger)
  {
    await DeleteZombieTables(config, AzureResources.DefaultZombieToleranceForEmulator, logger);
  }

  /// <summary>
  /// This removal is slightly different because tables, at least on an emulator, take forever to delete.
  /// </summary>
  /// <param name="config"></param>
  /// <param name="tolerance"></param>
  /// <param name="logger"></param>
  /// <returns></returns>
  private static async Task DeleteZombieTables(CosmosTestTableConfig config, TimeSpan tolerance, ILogger logger)
  {
    var client = new TableServiceClient(config.ConnectionString);
    var tableItems = client.Query().Where(table =>
      TestResourceNamingConvention.IsAZombieResource(tolerance, table.Name)).ToList();

    await Task.WhenAll(tableItems.Select(async table =>
    {
      try
      {
        logger.LogInformation($"{table.Name} identified as a zombie resource. Trying to delete...");
        await client.GetTableClient(table.Name).DeleteAsync();
        logger.LogInformation($"{table.Name} deleted successfully.");
      }
      catch (RequestFailedException e) when (e.Status == (int)HttpStatusCode.NotFound)
      {
        logger.LogInformation(
          $"{table.Name} could not be deleted. Most probably was already deleted by another process.");
      }
    }));
  }
}