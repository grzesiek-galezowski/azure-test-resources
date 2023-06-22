using Azure.Data.Tables;
using AzureTestResources.AzureStorage.Common;
using Microsoft.Extensions.Logging;

namespace AzureTestResources.CosmosDbTableApi;

public static class CosmosDbTableResources
{
  public static Task<CosmosTestTable> CreateTable(ILogger logger) => CreateTable(CosmosTestTableConfig.Default(), logger);

  private static async Task<CosmosTestTable> CreateTable(CosmosTestTableConfig config, ILogger logger)
  {
    var cosmosDbTableService = new CosmosDbTableService(
      config, 
      logger, 
      new TableServiceClient(config.ConnectionString));

    var api = await AzureResources.CreateApiToUnderlyingResource(cosmosDbTableService, logger);


    return api;
  }
}