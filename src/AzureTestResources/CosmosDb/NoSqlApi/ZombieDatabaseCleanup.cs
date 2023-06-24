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
        Console.WriteLine($"Clearing database {databaseProperties.Id}"); //bug
        var database = client.GetDatabase(databaseProperties.Id);
        await DeleteContainers(database, client);
        await database.DeleteAsync();
      }
      catch (CosmosException e) when (e.StatusCode == HttpStatusCode.NotFound)
      {
        //silently ignore these databases
      }
    }
  }

  /// <summary>
  /// The whole point of deleting the containers first is that the cosmos db emulator
  /// has a limit on container count, so we're trying to trim this as soon as possible.
  /// </summary>
  private static async Task DeleteContainers(Database database, CosmosClient client)
  {
    var containerPropertiesList = await DatabaseInformation.GetContainerList(database);
    Console.WriteLine($"Database has {containerPropertiesList.Count} containers"); //bug
    foreach (var containerProperties in containerPropertiesList)
    {
      try
      {
        Console.WriteLine($"Deleting container {containerProperties.Id}"); //bug
        var container = client.GetContainer(database.Id, containerProperties.Id);
        await container.DeleteContainerAsync();
        Console.WriteLine($"Deleted container {containerProperties.Id}"); //bug
      }
      catch (CosmosException e) when (e.StatusCode == HttpStatusCode.NotFound)
      {
        //silently ignore these containers
        Console.WriteLine($"Container {containerProperties.Id} already deleted"); //bug
      }
    }
  }
}