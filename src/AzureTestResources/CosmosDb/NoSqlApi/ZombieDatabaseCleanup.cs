using System.Net;
using AzureTestResources.Common;
using AzureTestResources.CosmosDb.NoSqlApi.ImplementationDetails;
using Microsoft.Azure.Cosmos;

namespace AzureTestResources.CosmosDb.NoSqlApi;

public static class ZombieDatabaseCleanup
{
  private static readonly TimeSpan DefaultTolerance = TimeSpan.FromMinutes(1);

  public static async Task DeleteZombieDatabases(CosmosTestDatabaseConfig config)
  {
    await DeleteZombieDatabases(config, DefaultTolerance);
  }

  private static async Task DeleteZombieDatabases(CosmosTestDatabaseConfig config, TimeSpan tolerance)
  {
    using var client = CosmosClientFactory.CreateCosmosClient(config);

    var databaseList = await DatabaseInformation.GetDatabaseList(client);

    foreach (var databaseProperties in databaseList.Where(properties =>
               TestResourceNamingConvention.IsAZombieResource(tolerance, properties.Id)))
    {
      try
      {
        //bug make this more efficient?
        //bug delete containers first?
        var database = client.GetDatabase(databaseProperties.Id);
        await database.DeleteAsync();
      }
      catch (CosmosException e) when (e.StatusCode == HttpStatusCode.NotFound)
      {
        //silently ignore these databases
      }
    }
  }
}