using Microsoft.Extensions.Logging;
using TddXt.AzureTestResources.Common;
using TddXt.AzureTestResources.Cosmos.ImplementationDetails;

namespace TddXt.AzureTestResources.Cosmos;

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
    using var cosmosClient = CosmosClientFactory.CreateCosmosClient(config);
    var cosmosDbService = new CosmosDbService(
      cosmosClient,
      config,
      logger,
      new CancellationToken());

    var cosmosTestDatabase = await AzureResources.CreateApiToUnderlyingResource(
      cosmosDbService,
      "database",
      logger);

    return cosmosTestDatabase;
  }
}