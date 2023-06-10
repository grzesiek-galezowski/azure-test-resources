using System.Net;
using AzureTestResources.CosmosDb;
using Microsoft.Azure.Cosmos;

namespace AzureTestResources;

public static class ZombieDatabaseCleanup
{
  private static readonly TimeSpan DefaultTolerance = TimeSpan.FromMinutes(2);

  public static async Task DeleteZombieDatabases()
  {
    await DeleteZombieDatabases(DefaultTolerance);
  }

  private static async Task DeleteZombieDatabases(TimeSpan tolerance)
  {
    using var client = CosmosClientFactory.CreateCosmosClient(
      CosmosTestDatabaseConfig.Default());

    var databaseList = await GetDatabaseList(client);

    foreach (var databaseProperties in databaseList
               .Where(TestDbNamingConvention.AdheresToNamingConvention)
               .Where(properties => TestDbNamingConvention.IsCreatedEarlierFromNowThan(
                 tolerance,
                 properties)))
    {
      try
      {
        //bug make this more efficient?
        var database = client.GetDatabase(databaseProperties.Id);
        await database.DeleteAsync();
      }
      catch (CosmosException e) when (e.StatusCode == HttpStatusCode.NotFound)
      {
        //silently ignore these databases
      }
    }
  }

  private static async Task<List<DatabaseProperties>> GetDatabaseList(CosmosClient cosmosClient)
  {
    var databases = new List<DatabaseProperties>();

    // Create a database iterator
    var databaseIterator = cosmosClient.GetDatabaseQueryIterator<DatabaseProperties>();

    while (databaseIterator.HasMoreResults)
    {
      var response = await databaseIterator.ReadNextAsync();
      databases.AddRange(response);
    }

    return databases;
  }
}