using Azure.Data.Tables;
using Microsoft.Extensions.Logging;

namespace AzureTestResources.CosmosDbTableApi;

public class CosmosTestTable : IAsyncDisposable
{
  private readonly string _tableId;
  private readonly TableServiceClient _client;
  private readonly ILogger _logger;

  private CosmosTestTable(string tableId, TableServiceClient client, ILogger logger)
  {
    _tableId = tableId;
    _client = client;
    _logger = logger;
  }

  public static Task<CosmosTestTable> Create(ILogger logger) => Create(CosmosTestTableConfig.Default(), logger);

  private static async Task<CosmosTestTable> Create(CosmosTestTableConfig config, ILogger logger)
  {
    var client = new TableServiceClient(config.ConnectionString);

    var table = await CosmosDbTableRequestPolicyFactory.CreateCreateResourcePolicy(logger).ExecuteAsync(
      async () =>
      {
        var tableId = TestResourceNamingConvention.GenerateDatabaseId(config.NamePrefix);
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
    return new CosmosTestTable(table.Name, client, logger);
  }

  public async ValueTask DisposeAsync()
  {
    _logger.LogInformation("Deleting table " + _tableId);
    await _client.DeleteTableAsync(_tableId);
  }
}