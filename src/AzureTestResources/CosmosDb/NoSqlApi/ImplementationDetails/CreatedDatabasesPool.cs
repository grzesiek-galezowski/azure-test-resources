using AzureTestResources.Common;
using Microsoft.Azure.Cosmos;

namespace AzureTestResources.CosmosDb.NoSqlApi.ImplementationDetails;

public class CreatedDatabasesPool : ICreatedResourcesPool
{
  private readonly CosmosClient _cosmosClient;

  public CreatedDatabasesPool(CosmosClient cosmosClient)
  {
    _cosmosClient = cosmosClient;
  }

  public async Task<IEnumerable<ICreatedResource>> LoadResources()
  {
    return (await DatabaseInformation.GetDatabaseList(_cosmosClient))
      .Select(x => new CreatedDatabase(x, _cosmosClient));
  }
}