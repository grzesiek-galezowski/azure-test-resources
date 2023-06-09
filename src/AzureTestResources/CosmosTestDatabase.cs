using System.Collections.Immutable;
using System.Net;
using System.Text.RegularExpressions;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace AzureTestResources;

public class CosmosTestDatabase : IAsyncDisposable
{
  private const string NameDelimiter = "_";
  private readonly Database _database;
  private readonly ILogger _logger;
  private readonly AsyncRetryPolicy _createResourcePolicy;
  private readonly CancellationToken _cancellationToken;

  private static readonly IReadOnlyList<HttpStatusCode> CreateDatabaseRetryCodes = 
    new List<HttpStatusCode>()
  {
    HttpStatusCode.ServiceUnavailable,
    HttpStatusCode.InternalServerError,
    HttpStatusCode.Conflict
  }.ToImmutableArray();

  private static readonly IReadOnlyList<HttpStatusCode> CreateContainerRetryCodes = 
    new List<HttpStatusCode>()
  {
    HttpStatusCode.ServiceUnavailable,
    HttpStatusCode.InternalServerError,
  }.ToImmutableArray();


  private static AsyncRetryPolicy CreateCreateDatabasePolicy(ILogger logger)
  {
    return CreateCreateResourcePolicy(logger, CreateDatabaseRetryCodes);
  }
  
  private static AsyncRetryPolicy CreateCreateContainerPolicy(ILogger logger)
  {
    return CreateCreateResourcePolicy(logger, CreateContainerRetryCodes);
  }

  private static AsyncRetryPolicy CreateCreateResourcePolicy(ILogger logger, IReadOnlyList<HttpStatusCode> createResourceRetryCodes)
  {
    return Policy.Handle<CosmosException>(
        e => createResourceRetryCodes.Any(c => c == e.StatusCode))
      .WaitAndRetryAsync(10,
        _ => TimeSpan.FromSeconds(10),
        (exception, _, retryCount, _) =>
        {
          logger.LogWarning($"Retry {retryCount} due to status code {((CosmosException)exception).StatusCode}");
        });
  }

  private readonly List<Container> _containers = new();
  private static readonly TimeSpan DefaultTolerance = TimeSpan.FromMinutes(2);

  public static async Task DeleteZombieDatabases()
  {
    await DeleteZombieDatabases(DefaultTolerance);
  }

  private static async Task DeleteZombieDatabases(TimeSpan tolerance)
  {
    using var client = CosmosClientFactory.CreateCosmosClient(
      CosmosTestDatabaseConfig.Default());

    var databaseList = await GetDatabaseList(client);

    foreach (var databaseProperties in databaseList
               .Where(AdheresToNamingConvention)
               .Where(properties => IsCreatedEarlierFromNowThan(
                 tolerance,
                 properties)))
    {
      try
      {
        //bug make this more efficient
        var database = client.GetDatabase(databaseProperties.Id);
        await database.DeleteAsync();
      }
      catch (CosmosException e) when (e.StatusCode == HttpStatusCode.NotFound)
      {
        //silently ignore these databases
      }
    }
  }

  static async Task<List<DatabaseProperties>> GetDatabaseList(CosmosClient cosmosClient)
  {
    var databases = new List<DatabaseProperties>();

    // Create a database iterator
    var databaseIterator = cosmosClient.GetDatabaseQueryIterator<DatabaseProperties>();

    while (databaseIterator.HasMoreResults)
    {
      var response = await databaseIterator.ReadNextAsync();
      databases.AddRange(response);
    }

    return databases;
  }

  public static async Task<CosmosTestDatabase> CreateDatabase(ILogger log)
  {
    var obj = CosmosTestDatabaseConfig.Default();

    return await CreateDatabase(obj, log);
  }

  public static async Task<CosmosTestDatabase> CreateDatabase(
    CosmosTestDatabaseConfig config,
    ILogger logger)
  {
    var client = CosmosClientFactory.CreateCosmosClient(config);
    var cancellationToken = new CancellationToken();

    var databaseResponse = await CreateCreateDatabasePolicy(logger).ExecuteAsync(() =>
      client.CreateDatabaseAsync(
        GenerateDatabaseId(config.NamePrefix),
        cancellationToken: cancellationToken)
    );

    return new CosmosTestDatabase(
      databaseResponse.Database, 
      logger, 
      CreateCreateContainerPolicy(logger),
      cancellationToken);
  }

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

  private CosmosTestDatabase(Database database,
    ILogger logger,
    AsyncRetryPolicy createResourcePolicy,
    CancellationToken cancellationToken)
  {
    _database = database;
    _logger = logger;
    _createResourcePolicy = createResourcePolicy;
    _cancellationToken = cancellationToken;
  }

  private static string GenerateDatabaseId(string namePrefix)
  {
    var utcNow = DateTime.UtcNow;
    var dbName = $"{namePrefix}{NameDelimiter}{utcNow.Ticks}{NameDelimiter}{Guid.NewGuid()}";
    AssertCreationTimeCanBeParsedBackFrom(dbName, utcNow);
    return dbName;
  }

  private static void AssertCreationTimeCanBeParsedBackFrom(string dbName, DateTime utcNow)
  {
    var timeOfCreation = GetTimeOfCreationOf(dbName);
    if (timeOfCreation != utcNow)
    {
      throw new Exception( //bug
        $"Could not get the correct time of creation from {dbName}. " +
        $"Expected {utcNow} but got {timeOfCreation}");
    }
  }

  private static DateTime GetTimeOfCreationOf(string dbName)
  {
    var ticksString = dbName.Split(NameDelimiter)[^2];
    var ticks = long.Parse(ticksString);
    return new DateTime(ticks, DateTimeKind.Utc);
  }

  private static bool IsCreatedEarlierFromNowThan(TimeSpan tolerance, DatabaseProperties d)
  {
    return GetTimeOfCreationOf(d.Id) < DateTime.UtcNow - tolerance;
  }

  private static bool AdheresToNamingConvention(DatabaseProperties properties)
  {
    return Regex.Match(properties.Id, @"^[\w]+-\d+-\w{8}-\w{4}-\w{4}-\w{4}-\w{12}$").Success;
  }
}