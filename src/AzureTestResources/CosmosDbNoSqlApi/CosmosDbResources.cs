using AzureTestResources.AzureStorage.Common;
using AzureTestResources.CosmosDbNoSqlApi.ImplementationDetails;
using Microsoft.Extensions.Logging;

namespace AzureTestResources.CosmosDbNoSqlApi;

public static class CosmosDbResources
{
  public static async Task<CosmosTestDatabase> CreateDatabase(ILogger log)
  {
    var config = CosmosTestDatabaseConfig.Default();

    return await CreateDatabase(config, log);
  }

  public static async Task<CosmosTestDatabase> CreateDatabase(
    CosmosTestDatabaseConfig config,
    ILogger logger)
  {
    var cosmosDbService = new CosmosDbService(
      CosmosClientFactory.CreateCosmosClient(config), 
      config, 
      logger, 
      new CancellationToken());

    var cosmosTestDatabase = await AzureResources.CreateApiToUnderlyingResource(
      cosmosDbService, 
      logger);

    return cosmosTestDatabase;
  }
}