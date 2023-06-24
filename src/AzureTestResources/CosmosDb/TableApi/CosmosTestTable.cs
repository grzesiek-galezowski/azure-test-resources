using Azure.Data.Tables;
using AzureTestResources.Common;
using Microsoft.Extensions.Logging;

namespace AzureTestResources.CosmosDb.TableApi;

public class CosmosTestTable : IAzureResourceApi
{
  public string Name { get; }
  public string ConnectionString { get; }
  private readonly TableServiceClient _client;
  private readonly ILogger _logger;

  public CosmosTestTable(string name, TableServiceClient client, string connectionString, ILogger logger)
  {
    Name = name;
    _client = client;
    _logger = logger;
    ConnectionString = connectionString;
  }

  public async ValueTask DisposeAsync()
  {
    _logger.LogInformation("Deleting table " + Name);
    await _client.DeleteTableAsync(Name);
  }
}