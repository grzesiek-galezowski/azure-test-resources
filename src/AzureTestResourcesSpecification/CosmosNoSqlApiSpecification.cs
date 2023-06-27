using AzureTestResources.CosmosDb.NoSqlApi;
using Extensions.Logging.NUnit;
using Microsoft.Azure.Cosmos;

namespace AzureTestResourcesSpecification;

public class CosmosNoSqlApiSpecification
{
  private readonly Lazy<Task> _deleteAllDatabases
    = new(() => ZombieDatabaseCleanup.DeleteZombieDatabases(CosmosTestDatabaseConfig.Default()));

  [TestCase(1)]
  [TestCase(2)]
  [TestCase(3)]
  [TestCase(4)]
  [TestCase(5)]
  [TestCase(6)]
  [TestCase(7)]
  [TestCase(8)]
  [TestCase(9)]
  [TestCase(10)]
  [TestCase(11)]
  [TestCase(12)]
  [TestCase(13)]
  [TestCase(14)]
  [TestCase(15)]
  [TestCase(16)]
  [TestCase(21)]
  [TestCase(22)]
  [TestCase(23)]
  [TestCase(24)]
  [TestCase(25)]
  [TestCase(26)]
  [TestCase(27)]
  [TestCase(28)]
  [TestCase(29)]
  [TestCase(30)]
  [TestCase(31)]
  [TestCase(32)]
  [TestCase(33)]
  [TestCase(34)]
  [TestCase(35)]
  [TestCase(36)]
  public async Task ShouldCreateTestDb(int x)
  {
    await _deleteAllDatabases.Value;

    await using var db = await CosmosDbResources.CreateDatabase(new NUnitLogger("test"));
    await db.CreateContainer(x.ToString(), "/id");

    using var cosmosClient = new CosmosClient(db.ConnectionString);
    var container = cosmosClient.GetContainer(db.Name, x.ToString());
    await container.CreateItemAsync(new
    {
      id = Guid.NewGuid().ToString(),
      value = "lol"
    });
  }
}