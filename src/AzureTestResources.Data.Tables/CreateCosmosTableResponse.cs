using Azure;
using Azure.Data.Tables;
using Azure.Data.Tables.Models;
using Microsoft.Extensions.Logging;
using TddXt.AzureTestResources.Common;

namespace TddXt.AzureTestResources.Data.Tables;

public class CreateCosmosTableResponse(
  CosmosTestTableConfig config,
  ILogger logger,
  Response<TableItem> response,
  TableServiceClient client,
  string tableName)
  : ICreateAzureResourceResponse<CosmosTestTable>
{
  public void AssertResourceCreated()
  {
    Assertions.AssertNotNull(response, "database", tableName);
    Assertions.AssertIsHttpCreated(response, "database");
    Assertions.AssertNamesMatch(tableName, response.Value.Name);
  }

  public bool ShouldBeRetried() => false;
  public string GetReasonForRetry() => "None";

  public CosmosTestTable CreateResourceApi()
  {
    return new CosmosTestTable(tableName, client, config.ConnectionString, logger);
  }
}