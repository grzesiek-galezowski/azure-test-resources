namespace AzureTestResourcesSpecification;

public record CosmosTestDatabaseConfig(
  string NamePrefix,
  string AccountEndpoint,
  string PrimaryKey,
  int MaxRetryAttemptsOnThrottledRequests,
  TimeSpan MaxRetryWaitTimeOnThrottledRequests,
  TimeSpan RequestTimeout)
{
  public static CosmosTestDatabaseConfig Default()
  {
    return new CosmosTestDatabaseConfig(
      "test-db",
      "https://localhost:8081",
      "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
      100,
      TimeSpan.FromSeconds(10),
      TimeSpan.FromSeconds(30));
  }
}