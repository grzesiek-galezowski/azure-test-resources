using System.Collections.Immutable;
using System.Net;
using Azure;
using Azure.Data.Tables;
using AzureTestResources.Common;
using Microsoft.Extensions.Logging;

namespace AzureTestResources.Data.Tables;

public class CosmosDbTableService : IAzureService<CosmosTestTable>
{
  private static readonly IReadOnlyList<int> CreateResourceRetryCodes =
    new List<int>
    {
      (int)HttpStatusCode.ServiceUnavailable,
      (int)HttpStatusCode.InternalServerError,
      (int)HttpStatusCode.Conflict
    }.ToImmutableArray();

  private readonly CancellationToken _cancellationToken;
  private readonly CosmosTestTableConfig _config;
  private readonly ILogger _logger;
  private readonly TableServiceClient _client;

  public CosmosDbTableService(CosmosTestTableConfig config, ILogger logger, TableServiceClient client, CancellationToken cancellationToken)
  {
    _config = config;
    _logger = logger;
    _client = client;
    _cancellationToken = cancellationToken;
  }

  public async Task<ICreateAzureResourceResponse<CosmosTestTable>> CreateResourceInstance()
  {
    try
    {
      var tableName = TestResourceNamingConvention.GenerateResourceId(_config.NamePrefix);
      _logger.Creating("table", tableName);
      var response = await _client.CreateTableAsync(tableName, _cancellationToken);
      return new CreateCosmosTableResponse(_config, _logger, response, _client, tableName);
    }
    catch (RequestFailedException e) when (CreateResourceRetryCodes.Contains(e.Status))
    {
      return new ResourceCouldNotBeCreatedResponse<CosmosTestTable>(e, true);
    }
  }
}