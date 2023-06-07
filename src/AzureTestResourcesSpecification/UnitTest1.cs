namespace AzureTestResourcesSpecification;

public class UnitTest1
{
  private Lazy<Task> DeleteAllDatabases = new(CosmosTestDatabase.DeleteAllDatabases);

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
    await DeleteAllDatabases.Value;

    Console.WriteLine("1");
    await using var db = await CosmosTestDatabase.CreateDatabase();
    Console.WriteLine("2");
    await db.CreateContainer(x.ToString(), "/id");
    Console.WriteLine("3");
  }
}