using AzureTestResources.AzureStorage.Common;
using AzureTestResources.CosmosDbNoSqlApi.ImplementationDetails;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace AzureTestResources.CosmosDbNoSqlApi;

public class CreateCosmosDatabaseResponse : ICreateAzureResourceResponse<CosmosTestDatabase>
{
  private readonly CancellationToken _cancellationToken;
  private readonly DatabaseResponse _databaseResponse;
  private readonly ILogger _logger;
  private readonly CosmosTestDatabaseConfig _config;

  public CreateCosmosDatabaseResponse(
    CosmosTestDatabaseConfig config, 
    ILogger logger, 
    DatabaseResponse databaseResponse, 
    CancellationToken cancellationToken)
  {
    _config = config;
    _logger = logger;
    _databaseResponse = databaseResponse;
    _cancellationToken = cancellationToken;
  }

  public void AssertValidResponse()
  {
    //bug assert on status code?
  }

  public bool ShouldBeRetried()
  {
    return false;
  }

  public string GetReasonForRetry()
  {
    return "None";
  }

  public CosmosTestDatabase CreateResourceApi()
  {
    return new CosmosTestDatabase(
      _databaseResponse.Database,
      _logger,
      CosmosDbRequestPolicyFactory.CreateCreateSubResourcePolicy(_logger),
      _config.ConnectionString,
      _cancellationToken);
  }
}