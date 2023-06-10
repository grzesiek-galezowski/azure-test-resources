using System.Collections.Immutable;
using System.Net;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace AzureTestResources.CosmosDb;

public static class ResourceRequestPolicyFactory
{
    private static readonly IReadOnlyList<HttpStatusCode> CreateDatabaseRetryCodes =
      new List<HttpStatusCode>
      {
      HttpStatusCode.ServiceUnavailable,
      HttpStatusCode.InternalServerError,
      HttpStatusCode.Conflict
      }.ToImmutableArray();

    private static readonly IReadOnlyList<HttpStatusCode> CreateContainerRetryCodes =
      new List<HttpStatusCode>
      {
      HttpStatusCode.ServiceUnavailable,
      HttpStatusCode.InternalServerError
      }.ToImmutableArray();

    public static AsyncRetryPolicy CreateCreateDatabasePolicy(ILogger logger)
    {
        return CreateCreateResourcePolicy(logger, CreateDatabaseRetryCodes);
    }

    public static AsyncRetryPolicy CreateCreateContainerPolicy(ILogger logger)
    {
        return CreateCreateResourcePolicy(logger, CreateContainerRetryCodes);
    }

    private static AsyncRetryPolicy CreateCreateResourcePolicy(ILogger logger,
      IReadOnlyList<HttpStatusCode> createResourceRetryCodes)
    {
        return Policy.Handle<CosmosException>(
            e => createResourceRetryCodes.Any(c => c == e.StatusCode))
          .WaitAndRetryAsync(5,
            _ => TimeSpan.FromSeconds(10),
            (exception, _, retryCount, _) =>
            {
                logger.LogWarning($"Retry {retryCount} due to status code {((CosmosException)exception).StatusCode}");
            });
    }
}