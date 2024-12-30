namespace TddXt.AzureTestResources.Cosmos;

public record CosmosTestDatabaseConfig(
  string NamePrefix,
  string ConnectionString,
  int MaxRetryAttemptsOnThrottledRequests,
  TimeSpan MaxRetryWaitTimeOnThrottledRequests,
  TimeSpan RequestTimeout)
{
  public const int DefaultPortNumber = 8081;
  public static CosmosTestDatabaseConfig Default() => WithPort(DefaultPortNumber);
  public static CosmosTestDatabaseConfig WithPort(int portNumber)
  {
    return new CosmosTestDatabaseConfig(
      "testDb",
      $"AccountEndpoint=https://localhost:{portNumber}/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==;",
      100,
      TimeSpan.FromSeconds(10),
      TimeSpan.FromSeconds(30));
  }

}