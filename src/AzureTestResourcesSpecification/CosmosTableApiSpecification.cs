using Azure.Data.Tables;
using Extensions.Logging.NUnit;
using FluentAssertions.Extensions;
using TddXt.AzureTestResources.Data.Tables;
using Testcontainers.CosmosDb;

namespace TddXt.AzureTestResourcesSpecification;

internal class CosmosTableApiSpecification
{
  private CosmosDbContainer _container;
  private Lazy<Task> _cleanupAllTables;

  [OneTimeSetUp]
  public async Task SetUpEmulator()
  {
    _container = await DockerContainersForTests.StartCosmosDbContainer();

    _cleanupAllTables = new(() => ZombieTableCleanup.DeleteZombieTables(
      CosmosTestTableConfig.Default(), 
      new NUnitLogger("test")));
  }

  [OneTimeTearDown]
  public async Task TearDownEmulator()
  {
    TestContext.Progress.WriteLine(await _container.GetLogsAsync());
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
  public async Task ShouldCreateTestTable(int testNo)
  {
    await _cleanupAllTables.Value.WaitAsync(1.Minutes()); //bug
    var cancellationToken = new CancellationTokenSource().Token;

    //GIVEN
    await using var table = await CosmosDbTableResources.CreateTable(
      CosmosTestTableConfig.Default(), 
      new NUnitLogger("table"), cancellationToken);

    //WHEN
    var tableClient = new TableServiceClient(table.ConnectionString);
    await tableClient.GetTableClient(table.Name).AddEntityAsync(new TableEntity
    {
      PartitionKey = testNo.ToString(),
      RowKey = "lol"
    }, cancellationToken: cancellationToken);

    //THEN
  }
}