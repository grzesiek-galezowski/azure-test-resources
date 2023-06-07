using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;

namespace AzureTestResources;

public static class CosmosClientFactory
{
  public static CosmosClient CreateCosmosClient(CosmosTestDatabaseConfig config)
  {
    var cosmosClient = new CosmosClientBuilder(
        config.AccountEndpoint,
        config.PrimaryKey)
      .WithThrottlingRetryOptions(
        maxRetryAttemptsOnThrottledRequests: config.MaxRetryAttemptsOnThrottledRequests,
        maxRetryWaitTimeOnThrottledRequests: config.MaxRetryWaitTimeOnThrottledRequests)
      .WithRequestTimeout(config.RequestTimeout)
      .Build();
    return cosmosClient;
  }
}