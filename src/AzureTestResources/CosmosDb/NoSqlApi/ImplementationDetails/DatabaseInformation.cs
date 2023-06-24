using Microsoft.Azure.Cosmos;

namespace AzureTestResources.CosmosDb.NoSqlApi.ImplementationDetails;

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
}