using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;

namespace AzureTestResourcesSpecification;

public static class CosmosClientFactory
{
  public static CosmosClient CreateCosmosClient(CosmosTestDatabaseConfig obj)
  {
    var cosmosClient = new CosmosClientBuilder(
        obj.AccountEndpoint,
        obj.PrimaryKey)
      //.WithBulkExecution(true)
      .WithThrottlingRetryOptions(
        maxRetryAttemptsOnThrottledRequests: obj.MaxRetryAttemptsOnThrottledRequests,
        maxRetryWaitTimeOnThrottledRequests: obj.MaxRetryWaitTimeOnThrottledRequests)
      .WithRequestTimeout(obj.RequestTimeout)
      .Build();
    return cosmosClient;
  }
}