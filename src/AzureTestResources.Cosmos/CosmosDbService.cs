using System.Collections.Immutable;
using System.Net;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using TddXt.AzureTestResources.Common;

namespace TddXt.AzureTestResources.Cosmos;

public class CosmosDbService(
  CosmosClient client,
  CosmosTestDatabaseConfig config,
  ILogger logger,
  CancellationToken cancellationToken)
  : IAzureService<CosmosTestDatabase>
{
  private static readonly IReadOnlyList<HttpStatusCode> CreateResourceRetryCodes =
    new List<HttpStatusCode>
    {
      HttpStatusCode.ServiceUnavailable,
      HttpStatusCode.InternalServerError,
      HttpStatusCode.Conflict
    }.ToImmutableArray();

  public async Task<ICreateAzureResourceResponse<CosmosTestDatabase>> CreateResourceInstance()
  {
    try
    {
      var dbName = TestResourceNamingConvention.GenerateResourceId(config.NamePrefix);
      var databaseResponse =
        await client.CreateDatabaseAsync(
          dbName,
          cancellationToken: cancellationToken);

      return new CreateCosmosDatabaseResponse(
        config,
        logger,
        databaseResponse,
        dbName,
        cancellationToken);
    }
    catch (CosmosException ex) when (CreateResourceRetryCodes.Contains(ex.StatusCode))
    {
      return new ResourceCouldNotBeCreatedResponse<CosmosTestDatabase>(ex, true);
    }
  }
}