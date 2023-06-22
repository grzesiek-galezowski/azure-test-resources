using System.Net;
using Azure;
using Azure.Data.Tables;

namespace AzureTestResources.CosmosDbTableApi;

public static class ZombieTableCleanup
{
  private static readonly TimeSpan DefaultTolerance = TimeSpan.FromMinutes(1);

  public static async Task DeleteZombieTables(CosmosTestTableConfig config)
  {
    await DeleteZombieTables(config, DefaultTolerance);
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