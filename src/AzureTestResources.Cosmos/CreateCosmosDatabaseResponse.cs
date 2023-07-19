using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using TddXt.AzureTestResources.Common;
using TddXt.AzureTestResources.Cosmos.ImplementationDetails;

namespace TddXt.AzureTestResources.Cosmos;

public class CreateCosmosDatabaseResponse : ICreateAzureResourceResponse<CosmosTestDatabase>
{
  private readonly CancellationToken _cancellationToken;
  private readonly DatabaseResponse _databaseResponse;
  private readonly string _dbName;
  private readonly ILogger _logger;
  private readonly CosmosTestDatabaseConfig _config;

  public CreateCosmosDatabaseResponse(
    CosmosTestDatabaseConfig config,
    ILogger logger,
    DatabaseResponse databaseResponse,
    string dbName,
    CancellationToken cancellationToken)
  {
    _config = config;
    _logger = logger;
    _databaseResponse = databaseResponse;
    _dbName = dbName;
    _cancellationToken = cancellationToken;
  }

  public void AssertResourceCreated()
  {
    CosmosDbAssertions.AssertIsHttpCreated(_databaseResponse, "database");
    CosmosDbAssertions.AssertNamesMatch(_dbName, _databaseResponse.Resource.Id);
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
      _databaseResponse.Database.Id,
      _logger,
      CosmosDbRequestPolicyFactory.CreateCreateSubResourcePolicy(_logger),
      _cancellationToken,
      _config);
  }
}