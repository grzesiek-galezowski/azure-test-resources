using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Polly.Retry;

namespace AzureTestResources.CosmosDbNoSqlApi;

public class CosmosTestDatabase : IAsyncDisposable
{
  private readonly CancellationToken _cancellationToken;
  private readonly List<Container> _containers = new();
  private readonly AsyncRetryPolicy _createResourcePolicy;
  private readonly Database _database;
  private readonly ILogger _logger;

  public CosmosTestDatabase(Database database,
    ILogger logger,
    AsyncRetryPolicy createResourcePolicy,
    string connectionString,
    CancellationToken cancellationToken)
  {
    _database = database;
    _logger = logger;
    _createResourcePolicy = createResourcePolicy;
    _cancellationToken = cancellationToken;
    ConnectionString = connectionString;
    Id = _database.Id;
  }

  public string ConnectionString { get; }
  public string Id { get; private set; }

  public async ValueTask DisposeAsync()
  {
    foreach (var container in _containers)
    {
      _logger.LogInformation($"Deleting container {container.Id}");
      await container.DeleteContainerAsync(cancellationToken: _cancellationToken);
    }

    _logger.LogInformation($"Deleting database {_database.Id}");
    await _database.DeleteAsync(cancellationToken: _cancellationToken);
  }

  public async Task CreateContainer(string containerName, string partitionKey)
  {
    var containerResponse = await _createResourcePolicy.ExecuteAsync(() =>
      _database.CreateContainerAsync(
        containerName,
        partitionKey,
        cancellationToken: _cancellationToken)
    );
    _containers.Add(containerResponse.Container);
  }
}