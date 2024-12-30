using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using TddXt.AzureTestResources.Common;
using TddXt.AzureTestResources.Cosmos.ImplementationDetails;

namespace TddXt.AzureTestResources.Cosmos;

public class CreateCosmosDatabaseResponse(
  CosmosTestDatabaseConfig config,
  ILogger logger,
  DatabaseResponse databaseResponse,
  string dbName,
  CancellationToken cancellationToken)
  : ICreateAzureResourceResponse<CosmosTestDatabase>
{
  public void AssertResourceCreated()
  {
    CosmosDbAssertions.AssertIsHttpCreated(databaseResponse, "database");
    CosmosDbAssertions.AssertNamesMatch(dbName, databaseResponse.Resource.Id);
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
      databaseResponse.Database.Id,
      logger,
      CosmosDbRequestPolicyFactory.CreateCreateSubResourcePolicy(logger),
      cancellationToken,
      config);
  }
}