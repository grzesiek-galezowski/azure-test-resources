using System.Collections.Immutable;
using System.Net;
using Azure.Data.Tables;
using AzureTestResources.Common;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace AzureTestResources.CosmosDbTableApi;

public class CosmosDbTableService : IAzureService<CosmosTestTable>
{
  private static readonly IReadOnlyList<HttpStatusCode> CreateResourceRetryCodes =
    new List<HttpStatusCode>
    {
      HttpStatusCode.ServiceUnavailable,
      HttpStatusCode.InternalServerError,
      HttpStatusCode.Conflict
    }.ToImmutableArray();

  public CosmosDbTableService(CosmosTestTableConfig config, ILogger logger, TableServiceClient client)
  {
    Config = config;
    Logger = logger;
    Client = client;
  }

  private CosmosTestTableConfig Config { get; }
  private ILogger Logger { get; }
  private TableServiceClient Client { get; }

  public async Task<ICreateAzureResourceResponse<CosmosTestTable>> CreateResourceInstance()
  {
    try
    {
      var tableName = TestResourceNamingConvention.GenerateResourceId(Config.NamePrefix);
      var response = await Client.CreateTableAsync(tableName);
      return new CreateCosmosTableResponse(Config, Logger, response, Client, tableName);
    }
    catch (CosmosException e) when (CreateResourceRetryCodes.Contains(e.StatusCode))
    {
      return new ResourceCouldNotBeCreatedResponse<CosmosTestTable>(e, true);
    }
  }
}