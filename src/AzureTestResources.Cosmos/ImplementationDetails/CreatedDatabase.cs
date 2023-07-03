using System.Net;
using AzureTestResources.Common;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace AzureTestResources.Cosmos.ImplementationDetails;

public class CreatedDatabase : ICreatedResource
{
  private readonly DatabaseProperties _databaseProperties;
  private readonly CosmosClient _cosmosClient;
  private readonly ILogger _logger;

  public CreatedDatabase(DatabaseProperties databaseProperties, CosmosClient cosmosClient, ILogger logger)
  {
    _databaseProperties = databaseProperties;
    _cosmosClient = cosmosClient;
    _logger = logger;
  }

  public string Name => _databaseProperties.Id;
  public async Task DeleteAsync()
  {
    try
    {
      var database = _cosmosClient.GetDatabase(_databaseProperties.Id);
      await DeleteContainers(database, _cosmosClient);
      await database.DeleteAsync();
    }
    catch (CosmosException e) when (e.StatusCode == HttpStatusCode.NotFound)
    {
      throw new ResourceCouldNotBeDeletedException(e);
    }
  }

  /// <summary>
  /// The whole point of deleting the containers first is that the cosmos db emulator
  /// has a limit on container count, so we're trying to trim this as soon as possible.
  /// </summary>
  private async Task DeleteContainers(Database database, CosmosClient client)
  {
    var containerPropertiesList = await DatabaseInformation.GetContainerList(database);
    _logger.LogInformation($"Database has {containerPropertiesList.Count} containers");
    foreach (var containerProperties in containerPropertiesList)
    {
      try
      {
        _logger.LogInformation($"Deleting container {containerProperties.Id}");
        var container = client.GetContainer(database.Id, containerProperties.Id);
        await container.DeleteContainerAsync();
        _logger.LogInformation($"Deleted container {containerProperties.Id}");
      }
      catch (CosmosException e) when (e.StatusCode == HttpStatusCode.NotFound)
      {
        _logger.LogWarning($"Container {containerProperties.Id} already deleted");
      }
    }
  }
}