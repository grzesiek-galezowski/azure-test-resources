using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using TddXt.AzureTestResources.Common;

namespace TddXt.AzureTestResources.Cosmos.ImplementationDetails;

public class CreatedDatabasesPool(CosmosClient cosmosClient, ILogger logger) : ICreatedResourcesPool
{
  public async Task<IEnumerable<ICreatedResource>> LoadResources()
  {
    return (await DatabaseInformation.GetDatabaseList(cosmosClient))
      .Select(x => new CreatedDatabase(x, cosmosClient, logger));
  }
}