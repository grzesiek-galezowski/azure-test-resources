using Azure.Data.Tables;
using Microsoft.Extensions.Logging;

namespace AzureTestResources.CosmosDbTableApi;

public class CosmosTestTable : IAsyncDisposable
{
  public string TableId { get; }
  public string ConnectionString { get; }
  private readonly TableServiceClient _client;
  private readonly ILogger _logger;

  private CosmosTestTable(string tableId, TableServiceClient client, string connectionString, ILogger logger)
  {
    TableId = tableId;
    _client = client;
    _logger = logger;
    ConnectionString = connectionString;
  }

  public static Task<CosmosTestTable> Create(ILogger logger) => Create(CosmosTestTableConfig.Default(), logger);

  private static async Task<CosmosTestTable> Create(CosmosTestTableConfig config, ILogger logger)
  {
    var client = new TableServiceClient(config.ConnectionString);

    var table = await CosmosDbTableRequestPolicyFactory.CreateCreateResourcePolicy(logger).ExecuteAsync(
      async () =>
      {
        var tableId = TestResourceNamingConvention.GenerateResourceId(config.NamePrefix);
        var table = await client.CreateTableAsync(tableId);
        if (!table.HasValue)
        {
          throw new InvalidOperationException("Could not create a table " + tableId);
        }

        if (tableId != table.Value.Name)
        {
          throw new InvalidOperationException("Naming mismatch");
        }

        return table.Value;
      });
    return new CosmosTestTable(table.Name, client, config.ConnectionString, logger);
  }

  public async ValueTask DisposeAsync()
  {
    _logger.LogInformation("Deleting table " + TableId);
    await _client.DeleteTableAsync(TableId);
  }
}