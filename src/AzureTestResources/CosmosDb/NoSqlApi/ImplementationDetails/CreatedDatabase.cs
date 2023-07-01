using System.Net;
using AzureTestResources.Common;
using Microsoft.Azure.Cosmos;

namespace AzureTestResources.CosmosDb.NoSqlApi.ImplementationDetails;

public class CreatedDatabase : ICreatedResource
{
  private readonly DatabaseProperties _databaseProperties;
  private readonly CosmosClient _cosmosClient;

  public CreatedDatabase(DatabaseProperties databaseProperties, CosmosClient cosmosClient)
  {
    _databaseProperties = databaseProperties;
    _cosmosClient = cosmosClient;
  }

  public string Name => _databaseProperties.Id;
  public async Task DeleteAsync()
  {
    try
    {
      var database = _cosmosClient.GetDatabase(_databaseProperties.Id);
      await DeleteContainers(database, _cosmosClient);
      await database.DeleteAsync();
    }
    catch (CosmosException e) when (e.StatusCode == HttpStatusCode.NotFound)
    {
      throw new ResourceCouldNotBeDeletedException(e);
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