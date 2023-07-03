using Azure.Data.Tables;
using AzureTestResources.Data.Tables;
using Extensions.Logging.NUnit;

namespace AzureTestResourcesSpecification;

internal class CosmosTableApiSpecification
{
  private static readonly Lazy<Task> CleanupZombieTablesOnce =
    new(() => ZombieTableCleanup.DeleteZombieTables(CosmosTestTableConfig.Default(), new NUnitLogger("table")));

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
    await CleanupZombieTablesOnce.Value;
    var cancellationToken = new CancellationToken();

    //GIVEN
    await using var table = await CosmosDbTableResources.CreateTable(new NUnitLogger("table"), cancellationToken);

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