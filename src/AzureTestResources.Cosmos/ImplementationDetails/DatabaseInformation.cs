using Microsoft.Azure.Cosmos;

namespace TddXt.AzureTestResources.Cosmos.ImplementationDetails;

public static class DatabaseInformation
{
  public static async Task<List<DatabaseProperties>> GetDatabaseList(CosmosClient cosmosClient)
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

  public static async Task<List<ContainerProperties>> GetContainerList(Database database)
  {
    var databases = new List<ContainerProperties>();

    // Create a database iterator
    var databaseIterator = database.GetContainerQueryIterator<ContainerProperties>();

    while (databaseIterator.HasMoreResults)
    {
      var response = await databaseIterator.ReadNextAsync();
      databases.AddRange(response);
    }

    return databases;
  }
}