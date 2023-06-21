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
    var client = CosmosClientFactory.CreateCosmosClient(config);
    var cancellationToken = new CancellationToken();

    var databaseResponse =
      await CosmosDbRequestPolicyFactory.CreateCreateResourcePolicy(logger).ExecuteAsync(() =>
        client.CreateDatabaseAsync(TestResourceNamingConvention.GenerateResourceId(config.NamePrefix),
          cancellationToken: cancellationToken)
      );

    return new CosmosTestDatabase(
      databaseResponse.Database,
      logger,
      CosmosDbRequestPolicyFactory.CreateCreateSubResourcePolicy(logger),
      config.ConnectionString,
      cancellationToken);
  }
}