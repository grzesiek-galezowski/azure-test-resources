using AzureTestResources.Common;
using AzureTestResources.Cosmos.ImplementationDetails;
using Microsoft.Extensions.Logging;

namespace AzureTestResources.Cosmos;

public static class ZombieDatabaseCleanup
{
  //bug pick tolerance based on emulator/cloud resource
  public static async Task DeleteZombieDatabases(CosmosTestDatabaseConfig config, ILogger logger)
  {
    await DeleteZombieDatabases(config, AzureResources.DefaultZombieToleranceForEmulator, logger);
  }

  private static async Task DeleteZombieDatabases(CosmosTestDatabaseConfig config, TimeSpan tolerance, ILogger logger)
  {
    using var client = CosmosClientFactory.CreateCosmosClient(config);

    await new ZombieResourceCleanupLoop(new CreatedDatabasesPool(client, logger), tolerance, logger)
      .Execute();
  }
}