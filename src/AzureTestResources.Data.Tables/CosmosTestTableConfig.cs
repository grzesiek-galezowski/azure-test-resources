namespace TddXt.AzureTestResources.Data.Tables;

public record CosmosTestTableConfig(string ConnectionString, string NamePrefix)
{
  public const int DefaultPortNumber = 8902;
  public static CosmosTestTableConfig Default() => WithPort(DefaultPortNumber);
  public static CosmosTestTableConfig WithPort(int portNumber) =>
    new(
      $"DefaultEndpointsProtocol=http;AccountName=localhost;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==;TableEndpoint=http://localhost:{portNumber}/;",
      "testTable");
}