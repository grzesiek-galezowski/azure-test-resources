using AzureTestResources.Common;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace AzureTestResources.CosmosDb.NoSqlApi.ImplementationDetails;

public class CreatedDatabasesPool : ICreatedResourcesPool
{
  private readonly CosmosClient _cosmosClient;
  private readonly ILogger _logger;

  public CreatedDatabasesPool(CosmosClient cosmosClient, ILogger logger)
  {
    _cosmosClient = cosmosClient;
    _logger = logger;
  }

  public async Task<IEnumerable<ICreatedResource>> LoadResources()
  {
    return (await DatabaseInformation.GetDatabaseList(_cosmosClient))
      .Select(x => new CreatedDatabase(x, _cosmosClient, _logger));
  }
}