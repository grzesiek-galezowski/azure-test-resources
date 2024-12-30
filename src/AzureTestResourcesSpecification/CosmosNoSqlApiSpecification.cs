using DotNet.Testcontainers.Containers;
using Extensions.Logging.NUnit;
using TddXt.AzureTestResources.Cosmos;
using TddXt.AzureTestResources.Cosmos.ImplementationDetails;

namespace TddXt.AzureTestResourcesSpecification;

public class CosmosNoSqlApiSpecification
{
  private IContainer _container;
  private Lazy<Task> _deleteAllDatabases;
  private CosmosTestDatabaseConfig EmulatorDocumentApiConfig => CosmosTestDatabaseConfig.WithPort(_container.GetMappedPublicPort(CosmosTestDatabaseConfig.DefaultPortNumber));

  [OneTimeSetUp]
  public async Task SetUpEmulator()
  {
    _container = await DockerContainersForTests.StartCosmosDbContainer2();

    _deleteAllDatabases = new(() => ZombieDatabaseCleanup.DeleteZombieDatabases(
      EmulatorDocumentApiConfig,
      new NUnitLogger("test")));
  }


  [OneTimeTearDown]
  public async Task TearDownEmulator()
  {
    await _container.DisposeAsync();
  }

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

    await using var db = await CosmosDbResources.CreateDatabase(EmulatorDocumentApiConfig, new NUnitLogger("test"));
    await db.CreateContainer(x.ToString(), "/id");

    using var cosmosClient = CosmosClientFactory.CreateCosmosClient(EmulatorDocumentApiConfig);
    var container = cosmosClient.GetContainer(db.Name, x.ToString());
    await container.CreateItemAsync(new
    {
      id = Guid.NewGuid().ToString(),
      value = "lol"
    });
  }
}