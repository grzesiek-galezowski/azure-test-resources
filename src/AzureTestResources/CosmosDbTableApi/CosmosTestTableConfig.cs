namespace AzureTestResources.CosmosDbTableApi;

public record CosmosTestTableConfig(string ConnectionString, string NamePrefix)
{
  public static CosmosTestTableConfig Default() =>
    new(
      "DefaultEndpointsProtocol=http;" +
      "AccountName=localhost;" +
      "AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==;" +
      "TableEndpoint=http://localhost:8902/;",
      "test-table");
}