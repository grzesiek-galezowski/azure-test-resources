using Azure.Data.Tables;
using AzureTestResources.Common;
using Microsoft.Extensions.Logging;

namespace AzureTestResources.Data.Tables;

public static class CosmosDbTableResources
{
  public static Task<CosmosTestTable> CreateTable(ILogger logger) => CreateTable(CosmosTestTableConfig.Default(), logger);

  private static async Task<CosmosTestTable> CreateTable(CosmosTestTableConfig config, ILogger logger)
  {
    var cosmosDbTableService = new CosmosDbTableService(
      config,
      logger,
      new TableServiceClient(config.ConnectionString));

    var api = await AzureResources.CreateApiToUnderlyingResource(cosmosDbTableService, "table", logger);


    return api;
  }
}