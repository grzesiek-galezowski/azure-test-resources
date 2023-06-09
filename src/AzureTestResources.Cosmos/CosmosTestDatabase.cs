using AzureTestResources.Common;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Polly.Retry;

namespace AzureTestResources.Cosmos;

public class CosmosTestDatabase : IAzureResourceApi
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
    Name = _database.Id;
  }

  public string ConnectionString { get; }
  public string Name { get; }

  public async ValueTask DisposeAsync()
  {
    foreach (var container in _containers)
    {
      _logger.Deleting("container", _database.Id);
      await container.DeleteContainerAsync(cancellationToken: _cancellationToken);
      _logger.Deleted("container", _database.Id);
    }

    _logger.Deleting("database", _database.Id);
    await _database.DeleteAsync(cancellationToken: _cancellationToken);
    _logger.Deleted("database", _database.Id);
  }

  public async Task CreateContainer(string containerName, string partitionKey)
  {
    //bug handle this using the generic mechanism?
    var containerResponse = await _createResourcePolicy.ExecuteAsync(() =>
      _database.CreateContainerAsync(
        containerName,
        partitionKey,
        cancellationToken: _cancellationToken)
    );
    _containers.Add(containerResponse.Container);
  }
}