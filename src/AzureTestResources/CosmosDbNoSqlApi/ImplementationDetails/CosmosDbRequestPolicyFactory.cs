using System.Collections.Immutable;
using System.Net;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace AzureTestResources.CosmosDbNoSqlApi.ImplementationDetails;

public static class CosmosDbRequestPolicyFactory
{
  private static readonly IReadOnlyList<HttpStatusCode> CreateSubResourceRetryCodes =
    new List<HttpStatusCode>
    {
      HttpStatusCode.ServiceUnavailable,
      HttpStatusCode.InternalServerError
    }.ToImmutableArray();

  public static AsyncRetryPolicy CreateCreateSubResourcePolicy(ILogger logger)
  {
    return CreateDefaultPolicy(logger, CreateSubResourceRetryCodes);
  }

  //bug replace with common policy
  private static AsyncRetryPolicy CreateDefaultPolicy(ILogger logger,
    IReadOnlyList<HttpStatusCode> createResourceRetryCodes)
  {
    return Policy.Handle<CosmosException>(
        e => createResourceRetryCodes.Any(c => c == e.StatusCode))
      .WaitAndRetryAsync(10,
        _ => TimeSpan.FromSeconds(5),
        (exception, _, retryCount, _) =>
        {
          logger.LogWarning($"Retry {retryCount} due to status code {((CosmosException)exception).StatusCode}");
        });
  }
}