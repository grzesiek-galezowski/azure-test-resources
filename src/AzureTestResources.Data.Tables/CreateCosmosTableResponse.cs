using Azure;
using Azure.Data.Tables;
using Azure.Data.Tables.Models;
using Microsoft.Extensions.Logging;
using TddXt.AzureTestResources.Common;

namespace TddXt.AzureTestResources.Data.Tables;

public class CreateCosmosTableResponse : ICreateAzureResourceResponse<CosmosTestTable>
{
  private readonly TableServiceClient _client;
  private readonly string _tableName;
  private readonly Response<TableItem> _response;
  private readonly ILogger _logger;
  private readonly CosmosTestTableConfig _config;

  public CreateCosmosTableResponse(CosmosTestTableConfig config,
    ILogger logger,
    Response<TableItem> response,
    TableServiceClient client,
    string tableName)
  {
    _config = config;
    _logger = logger;
    _response = response;
    _client = client;
    _tableName = tableName;
  }

  public void AssertResourceCreated()
  {
    Assertions.AssertNotNull(_response, "database", _tableName);
    Assertions.AssertIsHttpCreated(_response, "database");
    Assertions.AssertNamesMatch(_tableName, _response.Value.Name);
  }

  public bool ShouldBeRetried() => false;
  public string GetReasonForRetry() => "None";

  public CosmosTestTable CreateResourceApi()
  {
    return new CosmosTestTable(_tableName, _client, _config.ConnectionString, _logger);
  }
}