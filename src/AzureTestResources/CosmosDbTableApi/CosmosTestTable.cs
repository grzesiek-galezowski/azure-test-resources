using Azure.Data.Tables;
using Microsoft.Extensions.Logging;

namespace AzureTestResources.CosmosDbTableApi;

public class CosmosTestTable : IAsyncDisposable
{
  private readonly string _tableId;
  private readonly TableServiceClient _client;

  private CosmosTestTable(string tableId, TableServiceClient client)
  {
    _tableId = tableId;
    _client = client;
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
      return new CosmosTestTable(table.Name, client);
  }

  public async ValueTask DisposeAsync()
  {
    await _client.DeleteTableAsync(_tableId);
  }
}