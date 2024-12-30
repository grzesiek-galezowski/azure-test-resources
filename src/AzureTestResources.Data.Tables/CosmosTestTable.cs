using Azure.Data.Tables;
using Microsoft.Extensions.Logging;
using TddXt.AzureTestResources.Common;

namespace TddXt.AzureTestResources.Data.Tables;

public class CosmosTestTable(string name, TableServiceClient client, string connectionString, ILogger logger)
  : IAzureResourceApi
{
  public string Name { get; } = name;
  public string ConnectionString { get; } = connectionString;

  public async ValueTask DisposeAsync()
  {
    logger.LogInformation("Deleting table " + Name);
    await client.DeleteTableAsync(Name);
  }
}