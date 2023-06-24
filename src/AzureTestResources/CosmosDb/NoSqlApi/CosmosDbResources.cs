using AzureTestResources.Common;
using AzureTestResources.CosmosDb.NoSqlApi.ImplementationDetails;
using Microsoft.Extensions.Logging;

namespace AzureTestResources.CosmosDb.NoSqlApi;

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
          logger);

        return cosmosTestDatabase;
    }
}