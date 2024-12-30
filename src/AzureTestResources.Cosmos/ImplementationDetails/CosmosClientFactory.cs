using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;

namespace TddXt.AzureTestResources.Cosmos.ImplementationDetails;

public static class CosmosClientFactory
{
  public static CosmosClient CreateCosmosClient(CosmosTestDatabaseConfig config)
  {
    var cosmosClient = new CosmosClientBuilder(config.ConnectionString)
      .WithThrottlingRetryOptions(
        maxRetryAttemptsOnThrottledRequests: config.MaxRetryAttemptsOnThrottledRequests,
        maxRetryWaitTimeOnThrottledRequests: config.MaxRetryWaitTimeOnThrottledRequests)
      .WithRequestTimeout(config.RequestTimeout)
      .WithHttpClientFactory(() =>
      {
          HttpMessageHandler httpMessageHandler = new HttpClientHandler
          {
            ServerCertificateCustomValidationCallback = (req, cert, chain, errors) => true
          };
          return new HttpClient(httpMessageHandler);
      })
      .WithConnectionModeGateway()
      .Build();
    return cosmosClient;
  }
}