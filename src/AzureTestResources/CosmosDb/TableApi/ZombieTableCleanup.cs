using System.Net;
using Azure;
using Azure.Data.Tables;
using AzureTestResources.Common;

namespace AzureTestResources.CosmosDb.TableApi;

public static class ZombieTableCleanup
{
  public static async Task DeleteZombieTables(CosmosTestTableConfig config)
  {
    await DeleteZombieTables(config, AzureResources.DefaultZombieToleranceForEmulator);
  }

  private static async Task DeleteZombieTables(CosmosTestTableConfig config, TimeSpan tolerance)
  {
    var client = new TableServiceClient(config.ConnectionString);
    var tableItems = client.Query().Where(table =>
      TestResourceNamingConvention.IsAZombieResource(tolerance, table.Name)).ToList();

    await Task.WhenAll(tableItems.Select(async table =>
    {
      try
      {
        await client.GetTableClient(table.Name).DeleteAsync();
      }
      catch (RequestFailedException e) when (e.Status == (int)HttpStatusCode.NotFound)
      {

      }
    }));
  }
}