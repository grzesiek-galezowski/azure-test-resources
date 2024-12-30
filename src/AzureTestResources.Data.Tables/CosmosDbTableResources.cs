using Azure.Data.Tables;
using Microsoft.Extensions.Logging;
using TddXt.AzureTestResources.Common;

namespace TddXt.AzureTestResources.Data.Tables;

public static class CosmosDbTableResources
{
  public static Task<CosmosTestTable> CreateTable(ILogger logger, CancellationToken cancellationToken) => CreateTable(CosmosTestTableConfig.Default(), logger, cancellationToken);

  public static async Task<CosmosTestTable> CreateTable(
    CosmosTestTableConfig config, ILogger logger, CancellationToken cancellationToken)
  {
    var cosmosDbTableService = new CosmosDbTableService(
      config,
      logger,
      new TableServiceClient(config.ConnectionString),
      cancellationToken);

    var api = await AzureResources.CreateApiToUnderlyingResource(cosmosDbTableService, "table", logger);


    return api;
  }
}