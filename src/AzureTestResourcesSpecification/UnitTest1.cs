using AzureTestResources;

namespace AzureTestResourcesSpecification;

public class UnitTest1
{
  private readonly Lazy<Task> _deleteAllDatabases = new(CosmosTestDatabase.DeleteAllDatabases);

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
  public async Task Test1(int x)
  {
    await _deleteAllDatabases.Value;

    await using var db = await CosmosTestDatabase.CreateDatabase();
    await db.CreateContainer(x.ToString(), "/id");
  }
}