using Microsoft.Extensions.Logging;
using TddXt.AzureTestResources.Common;
using TddXt.AzureTestResources.Cosmos.ImplementationDetails;

namespace TddXt.AzureTestResources.Cosmos;

public static class CosmosDbResources
{
  //bug the cosmos db clients are not disposed. They need to be disposed in 
  //both cases: error creating a database and success.
  //One idea is to make this an object, sort of repository and make it own the cosmos client

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
      "database",
      logger);

    return cosmosTestDatabase;
  }
}