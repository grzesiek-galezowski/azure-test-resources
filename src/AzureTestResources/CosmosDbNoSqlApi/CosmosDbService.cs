using System.Collections.Immutable;
using System.Net;
using AzureTestResources.AzureStorage.Common;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace AzureTestResources.CosmosDbNoSqlApi;

public class CosmosDbService : IAzureService<CosmosTestDatabase>
{
  private static readonly IReadOnlyList<HttpStatusCode> CreateResourceRetryCodes =
    new List<HttpStatusCode>
    {
      HttpStatusCode.ServiceUnavailable,
      HttpStatusCode.InternalServerError,
      HttpStatusCode.Conflict
    }.ToImmutableArray();

  private readonly CosmosClient _client;
  private readonly CosmosTestDatabaseConfig _config;
  private readonly ILogger _logger;
  private readonly CancellationToken _cancellationToken;

  public CosmosDbService(
    CosmosClient client,
    CosmosTestDatabaseConfig config,
    ILogger logger,
    CancellationToken cancellationToken)
  {
    _client = client;
    _config = config;
    _logger = logger;
    _cancellationToken = cancellationToken;
  }

  public async Task<ICreateAzureResourceResponse<CosmosTestDatabase>> CreateResourceInstance()
  {
    try
    {
      var databaseResponse =
        await _client.CreateDatabaseAsync(
          TestResourceNamingConvention.GenerateResourceId(_config.NamePrefix),
          cancellationToken: _cancellationToken);

      return new CreateCosmosDatabaseResponse(_config, _logger, databaseResponse, _cancellationToken);
    }
    catch (CosmosException ex) when (CreateResourceRetryCodes.Contains(ex.StatusCode))
    {
      return new ResourceCouldNotBeCreatedResponse<CosmosTestDatabase>(ex, true);
    }
  }
}