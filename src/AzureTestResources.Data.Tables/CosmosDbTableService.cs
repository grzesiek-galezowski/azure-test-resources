using System.Collections.Immutable;
using System.Net;
using Azure;
using Azure.Data.Tables;
using Microsoft.Extensions.Logging;
using TddXt.AzureTestResources.Common;

namespace TddXt.AzureTestResources.Data.Tables;

public class CosmosDbTableService(
  CosmosTestTableConfig config,
  ILogger logger,
  TableServiceClient client,
  CancellationToken cancellationToken)
  : IAzureService<CosmosTestTable>
{
  private static readonly IReadOnlyList<int> CreateResourceRetryCodes =
    new List<int>
    {
      (int)HttpStatusCode.ServiceUnavailable,
      (int)HttpStatusCode.InternalServerError,
      (int)HttpStatusCode.Conflict
    }.ToImmutableArray();

  public async Task<ICreateAzureResourceResponse<CosmosTestTable>> CreateResourceInstance()
  {
    try
    {
      var tableName = TestResourceNamingConvention.GenerateResourceId(config.NamePrefix);
      logger.Creating("table", tableName);
      var response = await client.CreateTableAsync(tableName, cancellationToken);
      return new CreateCosmosTableResponse(config, logger, response, client, tableName);
    }
    catch (RequestFailedException e) when (CreateResourceRetryCodes.Contains(e.Status))
    {
      return new ResourceCouldNotBeCreatedResponse<CosmosTestTable>(e, true);
    }
  }
}